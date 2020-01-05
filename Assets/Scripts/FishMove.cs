using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMove : MonoBehaviour
{

    public float moveSpeed;
    public float moveRange;
    public Transform OriginPoint;

    private float transX;
    private float pitchPeriod;
    //private float turningPeriod;
    private float movingPeriod;
    private float steadyPeriod;
    private float accTime;
    //[SerializeField]
    private float turningAngle;
    private int isCenterRight;
    private Vector3 turningCenter;
    private float pitchAngleEnd;
    private float turningAngleEnd;
    private bool isTurning;
    private bool isPitching;

    // Use this for initialization
    void Start()
    {
        this.transform.position = OriginPoint.position + new Vector3(Random.Range(-80, 80), Random.Range(-100, 50), Random.Range(-80, 80)) * transform.parent.localScale.x;
        this.transform.eulerAngles = new Vector3(0, Random.Range(-180, 180), 0);
        isTurning = false;
        isPitching = false;
        movingPeriod = Random.Range(0, 1.0f);
        steadyPeriod = Random.Range(0, 1.0f);
        //SetTurn(Random.Range(-8.0f, 8.0f));
        //SetPitch(Random.Range(-1.0f, 1.0f));
        StartCoroutine(FishTurn(10.0f, Random.Range(0.0f, 5.0f)));

    }

    // Update is called once per frame
    void Update()
    {
        //判断是否在限时活动区域内
        if (new Vector2(this.transform.position.x - OriginPoint.position.x, this.transform.position.z - OriginPoint.position.z).magnitude <= moveRange * transform.parent.localScale.x)
        {
            //StopAllCoroutines();
            //远离则慢，靠近则快
            MoveForward(moveSpeed * (1 - (this.transform.position - OriginPoint.position).magnitude / moveRange / 5 - Vector3.Angle(this.transform.forward, -(this.transform.position - OriginPoint.position)) / 180 / 10));
            //前进
            if (isTurning == false)
            {
                //前进完成，设置下一次前进时间，开始转弯
                if ((movingPeriod -= Time.deltaTime) <= 0)
                {
                    movingPeriod = Random.Range(0, 1.0f);
                    //isTurning = true;
                }
            }
            //转弯
            else
            {
                //StartCoroutine(FishTurn(1.0f, Random.Range(0.0f, 3.0f)));
            }
        }
        //如果不在限制活动区域，则向着原点方向转弯
        else
        {
            if (!isTurning)
            {
                StartCoroutine(TurnAngle(Vector3.Angle(this.transform.forward, new Vector3(0, 0, 0) - this.transform.position)));
            }
        }
    }

    void MoveForward(float moveSpeed)
    {
        this.transform.position += this.transform.forward * moveSpeed * transform.parent.localScale.x * Time.deltaTime;
    }

    /// <summary>
    /// 以特定角速度，特定时长进行转弯
    /// </summary>
    /// <param name="angleSpeed">角速度</param>
    /// <param name="turningPeriod">旋转时长</param>
    /// <returns></returns>
    IEnumerator FishTurn(float angleSpeed, float turningPeriod)
    {
        //转弯时间
        float turningTime = 0;
        float presentAngleSpeed = 0;
        isTurning = true;

        while (turningTime < turningPeriod)
        {
            if (Mathf.Abs(presentAngleSpeed) < Mathf.Abs(angleSpeed))
            {
                //0.5s 内完成加速
                presentAngleSpeed = Mathf.Lerp(0, angleSpeed, turningTime / 0.5f);
            }
            this.transform.Rotate(this.transform.up, presentAngleSpeed * Time.deltaTime);
            yield return null;
            turningTime += Time.deltaTime;
        }
        angleSpeed = presentAngleSpeed;
        turningTime = 0;
        while (presentAngleSpeed > 0)
        {
            //0.5s 内完成减速
            presentAngleSpeed = Mathf.Lerp(angleSpeed, 0, turningTime / 0.5f);
            this.transform.Rotate(this.transform.up, presentAngleSpeed * Time.deltaTime);
            yield return null;
            turningTime += Time.deltaTime;
        }
        isTurning = false;
    }
    /// <summary>
    /// 旋转一定角度
    /// </summary>
    /// <returns></returns>
    IEnumerator TurnAngle(float turningAngle, float angularSpeed = 360)
    {
        //转弯时间
        float turningTime = 0;
        float presentTurningAngle = 0;
        float presentAngularSpeed = 0;
        //最大角速度为360度每秒
        angularSpeed = angularSpeed * Random.Range(0.8f, 1f) * Random.Range((int)-1f, (int)2f);
        float moveDistance = moveSpeed * 0.5f;
        float presentMoveDistance = 0;
        //angularSpeed = 360 * Random.Range((int)-1f, (int)2f);
        isTurning = true;

        while (angularSpeed == 0)
        {
            angularSpeed = 360 * Random.Range((int)-1f, (int)2f);
        }
        while (turningAngle - Mathf.Abs(presentTurningAngle) > 2.0f)
        {
            if (Mathf.Abs(presentAngularSpeed) < Mathf.Abs(angularSpeed))
            {
                presentAngularSpeed = Mathf.Lerp(0, angularSpeed, turningTime / 0.1f);
            }
            this.transform.Rotate(this.transform.up, presentAngularSpeed * Time.deltaTime);
            presentTurningAngle += presentAngularSpeed * Time.deltaTime;
            yield return null;
            turningTime += Time.deltaTime;
        }
        angularSpeed = presentAngularSpeed;
        turningTime = 0;
        while (Mathf.Abs(presentAngularSpeed) > 0.01f)
        {
            presentAngularSpeed = Mathf.Lerp(angularSpeed, 0, turningTime / 0.1f);
            this.transform.Rotate(this.transform.up, presentAngularSpeed * Time.deltaTime);
            MoveForward(moveSpeed * 0.5f);
            yield return null;
            turningTime += Time.deltaTime;
        }
        while (presentMoveDistance < moveDistance)
        {
            MoveForward(moveSpeed * 0.5f);
            presentMoveDistance += moveSpeed;
            yield return null;
        }
        isTurning = false;
    }

}
