using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconBehaviour : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(Camera.main.transform);
        this.transform.Rotate(Vector3.up, 180);
    }
}
