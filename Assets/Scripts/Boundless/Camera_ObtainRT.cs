using Global_StructClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获取unity物体的摄像机，该摄像机将图像信息生成RenderTexure
/// </summary>
public class Camera_ObtainRT : BaseGameObj
{
    public static RenderTexture ScreenLeft { get; private set; }
    public static RenderTexture ScreenRight { get; private set; }
    public Camera M_CurCamLeft
    {
        get
        {
            return curCamLeft;
        }
    }
    public Camera M_CurCamRight
    {
        get
        {
            return curCamRight;
        }
    }
    [SerializeField]
    private Camera curCamLeft;
    [SerializeField]
    private Camera curCamRight;
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

        if (this.GetComponent<Camera>())
        {
            Destroy(this.GetComponent<Camera>());
        }

        //设置摄像机的各项参数
        Data_Module tempdataM = Camera_DistortionManage.M_Instance.M_CurModule;

        float tempAspect = (float)tempdataM.ResolutionX / tempdataM.ResolutionY;
        curCamLeft.fieldOfView = tempdataM.FieldOfView;
        curCamLeft.aspect = tempAspect;
        curCamLeft.nearClipPlane = 0.1f;
        curCamLeft.transform.localEulerAngles = new Vector3(0, 0, 0);

        ScreenLeft = new RenderTexture(tempdataM.ResolutionX, tempdataM.ResolutionY, 24, RenderTextureFormat.ARGB32);
        curCamLeft.targetTexture = ScreenLeft;


        switch (Camera_DistortionManage.M_Instance.M_CurVisionMode)
        {
            case Gloabal_EnumCalss.EnumVisionMode.MONOCULAR:
                Destroy(curCamRight.gameObject);
                curCamLeft.transform.localPosition = new Vector3(0, 0, 0);
                break;
            case Gloabal_EnumCalss.EnumVisionMode.BINOCULAR:
                //设置摄像机直接的距离，以保证跟瞳距一样
                curCamRight.fieldOfView = tempdataM.FieldOfView;
                curCamRight.aspect = tempAspect;
                curCamRight.nearClipPlane = 0.1f;
                curCamRight.transform.localEulerAngles = new Vector3(0, 0, 0);
                ScreenRight = new RenderTexture(tempdataM.ResolutionX, tempdataM.ResolutionY, 24, RenderTextureFormat.ARGB32);
                curCamRight.targetTexture = ScreenRight;

                float tempDis = tempdataM.InterPupilDistance / 2.0f;
                curCamLeft.transform.localPosition = new Vector3(-tempDis, 0, 0);
                curCamRight.transform.localPosition = new Vector3(tempDis, 0, 0);
                break;
        }

    }
}
