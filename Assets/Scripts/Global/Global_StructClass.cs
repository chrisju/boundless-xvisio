using Gloabal_EnumCalss;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global_StructClass
{

    [Serializable]
    /// <summary>
    /// 眼镜模组数据
    /// </summary>
    public struct Data_Module
    {
        /// <summary>
        /// 当前模组的类型
        /// </summary>
        public EnumModuleType ModuleType;
        /// <summary>
        /// 当前模组名称
        /// </summary>
        public string ModuleName;
        /// <summary>
        /// 畸变贴图的名字
        /// </summary>
        public string DistortTexName;
        /// <summary>
        /// 瞳距
        /// </summary>
        public float InterPupilDistance;
        /// <summary>
        /// 视场角
        /// </summary>
        public float FieldOfView;
        /// <summary>
        /// 眼镜分辨率
        /// </summary>
        public int ResolutionX;
        public int ResolutionY;
        /// <summary>
        /// 畸变处理的网格的顶点数，x、y分别为对应方向上的顶点数
        /// </summary>
        public int NumDMPX;
        public int NumDMPY;
        public float MinX;
        public float MinY;
        public float DeltaX;
        public float DeltaY;
        public void Init(EnumModuleType mtype, string mName)
        {
            ModuleType = mtype;
            ModuleName = mName;
        }
    }
    [Serializable]
    /// <summary>
    /// 模组数据列表
    /// </summary>
    public struct Data_ListModule
    {
        public int Number;

        public List<Data_Module> ListDataModule;
        public void Init(int num)
        {
            Number = num;
            ListDataModule = new List<Data_Module>();
        }
    }

    /// <summary>
    /// Xvisio的配置
    /// </summary>
    [Serializable]
    public struct Data_XvisioConfig
    {
        /// <summary>
        /// 设备移动的距离与视觉效果距离的比例
        /// </summary>
        public float PosScale;
        /// <summary>
        /// // after how many frames of wrong state will the slam reset itself
        /// </summary>
        public int AllowedMaxWrongStateFrameNumber;
        /// <summary>
        ///  // time to wait affter reseting the slam, in seconds
        /// </summary>
        public float SlamResetCooldownTime;
        /// <summary>
        /// // Flip in firmware enabled,设备翻转
        /// </summary>
        public bool FirmwareFlip;
        /// <summary>
        /// // decide if module laid up side down;
        /// </summary>
        public bool CameraUpSideDown;
        /// <summary>
        /// Xslam only provide position only, and no rotation
        /// </summary>
        public bool PositionOnly;
        /// <summary>
        ///  // Enable post filter
        /// </summary>
        public bool PostFilterEnabled;
        /// <summary>
        /// // Enable post filter
        /// </summary>
        public bool AutoPostFilter;
        /// <summary>
        /// // Post filter coefficients for rotation
        /// </summary>
        public float FilterCoefRotation;
        /// <summary>
        /// // Post filter coefficients for translation
        /// </summary>
        public float FilterCoefTranslation;
        /// <summary>
        /// Enable  IMU fusion
        /// </summary>
        public bool ImuFusionEnable;
        /// <summary>
        /// // Offset adjustment of exposure delay
        /// </summary>
        public float OffsetAdjust;
        /// <summary>
        ///  // Prediction
        /// </summary>
        public float Prediction;

    }

    public struct Data_Calibration
    {
        public float OffsetY;
        public float OffsetX;
        public bool IsLeftRightSwitch;
        public bool IsLeftUpsideDown;
        public bool IsRightUpsideDown;
    }
}
