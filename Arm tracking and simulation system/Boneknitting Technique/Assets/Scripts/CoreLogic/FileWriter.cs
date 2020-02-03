using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class FileWriter
{
    private List<ModelCtrlData> cacheDataList = new List<ModelCtrlData>();
    //此接口用于缓存每一帧传来的数据，（设备传来的数据）
    public void CacheData(ModelCtrlData modelCtrlData)
    {
        ModelCtrlData temp = new ModelCtrlData(modelCtrlData);
        cacheDataList.Add(temp);
    }

    /// <summary>
    ///  从缓存的数据中获取某一帧的模型控制数据
    /// </summary>
    /// <param name="nFrameCount">该帧</param>
    /// <returns></returns>
    public ModelCtrlData GetModelCtrlDataByTime(int nFrameCount)
    {

        if (nFrameCount < 0) nFrameCount = 0;
        if (nFrameCount > cacheDataList.Count - 1) nFrameCount = cacheDataList.Count - 1;

        return cacheDataList[nFrameCount];
    }

    /// <summary>
    /// 将数据存入文件
    /// </summary>
    /// <param name="headData">文件头部数据</param>
    /// <param name="strFileName">文件存储全路径</param>
    /// <param name="StartFrame">原数据的有效起始帧</param>
    /// <param name="EndFrame">原数据的有效终止帧</param>
    public void SaveDataToFile(MovieHeadData headData, string strFileName, int StartFrame = -1, float EndFrame = -1)
    {
        if (StartFrame < 0 || StartFrame >= cacheDataList.Count) StartFrame = 0;
        if (EndFrame < 1 || EndFrame >= cacheDataList.Count) EndFrame = cacheDataList.Count ;


        try
        {
            using (StreamWriter sw = new StreamWriter(strFileName, false))
            {
                //文件头
                sw.Write(headData.toStr());

                //数据
                for (int i = StartFrame; i < EndFrame; ++i)
                {
                    sw.Write(cacheDataList[i].toStr());
                }

                sw.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log("The file could not be write:");
            Debug.Log(e);
        }
    }

}
