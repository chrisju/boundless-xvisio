using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using System;
using UnityEngine;

using Xvisio;
using Global_StructClass;

public class XvSlam : MonoBehaviour
{
    private XvHid xvHid;
    public static XvSlam firstInstance;
    Vector3 originalPos, originalAngles, middlePos, middleAngles;
    private static int wrongStateCount = 0;
    public KeyCode userResetKey = KeyCode.A;
    private bool slamInitialized = false;
    private bool lastImuFusionEnable = true;  // saved imuFusionEnable;
    private float lastOffsetAdjust = 0.0f;  // saved offsetAdjust;
    private float lastPrediction = 0.0f;  // saved prediction;
    private Quaternion quatC;
    private Vector3 posC;
    private bool filterStatus = false;

    bool mIsApplicationIsPlaying = false;
    Thread usbReadingThread = null;
    bool mResettingSLAM = false;

    int i = 0, j = 0;

    [SerializeField] Data_XvisioConfig curDataXvisioConfig;
    //    [SerializeField] Vector3 offsetRot;
    void Awake()
    {

    }

    void Start()
    {

        string tempURL = Global_Manage.M_CurResourcesDataURL_JSON + Global_XMLCtr.M_Instance.GetElementValue("XvisioConfigJsonName");
        curDataXvisioConfig = Global_Manage.ReadData_JSON<Data_XvisioConfig>(tempURL);

        if (firstInstance == null)
        {
            firstInstance = this;
        }
        if (this == firstInstance)
        {
            originalAngles = new Vector3();
            middleAngles = new Vector3();
            originalAngles = middleAngles = this.transform.eulerAngles;
            originalPos = new Vector3();
            middlePos = new Vector3();
            originalPos = middlePos = this.transform.position;
            mIsApplicationIsPlaying = Application.isPlaying;
        }
        if (this == firstInstance && !slamInitialized)
        {
            try
            {

                xvHid = new XvHid(curDataXvisioConfig.PostFilterEnabled, curDataXvisioConfig.FilterCoefRotation,
                    curDataXvisioConfig.FilterCoefTranslation, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
                //xvHid = new XvHid(firmwareFlip && cameraUpSideDown);
                slamInitialized = true;
                usbReadingThread = new Thread(new ThreadStart(usbReadingLoop));
                usbReadingThread.Start();
            }
            catch
            {
                Debug.Log("Failed to open the slam.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Update " + i++);
        XVisioUpdate();
    }

    void updateIMUConfig()
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

    void enablePostFlt()
    {
        if (filterStatus != curDataXvisioConfig.PostFilterEnabled)
        {
            Debug.Log("filterStatus: " + filterStatus + "postFilterEnabled: " + curDataXvisioConfig.PostFilterEnabled);
            xvHid.enablePostFilter(curDataXvisioConfig.PostFilterEnabled, curDataXvisioConfig.FilterCoefRotation, curDataXvisioConfig.FilterCoefTranslation);
            filterStatus = curDataXvisioConfig.PostFilterEnabled;
        }
    }

    void enablePostFlt_pause_6dof()
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

    void OnApplicationQuit()
    {
        mIsApplicationIsPlaying = false;
    }

    // USB read funtcion on PC platform (in the plugin XvHid for Android)
    public void usbReadingLoop()
    {
        // While the application is playing, we read usb data
        while ( mIsApplicationIsPlaying )
        {
            if (xvHid != null && xvHid.setup())
            {
                try {
                    xvHid.update();
                }catch(Exception e)
                {
                    UnityEngine.Debug.LogError("read error");
                }
            }
            else
            {
                Thread.Sleep(1);
            }
        }
    }

    IEnumerator restartSLAM()
    {
        if (mResettingSLAM) yield break;
        mResettingSLAM = true;

        middleAngles = this.transform.eulerAngles;
        middlePos = this.transform.position;
        xvHid.request6Dof(false, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
        //xvHid.enablePostFilter(postFilterEnabled, filterCoefRotation, filterCoefTranslation);
        enablePostFlt();
        xvHid.resetSLAM(true);
        Debug.Log("Slam reset");
        yield return new WaitForSecondsRealtime(curDataXvisioConfig.SlamResetCooldownTime);
        xvHid.request6Dof(true, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
        wrongStateCount = 0;

        mResettingSLAM = false;
    }

    public void XVisioUpdate()
    {
        if (xvHid == null || !xvHid.setup()) return;
        
        enablePostFlt_pause_6dof();
        updateIMUConfig();

        int device_state = xvHid.state();

        if ((device_state == 2) || (device_state == 4) || (device_state == 5))
        {
            wrongStateCount = 0;
            doMoving();
        }
        else
        {
            wrongStateCount++;
        }

        if (wrongStateCount > curDataXvisioConfig.AllowedMaxWrongStateFrameNumber)
        {
            // reset the slam
            if (!mResettingSLAM)
            {
                StartCoroutine(restartSLAM());
            }                
        }
        if (Input.GetKey(KeyCode.A))
        {
            xvHid.request6Dof(false, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
            middlePos = originalPos;
            middleAngles = originalAngles;
            //xvHid.enablePostFilter(postFilterEnabled, filterCoefRotation, filterCoefTranslation);
            enablePostFlt();
            xvHid.resetSLAM(true);
            new WaitForSecondsRealtime(curDataXvisioConfig.SlamResetCooldownTime);
            xvHid.request6Dof(true, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
        }
    }

    private void doMoving()
    {

        posC = xvHid.position();
        quatC = xvHid.rotation();
        //	Debug.Log("we got p and q: "+ posC + "   ,  "+quatC + ", " + reversal);
        if (!curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown)
        // if(cameraUpSideDown)
        {
            quatC.x = -quatC.x;
            quatC.y = -quatC.y;
            //		quatC.z = quatC.z;
            //		quatC.w = quatC.w;

            posC.x = -posC.x;
            posC.y = -posC.y;
            //		posC.z = posC.z;
        }

        if (!curDataXvisioConfig.PositionOnly)
        {
            this.transform.rotation = quatC;
            this.transform.Rotate(middleAngles);
            //  transform.eulerAngles = new Vector3(transform.eulerAngles.x+ offsetRot.x, transform.eulerAngles.y+ offsetRot.y, transform.eulerAngles.z + offsetRot.z);
        }

        this.transform.position = posC * curDataXvisioConfig.PosScale + middlePos;

    }


    public static Vector3 position()
    {
        if (firstInstance)
        {
            return firstInstance.transform.position;
        }
        else
        {
            throw new Exception("Please put a XvSlam object into the scene!");
        }
    }

    public static Quaternion rotation()
    {
        if (firstInstance)
        {
            return firstInstance.transform.rotation;
        }
        else
        {
            throw new Exception("Please put a XvSlam object into the scene!");
        }
    }

    public static Vector3 eulerAngles()
    {
        if (firstInstance)
        {
            return firstInstance.transform.eulerAngles;
        }
        else
        {
            throw new Exception("Please put a XvSlam object into the scene!");
        }
    }

    public void reset()
    {
        if (firstInstance.slamInitialized)
        {
            firstInstance.middlePos = originalPos;
            firstInstance.middleAngles = originalAngles;
            firstInstance.xvHid.request6Dof(false, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
            //xvHid.enablePostFilter(postFilterEnabled, filterCoefRotation, filterCoefTranslation);
            enablePostFlt();
            firstInstance.xvHid.resetSLAM(true);
            firstInstance.xvHid.request6Dof(true, curDataXvisioConfig.FirmwareFlip && curDataXvisioConfig.CameraUpSideDown);
        }
    }
}
