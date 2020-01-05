using Gloabal_EnumCalss;
using Global_StructClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 渲染显示到最终显示器的摄像机
/// </summary>
public class Camera_RenderDisplay : BaseGameObj
{
    private Camera curCam;
    [SerializeField] MeshFilter subMeshRTLeft;
    [SerializeField] MeshFilter subMeshRTRight;
    [SerializeField] List<Texture2D> listDistortionTexes = new List<Texture2D>();

    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {

    }

    /// <summary>
    /// 根据顶点创建网格,并进行畸变处理
    /// </summary>
    /// <param name="numX"></param>
    /// <param name="numY"></param>
    /// <returns></returns>
     private Mesh Create_DistortionMesh(Data_Module dataModule)
    {
        int SizeX = dataModule.NumDMPX;
        int SizeY = dataModule.NumDMPY;
        Mesh tempMesh = new Mesh();

        Vector3[] vertices = new Vector3[(SizeX + 1) * (SizeY + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= SizeY; y++)
        {
            for (int x = 0; x <= SizeX; x++, i++)
            {
                vertices[i] = new Vector3(x - SizeX / 2, y - SizeY / 2);
                uv[i] = new Vector2((float)x / SizeX, (float)y / SizeY);
                tangents[i] = tangent;
            }
        }
        int[] triangles = new int[SizeX * SizeX * 6];
        for (int ti = 0, vi = 0, y = 0; y < SizeY; y++, vi++)
        {
            for (int x = 0; x < SizeX; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + SizeX + 1;
                triangles[ti + 5] = vi + SizeX + 2;
            }
        }
        #region 计算畸变
        
        //获取Matlab计算好的贴图
        Texture2D tempDistTex = null;
        int tempIndex = listDistortionTexes.FindIndex(p => p.name == dataModule.DistortTexName.ToString());
        if(-1==tempIndex)
        {
            Debug.LogError("读取反畸变图片失败！");
            return null;
        }
        tempDistTex = listDistortionTexes[tempIndex];

        //获取贴图RGB的值
        Color[] tempColors = tempDistTex.GetPixels();
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i][0] += ((float)((int)(tempColors[i][0] * 255 * 16) + (int)(tempColors[i][2] * 255 / 16)) / 4096 * dataModule.DeltaX + dataModule.MinX) * SizeX;
            vertices[i][1] += ((float)((int)(tempColors[i][1] * 255 * 16) + (int)(tempColors[i][2] * 255) % 16) / 4096 * dataModule.DeltaY + dataModule.MinY) * SizeY;
        }          
        
        #endregion
        tempMesh.vertices = vertices;
        tempMesh.uv = uv;
        tempMesh.tangents = tangents;
        tempMesh.triangles = triangles;
        tempMesh.RecalculateNormals();

        return tempMesh;
    }

    public override void Init()
    {
        base.Init();

        curCam = GetComponent<Camera>();
        Data_Module tempCurData = Camera_DistortionManage.M_Instance.M_CurModule;

        ///设置显示面板的网格,重新计算畸变网格的顶点数
        subMeshRTLeft.mesh = Create_DistortionMesh(tempCurData);

        Vector3 tempVecScale = Vector3.one * Camera_DistortionManage.SIZERATE_DISTORTIONMESH;
        subMeshRTLeft.transform.localScale = tempVecScale;
        subMeshRTLeft.GetComponent<MeshRenderer>().material.mainTexture = Camera_ObtainRT.ScreenLeft;
        //一定要先转换成浮点数，否则精度会丢失
        curCam.aspect = (float)tempCurData.ResolutionX / tempCurData.ResolutionY;

        switch (Camera_DistortionManage.M_Instance.M_CurVisionMode)
        {
            case EnumVisionMode.MONOCULAR:
                subMeshRTLeft.transform.localPosition = new Vector3(0, 0, curCam.farClipPlane / 2.0f);
                Destroy(subMeshRTRight.gameObject);
                break;
            case EnumVisionMode.BINOCULAR:
                subMeshRTRight.gameObject.SetActive(true);
                curCam.aspect *= 2.0f;
                //将左右两个渲染面板隔开分辨率的一半距离,保证两个面板严格的出现在相机视觉矩阵里
                subMeshRTRight.mesh = Create_DistortionMesh(tempCurData);
                subMeshRTRight.transform.localScale = tempVecScale;
                subMeshRTRight.GetComponent<MeshRenderer>().material.mainTexture = Camera_ObtainRT.ScreenRight;
                subMeshRTLeft.transform.localPosition = new Vector3(-tempCurData.ResolutionX / 2.0f, 0, curCam.farClipPlane / 2.0f);
                subMeshRTRight.transform.localPosition = new Vector3(tempCurData.ResolutionX / 2.0f, 0, curCam.farClipPlane / 2.0f);
                break;
        }
        curCam.orthographicSize = tempCurData.ResolutionY / Camera_DistortionManage.SIZERATE_CAMORTH;

    }
}
