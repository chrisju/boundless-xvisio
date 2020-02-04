using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using System;
using UnityEngine;

public class XvSlamBahaviour : MonoBehaviour {

  public float posScale = 1.0f;
  Vector3 originalPos, originalAngles;
  public KeyCode userResetKey = KeyCode.A;

	// Use this for initialization
	void Start () {
    originalAngles = new Vector3();
    originalAngles = this.transform.localEulerAngles;
    originalPos = new Vector3();
    originalPos = this.transform.localPosition;
	}
	
	// Update is called once per frame
  void LateUpdate () {
    this.transform.localPosition = XvSlam.position() * posScale + originalPos;
    this.transform.localRotation = XvSlam.rotation();
    this.transform.Rotate(this.originalAngles);
  }
}
