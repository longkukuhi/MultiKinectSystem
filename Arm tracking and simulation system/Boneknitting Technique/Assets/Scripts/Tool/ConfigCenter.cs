using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public class DataPath
{
    public static string strDataRootPath= Application.dataPath + "/StreamingAssets";
    public static string strConfigFilePath = strDataRootPath + "/Config.xml";
    public static string strDefaultSaveFolderPath = strDataRootPath + "/Movies";
    public static string strDefaultPortraitFolder = strDataRootPath + "/Portraits";
}

public class XmlString
{
    public const string strSaveDirectoryParentNode = "SaveFloder";
    public const string strHistoryParentNode = "HistoryFile";

    public const   string strSaveDirectoryNodeName = "CurrentFolder";
    public const  string strHistoryFilePathNodeName = "History";
}

public class ConfigCenter
{
    private static ConfigCenter ConfigCenterInstance;
    private ConfigCenter()
    {
        strSaveDirectory = null;
        HistoryFilePathList = new List<string>();
        m_fFPS = 30;
    }
    public static ConfigCenter Instance()
    {
        if (ConfigCenterInstance == null)
        {
            ConfigCenterInstance = new ConfigCenter();
        }
        return ConfigCenterInstance;
    }
    XmlDocument xml;

    private string strSaveDirectory;
    private List<string> HistoryFilePathList;
    private int m_fFPS;

    public string GetDefaultDirPath()
    {
        return strSaveDirectory;
    }
    public List<string> GetHistoryFilePathList()
    {
        return HistoryFilePathList;
    }

    public int GetFPS()
    {
        return m_fFPS;
    }

    /// <summary>
    /// 读取Config.xml文件，初始化ConfigCenter,路径；
    /// </summary>
    /// <param name="path">配置文件相对于Application.datapath+"StreamAsserts"路径</param>
    /// <returns></returns>
    public bool ConfigDataInit(string path)
    {
        xml = new XmlDocument();
        string content = System.IO.File.ReadAllText(path);
        xml.LoadXml(content);

        XmlElement players = xml.DocumentElement;//获取根元素  
        foreach (XmlNode player in players.ChildNodes)//遍历所有子节点  
        {
            foreach (XmlNode node in player.ChildNodes)
            {
                XmlElement xe = (XmlElement)node;
                switch (xe.Name)
                {
                    case XmlString.strSaveDirectoryNodeName:
                        strSaveDirectory = xe.InnerText;
                        break;
                    case XmlString.strHistoryFilePathNodeName:
                        HistoryFilePathList.Add(xe.InnerText);
                        break;
                }
            }
        }

        return true;
    }

    private void SetSaveFilePath(string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            return;
        }
        var SaveFileDirectoryNode = xml.GetElementsByTagName(XmlString.strSaveDirectoryNodeName);
        SaveFileDirectoryNode[0].InnerText = path;
    }

    public void DeleteFileByPath(string path)
    {
        var HistoryNode = xml;
        foreach( XmlNode node in HistoryNode)
        {
            if (node.InnerText.Equals(path))
            {
                xml.RemoveChild(node);
            }
        }
    }

    public void AddHistoryFile(string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            return;
        }
        var HistoryFile = xml.GetElementsByTagName(XmlString.strHistoryParentNode);
        XmlElement history = xml.CreateElement("History");
        history.InnerText = path;
        HistoryFile[0].AppendChild(history);
        xml.Save(DataPath.strConfigFilePath);
    }


}
