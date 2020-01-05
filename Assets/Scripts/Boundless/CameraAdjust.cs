using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gloabal_EnumCalss;
using Global_StructClass;

public class CameraAdjust : BaseGameObj
{
    public static CameraAdjust M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<CameraAdjust>();
            }
            return _instance;
        }
    }
    public Transform m_MeshLeft;
    public Transform m_MeshRight;
    public Transform m_EyeLeft;
    public Transform m_EyeRight;
    private Data_Calibration curCalirationData;
    private static CameraAdjust _instance;
    private int index = 0;
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (0 == index)
            {
                m_MeshLeft.localPosition += new Vector3(0, 0.1f, 0);
                index = 1;
            }
            else
            {
                m_MeshRight.localPosition += new Vector3(0, -0.1f, 0);
                index = 0;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (1 == index)
            {
                m_MeshLeft.localPosition -= new Vector3(0, 0.1f, 0);
                index = 0;
            }
            else
            {
                m_MeshRight.localPosition -= new Vector3(0, -0.1f, 0);
                index = 1;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_EyeLeft.localPosition -= new Vector3(0.00005f, 0, 0);
            m_EyeRight.localPosition += new Vector3(0.00005f, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (m_EyeLeft.localPosition.x <= 0.02f)
            {
                m_EyeLeft.localPosition += new Vector3(0.00005f, 0, 0);
                m_EyeRight.localPosition -= new Vector3(0.00005f, 0, 0);
            }
        }

    }
    public override void Init()
    {
        base.Init();
        string tempJsonDataURL = Global_Manage.M_CurProjectAssetPath + @"\ResourcesData\JSON\CalibrationData.json";
        curCalirationData = Global_Manage.ReadData_JSON<Data_Calibration>(tempJsonDataURL);
        m_MeshLeft.localPosition += new Vector3(0, curCalirationData.OffsetY, 0);
        m_MeshRight.localPosition += new Vector3(0, -curCalirationData.OffsetY, 0);
        m_EyeLeft.localPosition += new Vector3(-curCalirationData.OffsetX, 0, 0);
        m_EyeRight.localPosition += new Vector3(-curCalirationData.OffsetX, 0, 0);
        if (curCalirationData.IsLeftRightSwitch)
        {
            m_MeshLeft.localPosition += new Vector3(-m_MeshLeft.localPosition.x * 2, 0, 0);
            m_MeshRight.localPosition += new Vector3(-m_MeshRight.localPosition.x * 2, 0, 0);
        }
        if (curCalirationData.IsLeftUpsideDown)
        {
            m_MeshLeft.localScale = Vector3.Scale(m_MeshLeft.localScale,new Vector3(1, -1, 1));
        }
        if (curCalirationData.IsRightUpsideDown)
        {
            m_MeshRight.localScale = Vector3.Scale(m_MeshLeft.localScale,new Vector3(1, -1, 1));
        }
    }
    private void OnApplicationQuit()
    {
        curCalirationData.OffsetY = m_MeshLeft.localPosition.y;
        curCalirationData.OffsetX = m_EyeRight.localPosition.x - Camera_DistortionManage.M_Instance.M_CurModule.InterPupilDistance / 2;
        string dataPath = Global_Manage.M_CurProjectAssetPath + @"\ResourcesData\JSON\CalibrationData.json";
        // Debug.Log(curCalirationData.OffsetY.ToString() + "," + curCalirationData.OffsetX.ToString());
        Global_Manage.SaveData_JSON(curCalirationData, dataPath);
    }
}