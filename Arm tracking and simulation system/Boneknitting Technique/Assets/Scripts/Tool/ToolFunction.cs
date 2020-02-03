using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class FIleDialogStruct
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;

    //打开文件选择对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] FIleDialogStruct ofn);


    // 打开文件保存对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] FIleDialogStruct ofd);
}

public class ToolFunction
{
    /// <summary>
    /// 将毫秒时间转换成 mm:ss
    /// </summary>
    public static string TranslateToMMSS(float fTimeInMiloSecond)
    {
        string ret;
        int nMinute = (int)(fTimeInMiloSecond / 60000) % 60;
        int nSecond = (int)(fTimeInMiloSecond / 1000) % 60;
        //int nMilloSecond = (int)fTimeInMiloSecond % 1000;
        ret = string.Format("{0:D2} : {1:D2}", nMinute, nSecond);
        return ret;
    }

    /// <summary>
    /// 弹出文件选择对话框，返回文件路径
    /// </summary>
    /// <param name="filter">文件过滤字符串，使用逗号分隔</param>
    /// <param name="title">窗口标题</param>
    /// <param name="extension">默认添加后缀</param>
    /// <returns>文件路径</returns>
    public static string OpenFilePath(string filter, string title, string extension)
    {
        FIleDialogStruct pth = new FIleDialogStruct();
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        pth.filter = filter;
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath;  // default path  
        pth.title = title;
        pth.defExt = extension;
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (FIleDialogStruct.GetOpenFileName(pth))
        {
            return pth.file;//选择的文件路径;  
        }
        else
        {
            return null;
        }
    }

    public static string SaveFilePath(string filter, string title, string extension)
    {
        FIleDialogStruct pth = new FIleDialogStruct();
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        pth.filter = filter;
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath;  // default path  
        pth.title = title;
        pth.defExt = extension;
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (FIleDialogStruct.GetSaveFileName(pth))
        {
            return pth.file;//选择的文件路径;  
        }
        else
        {
            return null;
        }
    }

    public static Sprite CreateSpriteFromImage(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        WWW www = new WWW("file://" + path);
        Texture2D texture = www.texture;

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    /// <summary>
    /// 生成字符串ID
    /// </summary>
    /// <returns></returns>
    public static string GenerateStringID()
    {
        long i = 1;
        foreach (byte b in Guid.NewGuid().ToByteArray())
        {
            i *= ((int)b + 1);
        }
        return string.Format("{0:x}", i - DateTime.Now.Ticks);
    }

    /// <summary>
    /// 图片保存
    /// </summary>
    /// <param name="tex">Tex.</param>
    public static void ImageSaveLocal(Texture tex,string path)
    {
        Texture2D saveImageTex = tex as Texture2D;
        FileStream newFs = new FileStream(path, FileMode.Create, FileAccess.Write);
        byte[] bytes = saveImageTex.EncodeToJPG();
        newFs.Write(bytes, 0, bytes.Length);
        newFs.Close();
        newFs.Dispose();
    }

    /// <summary>
    /// 获取默认存储文件的全路径
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="extension">后缀</param>
    /// <returns></returns>
    public static string GetMovieSaveFilePath(string fileName,string extension)
    {
        if(string.IsNullOrEmpty(fileName))
        {
            return null;
        }
       if(IsExtension(fileName,extension))
        {
            return DataPath.strDefaultSaveFolderPath +"/"+ fileName;
        }
        else
        {
            return DataPath.strDefaultSaveFolderPath+"/" + fileName + extension;
        }
    }

    /// <summary>
    /// 判断文件名是否带有相应后缀
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="extension">后缀</param>
    /// <returns></returns>
    public static bool IsExtension(string fileName,string extension)
    {
        for (int ExtensionIndex=extension.Length-1,FileNameIndex=fileName.Length-1;ExtensionIndex>0 && FileNameIndex>0;--ExtensionIndex,--FileNameIndex)
        {
            if(! extension[ExtensionIndex].Equals(fileName[FileNameIndex]))
            {
                return false; 
            }
        }
        return true;
    }

    public static string GetDefaultPortraitPathByName(string PortraitName,string extension)
    {
        return string.Format("{0}/{1}{2}", DataPath.strDefaultPortraitFolder, PortraitName, extension);
    }
}
