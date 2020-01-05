using Gloabal_EnumCalss;
using Global_StructClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 带有畸变的摄像机管理类
/// </summary>
public class Camera_DistortionManage : BaseGameObj
{
    #region 公有变量

    /// <summary>
    /// 正交摄像机的size，主要是高度，正交相机受到屏幕分辨率的影响，
    /// 屏幕分辨率如果为宽x,高y则正交相机的size的宽高比为w/h=x/y，
    /// 其中y/h=该值
    /// </summary>
    public const float SIZERATE_CAMORTH = 2.0f;
    /// <summary>
    /// 生成的畸变网格大小和unity中的距离比例，
    /// 比如网格是144*80的则在unity中应该是144*该值为宽度
    /// </summary>
    public const float SIZERATE_DISTORTIONMESH = 20;
    #endregion

    #region 属性
    /// <summary>
    /// 当前眼镜的模式，分为单目和双目
    /// </summary>
    public EnumVisionMode M_CurVisionMode
    {
        get
        {
            return curVisionMode;
        }
    }
    public Camera_RenderDisplay M_SubObjCamRenderDisplay
    {
        get
        {
            if (null == subObjCamRenderDisplay)
            {
                subObjCamRenderDisplay = GetComponentInChildren<Camera_RenderDisplay>(true);
            }
            return subObjCamRenderDisplay;
        }
    }
    public Camera_ObtainRT M_SubObjCamObtainRT
    {
        get
        {
            if (null == subObjCamObtainRT)
            {
                subObjCamObtainRT = GetComponentInChildren<Camera_ObtainRT>(true);
            }
            return subObjCamObtainRT;
        }
    }
    public static Camera_DistortionManage M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<Camera_DistortionManage>();
            }
            return _instance;
        }
    }
    public Data_Module M_CurModule
    {
        get
        {
            return curDataModule;
        }
    }
    #endregion

    #region 私有变量

    private static Camera_DistortionManage _instance;
    private Camera_RenderDisplay subObjCamRenderDisplay;
    private Camera_ObtainRT subObjCamObtainRT;

    [SerializeField] Data_Module curDataModule;
    /// <summary>
    /// JSON数据文件里所有的数据集
    /// </summary>
   //  [SerializeField]
    private Data_ListModule curListDataModule;
    [SerializeField] EnumVisionMode curVisionMode;
    #endregion
    private void Start()
    {
        // Init();
    }

    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {

    }
    public override void Init()
    {
        base.Init();

        //从XML中获取当前视觉的模式
        int tempVisionMode =int.Parse(Global_XMLCtr.M_Instance.GetElementValue("curVisionMode"));
        curVisionMode = (EnumVisionMode)tempVisionMode;
        //从JSON文件中读取当前眼镜模组数据
        string tempJsonDataURL = Global_Manage.M_CurProjectAssetPath + @"\ResourcesData\JSON\" + Global_XMLCtr.M_Instance.GetElementValue("ModuleDataName");
        curListDataModule = Global_Manage.ReadData_JSON<Data_ListModule>(tempJsonDataURL);
        string curModuleName = Global_XMLCtr.M_Instance.GetElementValue("curModuleName");
        int tempIndex = curListDataModule.ListDataModule.FindIndex(p => p.ModuleName == curModuleName);
        if (-1 == tempIndex)
        {
            Debug.Log("当前JSON文件中的没有找到XML对应的curModuleName数据" + curModuleName);
        }
        else
        {
            curDataModule = curListDataModule.ListDataModule[tempIndex];
        }

        M_SubObjCamObtainRT.Init();
        M_SubObjCamRenderDisplay.Init();
    }
}
