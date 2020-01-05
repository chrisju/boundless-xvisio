using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;//调用外部库，需要引用命名空间
using System;
using System.Threading;

public class Global_Windows  {

    public static Global_Windows M_Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Global_Windows();
            }
            return _instance;
        }
    }
    private openFileName ofn = new openFileName();
    private static Global_Windows _instance;

    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, String message, String title, int type);//具体方法
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] openFileName ofn);
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([Out, In] openFileName ofn);

    /// <summary>
    /// 打开某个格式的文件，并返回该文件的地址
    /// </summary>
    /// <param name="dataType">该文件的格式</param>
    /// <param name="isSaveFile">true为保存文件，false为只是打开文件，默认为false</param>
    /// <returns></returns>
    public string Open_WindowFile(string dataType,bool isSaveFile=false)
    {
        string tempURL = string.Empty;
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = dataType+"\0*."+ dataType + "*\0\0";
       // ofn.filter = "json\0*.json*\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTittle = new string(new char[64]);
        ofn.maxFileTittle = ofn.fileTittle.Length;
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        ofn.initialDir = path;
        ofn.tittle = "Open Project";
        ofn.defExt = dataType;
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (isSaveFile&& GetSaveFileName(ofn))
        {
            tempURL = ofn.file;
        }
        else if(!isSaveFile&&GetOpenFileName(ofn))
        {
            tempURL = ofn.file;
        }
        return tempURL;
       
    }
}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class openFileName
{

    public int structSize = 0;
    public IntPtr digOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTittle = null;
    public int maxFileTittle = 0;
    public String initialDir = null;
    public String tittle = null;
    public int flags = 0;
    public short fileoffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templataName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int resveredInt = 0;
    public int flagsEx = 0;
}
