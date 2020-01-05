using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System;

public class Global_XMLCtr
{

    private static Global_XMLCtr _Instance;


    /// <summary>
    /// 安卓的SD卡绝对路径，另需要设置发布时候SD卡壳读写
    /// </summary>
    private static string xmlpath = String.Empty;
    public static Global_XMLCtr M_Instance
    {
        get
        {
            if (_Instance == null)
            {
#if UNITY_ANDROID 
                xmlpath= "file:///storage/emulated/0" + "/ResourcesData/XML/GlobalSetXML";

#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                xmlpath = Global_Manage.M_CurProjectAssetPath + "ResourcesData/XML/GlobalSetXML";
#endif
                _Instance = new Global_XMLCtr();

            }
            return _Instance;
        }
    }
    public void CreateXMLDocument()
    {
        XElement root = new XElement("XMLContent",
           new XElement("Root", "root")
      );
        root.Save(xmlpath);
    }
    public bool CheckXMLISNull()
    {
        XElement root = null;
        try
        {
            root = XElement.Load(xmlpath);
        }
        catch (Exception ex)
        {
            if (ex != null)
                return true;
        }
        return false;
    }
    public XElement LoadXMLFromFile()
    {
        XElement root = XElement.Load(xmlpath);
        return root;
    }
    public void SetElementValue(string name, string value)
    {
        XElement root = LoadXMLFromFile();
        root.Element(name).SetValue(value);
        root.Save(xmlpath);
    }
    /// <summary>
    /// 在根节点元素之前添加新的元素
    /// </summary>
    /// <param name="name">元素名字</param>
    /// <param name="value">元素的值</param>
    public void AddElement(string name, string value)
    {
        XElement root = LoadXMLFromFile();
        XElement newElement = new XElement(name, value);
        root.Add(newElement);
        root.Save(xmlpath);
    }
    /// <summary>
    /// 删除指定的元素
    /// </summary>
    /// <param name="name">要删除的元素名称</param>
    public void RemoveElement(string name)
    {
        XElement root = LoadXMLFromFile();
        root.Element(name).Remove();
        root.Save(xmlpath);
    }
    /// <summary>
    /// 根据元素名查找元素对应的值
    /// </summary>
    /// <param name="name">元素名</param>
    /// <returns></returns>
    public string GetElementValue(string name)
    {
        XElement root = LoadXMLFromFile();
        //     XAttribute xattr = root.Element(name.Trim()).Attribute("MyVaule");
        XElement curElement = root.Element(name.Trim());
        string s = curElement.Value;
        return s;
    }
    /// <summary>
    /// 检测这个元素是否存在，不存在为True
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckElementIsNull(string name)
    {
        XElement root = LoadXMLFromFile();
        XElement xElement = root.Element(name);//XMl的元素名不能以数字开头

        return xElement == null ? true : false;

    }
}
