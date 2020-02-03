using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileConfig
{
    public const int KINECT_NODE_NUM = 22;
    public const int FIVE_DT_NODE_NUM = 14;
}

public class FileReader
{
    private string m_filepath;
    public MovieHeadData m_head_data;
    public List<ModelCtrlData> m_data_list = new List<ModelCtrlData>();

    //用文件路径初始化对象
    public FileReader(string strFilePath)
    {
        m_filepath = strFilePath;
        m_head_data = GetHeadFromFile(strFilePath);

        if (m_head_data.strIdentify == "MOVIE_DATA")
            readTxtFile(m_filepath, ref m_data_list);
        else
            Debug.Log("FILE TYPE ERROR!");
    }

    /// <summary>
    /// 通过帧编号获取模型控制数据
    /// </summary>
    /// <param name="nFrameCount">帧编号</param>
    /// <returns></returns>
    public ModelCtrlData PraseDataByFrameCount(int nFrameCount)
    {
        if (nFrameCount < 0) nFrameCount = 0;
        if (nFrameCount > m_head_data.nTotalFrameCount - 1) nFrameCount = m_head_data.nTotalFrameCount - 1;
        return m_data_list[nFrameCount];
    }

    public static bool readTxtFile(string path, ref List<ModelCtrlData> target)
    {
        string line = "";

        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                line = sr.ReadLine();
                var temp = line.Split('\t');
                if (temp[0] != "MOVIE_DATA")
                    return false;

                while ((line = sr.ReadLine()) != null)
                {
                    ModelCtrlData frame = new ModelCtrlData();
                    frame.readData(line);
                    target.Add(frame);
                }

                sr.Close();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
            return false;
        }

    }
    /// <summary>
    /// 根据文件路径获取文件头，此为静态函数
    /// </summary>
    /// <param name="strFilePath"></param>
    /// <returns></returns>
    public static MovieHeadData GetHeadFromFile(string strFilePath)
    {
        string line = "";

        try
        {
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                line = sr.ReadLine();
            }
        }
        catch (Exception e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }


        return new MovieHeadData(line);
    }
}