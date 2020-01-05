using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(TextMesh))]
public class FPS : MonoBehaviour
{
    private TextMesh textField;
    private float lastUpdateShowTime = 0f;    //上一次更新帧率的时间;
    private float updateShowDeltaTime = 0.5f;//更新帧率的时间间隔;
    private int frameUpdate = 0;//帧数;
    private float curFPS = 0;

    void Awake()
    {
        textField = GetComponent<TextMesh>();
        Application.targetFrameRate = 100;
        lastUpdateShowTime = Time.realtimeSinceStartup;
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        frameUpdate++;
        float tempCurRealTime = Time.realtimeSinceStartup;
        if (tempCurRealTime - lastUpdateShowTime >= updateShowDeltaTime)
        {
            curFPS = frameUpdate / (tempCurRealTime - lastUpdateShowTime);
            frameUpdate = 0;
            lastUpdateShowTime = tempCurRealTime;
        }
        textField.text = "CurFPS:" + curFPS;
    }
    private void FixedUpdate()
    {

    }
    private void OnGUI()
    {

    }

}
