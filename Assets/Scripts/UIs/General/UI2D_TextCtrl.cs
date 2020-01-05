using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
/// <summary>
/// 读取Text数据的类
/// </summary>
public class UI2D_TextCtrl
{
    #region 私有变量
    /// <summary>
    /// 从txt中读取的文字数据
    /// </summary>
    private Dictionary<int, string> DicTextEditor;

    /// <summary>
    /// 单例
    /// </summary>
    private static UI2D_TextCtrl _instance = new UI2D_TextCtrl();
    #endregion

    #region 属性
    /// <summary>
    /// 单例属性
    /// </summary>
    public static UI2D_TextCtrl M_Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UI2D_TextCtrl();
            return _instance;
        }
    }

    /// <summary>
    /// txt信息的索引字典
    /// </summary>
    public Dictionary<int,string> M_DicText
    {
        get
        {
            if (DicTextEditor == null)
                DicTextEditor = new Dictionary<int, string>();
            return DicTextEditor;
        }
    }
    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
    public UI2D_TextCtrl()
    {
        if (DicTextEditor != null)
        {
            DicTextEditor.Clear();
        }
        else
        {
            DicTextEditor = new Dictionary<int, string>();
        }
        //获取txt中的文字信息
        string TipText;
        StreamReader sr = new StreamReader(Global_Manage.M_CurProjectAssetPath + @"\ResourcesData\XML\TextEditor.txt", Encoding.Default);
        DicTextEditor = new Dictionary<int, string>();
        while ((TipText = sr.ReadLine()) != null)
        {
            if (TipText.Contains(":"))
            {
                int keyPos = TipText.IndexOf(":");
                string TextKey = TipText.Substring(0, keyPos);//不允许用Split，不然信息中的其余冒号也会被抹杀
                string TextInfo = TipText.Substring(keyPos + 1);
                DicTextEditor.Add(int.Parse(TextKey), TextInfo);
            }
        }
        sr.Close();
    }
   
}
