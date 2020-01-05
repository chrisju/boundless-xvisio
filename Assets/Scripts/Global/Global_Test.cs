using Global_StructClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        try
        {
            tempStrMsg = Global_XMLCtr.M_Instance.GetElementValue("ModuleDataName");
            string tempJsonDataURL = Global_Manage.M_CurProjectAssetPath + @"\ResourcesData\JSON\" + Global_XMLCtr.M_Instance.GetElementValue("ModuleDataName");
            Data_ListModule curListDataModule = Global_Manage.ReadData_JSON<Data_ListModule>(tempJsonDataURL);
            tempStrMsg += "json:" + curListDataModule.ListDataModule[0].ModuleName;
        }
        catch(Exception e)
        {
            tempStrMsg = e.Message;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private string tempStrMsg = "测试sdsdsdsddddddddddddddddddddddddddddddddddddddddddddddddd";
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 500, 500), tempStrMsg))
        {
           
        }
    }
}
