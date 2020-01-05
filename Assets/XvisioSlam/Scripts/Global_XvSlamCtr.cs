using Global_StructClass;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Xvisio;

public class Global_XvisioCtr : BaseGameObj
{

    #region 公有变量

    public KeyCode userTestResetKey = KeyCode.A;
    public static Global_XvisioCtr M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<Global_XvisioCtr>();
            }
            return _instance;
        }
    }
    public Data_XvisioConfig M_CurDataXvisioConfig
    {
        get
        {
            return curDataXvisioConfig;
        }
    }

    /// <summary>
    /// 第一次读到数据，此处是再设备初始化之后，第一次位置和角度都不为零的情况示为第一次读到数据
    /// </summary>
    public event Action<Vector3, Quaternion> Event_FirstReadData;
    #endregion

    #region 私有变量

    private Vector3 originalPos;
    private Vector3 originalAngles;
    private Vector3 middlePos;
    private Vector3 middleAngles;

    private bool lastImuFusionEnable = true;  // saved imuFusionEnable;
    private bool filterStatus = false;

    private float lastOffsetAdjust = 0.0f;  // saved offsetAdjust;
    private float lastPrediction = 0.0f;  // saved prediction;

    [SerializeField] Data_XvisioConfig curDataXvisioConfig;

    /// <summary>
    /// 当前累计的错误数量
    /// </summary>
    private static int curWrongStateCount = 0;
    private static Global_XvisioCtr _instance;
    private static XvHid xvHid;

    private Thread dealDeviceDataThread;
    private static Queue<Vector3> QueueFilterPos = new Queue<Vector3>();
    private static Queue<Quaternion> QueueFilterQua = new Queue<Quaternion>();
    private static Vector3 CurFilterPos = Vector3.zero;
    private static Quaternion CurFilterQua = Quaternion.identity;
    /// <summary>
    /// 滤波队列长度
    /// </summary>
  //  [SerializeField]
    private int filterQueLength = 10;

    /// <summary>
    /// 第一次读到数据
    /// </summary>
    private bool isFirstReadData = false;

    #endregion

    #region 系统方法
    private void Start()
    {
        // Init();
    }
    // Use this for initialization
    protected override void Update_State()
    {
        #region 测试

        //if (Input.GetKey(userResetKey))
        //{
        //    Reset_Device();
        //}

        #endregion
    }

    protected override void LateUpdate_State()
    {

    }

    protected override void FixedUpdate_State()
    {
        /// 此处是再设备初始化之后，第一次位置和角度都不为零的情况示为第一次读到数据
        if (!isFirstReadData && CurFilterPos != Vector3.zero && CurFilterQua != Quaternion.identity)
        {
            isFirstReadData = true;

            Event_FirstReadData(CurFilterPos, CurFilterQua);
            Debug.Log("第一次初始化设备成功读取到数据！");
        }

        transform.position = CurFilterPos;
        transform.rotation = CurFilterQua;
    }
    /// <summary>
    /// 程序退出的时候关闭线程
    /// </summary>
    private void OnApplicationQuit()
    {
        if (null != dealDeviceDataThread)
        {
            dealDeviceDataThread.Interrupt();
            dealDeviceDataThread.Abort();
        }
    }
    #endregion

    #region 私有方法
    /// <summary>
    /// 处理设备获取数据
    /// </summary>
    private void DealXvDeviceData()
    {
        while (true)
        {
            try
            {
                if (!xvHid.setup())
                {
                    continue;
                }
                EnablePostFlt_pause_6dof();
                Update_IMUConfig();
                //先更新一下
                xvHid.update();
                int tempState = xvHid.state();
                if (2 == tempState || 4 == tempState || 5 == tempState)
                {
                    curWrongStateCount = 0;

                    Vector3 tempPos = xvHid.position();
                    Quaternion tempQua = xvHid.rotation();


                    CurFilterPos = tempPos;
                    CurFilterQua = tempQua;

                    //     Debug.Log(CurFilterPos + "q" + CurFilterQua);
                }
                else
                {
                    curWrongStateCount++;
                    //    Debug.Log(curWrongStateCount);
                }
                if (curWrongStateCount >= curDataXvisioConfig.AllowedMaxWrongStateFrameNumber)
                {
                    xvHid.request6Dof(false, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
                    EnablePostFlt();
                    xvHid.resetSLAM(true);
                    Debug.Log("累计错误值超过最大值，重置设备！");
                    Thread.Sleep(TimeSpan.FromSeconds(curDataXvisioConfig.SlamResetCooldownTime));
                    xvHid.request6Dof(true, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
                    curWrongStateCount = 0;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("xvisio设备运行出错，如果一直出现这个错误，建议拔插设备并重启程序！" + e.Message);
            }
        }
    }
    /// <summary>
    /// 使用滤波算法对数据进行过滤
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="qua"></param>
    private void FilterData(Vector3 pos, Quaternion qua)
    {
        QueueFilterPos.Enqueue(pos);
        if (filterQueLength <= QueueFilterPos.Count)
        {
            QueueFilterPos.Dequeue();
            Vector3 tempFilterPos = pos;
            //计算队列的中位数
            foreach (var item in QueueFilterPos)
            {
                tempFilterPos += item;
            }
            CurFilterPos = tempFilterPos / filterQueLength;

        }
        if (QueueFilterQua.Count > 0)
        {
            //如果第一个数据和最后一个值非常接近但是符号相反,将其值的符号也赋值成一样
            if (Quaternion.Dot(QueueFilterQua.Peek(), qua) < 0)
            {
                qua = new Quaternion(-qua.x, -qua.y, -qua.z, -qua.w);
                //   Debug.Log("翻转符号突变的数据！");
            }
        }
        QueueFilterQua.Enqueue(qua);
        if (filterQueLength <= QueueFilterQua.Count)
        {
            Quaternion tempFirst = QueueFilterQua.Dequeue();
            Quaternion tempFilterQua = qua;
            foreach (var item in QueueFilterQua)
            {
                tempFilterQua.x += item.x;
                tempFilterQua.y += item.y;
                tempFilterQua.z += item.z;
                tempFilterQua.w += item.w;
            }
            CurFilterQua.x = tempFilterQua.x / filterQueLength;
            CurFilterQua.y = tempFilterQua.y / filterQueLength;
            CurFilterQua.z = tempFilterQua.z / filterQueLength;
            CurFilterQua.w = tempFilterQua.w / filterQueLength;
        }
    }
    private IEnumerator DelayExeMethod1(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        xvHid.request6Dof(true, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
    }
    private void Update_IMUConfig()
    {
        if ((curDataXvisioConfig.ImuFusionEnable != lastImuFusionEnable) ||
        (curDataXvisioConfig.OffsetAdjust != lastOffsetAdjust) || (curDataXvisioConfig.Prediction != lastPrediction))
        {
            Debug.Log("offsetAdjust: " + curDataXvisioConfig.OffsetAdjust + ", prediction: " + curDataXvisioConfig.Prediction);
            xvHid.request6Dof(false, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
            xvHid.imuConfiguration(curDataXvisioConfig.ImuFusionEnable, true, curDataXvisioConfig.OffsetAdjust, curDataXvisioConfig.Prediction);
            xvHid.request6Dof(true, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
            lastImuFusionEnable = curDataXvisioConfig.ImuFusionEnable;
            lastOffsetAdjust = curDataXvisioConfig.OffsetAdjust;
            lastPrediction = curDataXvisioConfig.Prediction;
        }
    }
    private void EnablePostFlt()
    {
        if (filterStatus != curDataXvisioConfig.PostFilterEnabled)
        {
            Debug.Log("filterStatus: " + filterStatus + "postFilterEnabled: " + curDataXvisioConfig.PostFilterEnabled);
            xvHid.enablePostFilter(curDataXvisioConfig.PostFilterEnabled, curDataXvisioConfig.FilterCoefRotation, curDataXvisioConfig.FilterCoefTranslation);
            filterStatus = curDataXvisioConfig.PostFilterEnabled;
        }
    }
    private void EnablePostFlt_pause_6dof()
    {
        if (filterStatus != curDataXvisioConfig.PostFilterEnabled)
        {
            Debug.Log("filterStatus: " + filterStatus + "postFilterEnabled: " + curDataXvisioConfig.PostFilterEnabled);
            xvHid.request6Dof(false, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
            xvHid.enablePostFilter(curDataXvisioConfig.PostFilterEnabled, curDataXvisioConfig.FilterCoefRotation, curDataXvisioConfig.FilterCoefTranslation);
            xvHid.request6Dof(true, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
            filterStatus = curDataXvisioConfig.PostFilterEnabled;
        }
    }

    #endregion

    #region 公有方法
    /// <summary>
    /// 重置设备
    /// </summary>
    public void Reset_Device()
    {
        xvHid.request6Dof(false, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
        middlePos = originalPos;
        middleAngles = originalAngles;
        EnablePostFlt();
        xvHid.resetSLAM(true);
        StartCoroutine(DelayExeMethod1(curDataXvisioConfig.SlamResetCooldownTime));
    }
    /// <summary>
    /// 初始化并启动设备
    /// </summary>
    public override void Init()
    {
        base.Init();
        string tempURL = Global_Manage.M_CurResourcesDataURL_JSON + Global_XMLCtr.M_Instance.GetElementValue("XvisioConfigJsonName");
        curDataXvisioConfig = Global_Manage.ReadData_JSON<Data_XvisioConfig>(tempURL);

        originalAngles = middleAngles = transform.eulerAngles;
        originalPos = middlePos = transform.position;
        try
        {
            xvHid = new XvHid(curDataXvisioConfig.PostFilterEnabled, curDataXvisioConfig.FilterCoefRotation,
                curDataXvisioConfig.FilterCoefTranslation, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);

            dealDeviceDataThread = new Thread(DealXvDeviceData);
            dealDeviceDataThread.IsBackground = true;
            dealDeviceDataThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("初始化Xvisio设备失败" + e.Message);
        }
    }

    #endregion
}
