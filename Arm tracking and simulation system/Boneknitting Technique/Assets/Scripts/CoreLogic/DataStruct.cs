using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using TrajectoryData;

public class ModelCtrlData
{
    public int frame;
    public HandCtrlData handCtrlData;
    public BodyCtrlData bodyCtrlData;
    public WristCtrlData wristCtrlData;

    public ModelCtrlData()
    {
        handCtrlData = new HandCtrlData();
        bodyCtrlData = new BodyCtrlData();
        wristCtrlData = new WristCtrlData();
    }

    public ModelCtrlData(ModelCtrlData obj)
    {
        handCtrlData = new HandCtrlData(obj.handCtrlData);
        bodyCtrlData = new BodyCtrlData(obj.bodyCtrlData);
        wristCtrlData = new WristCtrlData(obj.wristCtrlData);

    }
    public TPose toLeftTPose()
    {
        TPose pose = new TPose();

        pose.time = frame.ToString();
        pose.position = new Vec3(bodyCtrlData.HandLeftPos);
        pose.azimuth = wristCtrlData.left_wrist_rotate.z;
        pose.elevation = wristCtrlData.left_wrist_rotate.y;
        pose.roll = wristCtrlData.left_wrist_rotate.x;

        return pose;
    }
    public TPose toRightTPose()
    {
        TPose pose = new TPose();

        pose.time = frame.ToString();
        pose.position = new Vec3(bodyCtrlData.HandRightPos);
        pose.azimuth = wristCtrlData.right_wrist_rotate.z;
        pose.elevation = wristCtrlData.right_wrist_rotate.y;
        pose.roll = wristCtrlData.right_wrist_rotate.x;

        return pose;
    }
    public string toStr()
    {
        string str = string.Format("{0}\t",frame);

        str += handCtrlData.toStr();
        str += bodyCtrlData.toStr();
        str += wristCtrlData.toStr();
        str += "\n";
        return str;
    }

    public void readData(string str_data)
    {
        var data = str_data.Split('\t');
        frame = int.Parse(data[0]);
        handCtrlData.readData(data);
        bodyCtrlData.readData(data);
        wristCtrlData.readData(data);
    }
}

public class MovieHeadData
{
    //表示用于判断文件格式是否合法
    public string strIdentify;
    public string strDoctorName;
    //头像名，从Portrait文件夹中获取头像
    public string strPortrait;
    //l录制时间 如 2018050914  （年月日时）
    public string strGenerateTime;
    //录像裁剪后的总帧数
    public int nTotalFrameCount;
    //当前帧数
    public int nCurrentFrame;
    //录像帧率
    public int nFPS;

    public MovieHeadData(string identify,string name,string portrait,string time,int nTotalCount,int nCurrentFrame, int nFPS)
    {
        strIdentify = identify;
        strDoctorName = name;
        strPortrait = portrait;
        strGenerateTime = time;
        nTotalFrameCount = nTotalCount;
        this.nCurrentFrame = nCurrentFrame;
        this.nFPS = nFPS;
    }

    /// <summary>
    /// 将MovieHeadData转换成一个用'\t'分隔的字符串
    /// </summary>
    /// <returns></returns>
    public string toStr()
    {
        Debug.Log(string.Format("{0}\t{1}\t{2}\t{3}\t{4:D}\t{5:D}\t{6:D}\n", strIdentify, strDoctorName, strPortrait, strGenerateTime, nTotalFrameCount, nCurrentFrame, nFPS));
        return string.Format("{0}\t{1}\t{2}\t{3}\t{4:D}\t{5:D}\t{6:D}\n", strIdentify,strDoctorName, strPortrait, strGenerateTime, nTotalFrameCount,nCurrentFrame, nFPS);
    }

    /// <summary>
    /// 使用一个字符串来初始化MovieHeadData
    /// </summary>
    /// <param name="str"></param>
    public void ReadData(string str)
    {
        string[] temp = str.Split('\t');//该数组第一位是数据类型标志位，所以从有用数据从下标1开始
        strIdentify = temp[0];
        if (strIdentify == "MOVIE_DATA")
        {
            strDoctorName = temp[1];
            strPortrait = temp[2];
            strGenerateTime = temp[3];
            nTotalFrameCount = int.Parse(temp[4]);
            nCurrentFrame = int.Parse(temp[5]);
            nFPS = int.Parse(temp[6]);
        }
        else
        {
            strDoctorName = "FILE_ERROR";
            strPortrait = "FILE_ERROR";
            strGenerateTime = "FILE_ERROR";
            nTotalFrameCount = 0;
            nCurrentFrame = 0;
            nFPS = 0;
        }
    }
    /// <summary>
    /// 使用一个字符串来构造MovieHeadData
    /// </summary>
    /// <param name="str"></param>
    public MovieHeadData(string str)
    {
        ReadData(str);
    }
}

public class VideoRateCtrl
{
    private int m_nTotalFrameCount;
    public int nTotalFrameCount
    {
        get { return m_nTotalFrameCount; }
        set { m_nTotalFrameCount = value; }
    }

    private float m_nCurrentFrame;    //当前帧
    public float nCurrentFrame
    {
        set
        {
            if(value>m_nTotalFrameCount)
            {
                m_nCurrentFrame = m_nTotalFrameCount;
            }
            else
            {
                m_nCurrentFrame = value;
            }
        }
        get
        {
            return m_nCurrentFrame;
        }
    }

    private float m_fIntervalTime;   //当前播放时间间隔
    public float fIntervalTime
    {
        set
        {
                m_fIntervalTime = value;
        }
        get
        {
            return m_fIntervalTime;
        }
    }

    private float m_nAccelerate;
    public float nAccelerate
    {
        set
        {
            m_nAccelerate = value;
        }
        get
        {
            return m_nAccelerate;
        }
    }

    public VideoRateCtrl(int nTotalFrameCount, float fIntervalTime, int nCurrentFrame= 0)
    {
        InitVideoRateCtrl(nTotalFrameCount, fIntervalTime, nCurrentFrame);
    }

    public bool InitVideoRateCtrl(int nTotalFrameCount, float fIntervalTime, int nCurrentFrame = 0)
    {
        m_nTotalFrameCount = nTotalFrameCount;
        m_nCurrentFrame = nCurrentFrame;
        m_fIntervalTime = fIntervalTime;
        m_nAccelerate = 1.0001f;
        return true;
    }
}

public enum TrailType
{
    EG_S1,
    EG_S2,
    EG_S3,
    EG_S4
}

public enum ChartType
{
    CHART_SPEED,
    CHART_ACCELERATE,
    CHART_CURVATURE,
    CHART_TORSION
}

public class HandCtrlData
{
    public float[] HandData;

    public string toStr()
    {
        string str = "";

        for (int i = 0; i < FileConfig.FIVE_DT_NODE_NUM; ++i)
        {
            str += string.Format("{0}\t", HandData[i]);
        }

        return str;
    }
    public void readData(string[] data)
    {
        int start_index = 1;
        for (int i = 0; i < FileConfig.FIVE_DT_NODE_NUM; ++i)
        {
            HandData[i] = float.Parse(data[i + start_index]);
        }
    }

    public HandCtrlData()
    {
        HandData = new float[14];
    }

    /// <summary>
    /// 复制构造函数
    /// </summary>
    /// <param name="data"></param>
    public HandCtrlData(HandCtrlData data)
    {
        HandData = new float[14];
        for(int i = 0; i < 14; ++i)
        {
            HandData[i] = data.HandData[i];
        }
    }
}
public class BodyCtrlData
{
    public Quaternion[] jointRotation;
    public Vector3 userPosition;
    public Vector3 HandLeftPos;
    public Vector3 HandRightPos;
    public uint UserID;
    public string toStr()
    {
        string str = "";

        for (int i = 0; i < FileConfig.KINECT_NODE_NUM; ++i)
        {
            str += string.Format("{0}\t{1}\t{2}\t{3}\t",
                                        jointRotation[i].x,
                                        jointRotation[i].y,
                                        jointRotation[i].z,
                                        jointRotation[i].w);
        }

        str += string.Format("{0}\t{1}\t{2}\t",
                                        userPosition.x,
                                        userPosition.y,
                                        userPosition.z);

        str += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t",
                        HandLeftPos.x,
                        HandLeftPos.y,
                        HandLeftPos.z,
                        HandRightPos.x,
                        HandRightPos.y,
                        HandRightPos.z);

        str += string.Format("{0}\t", UserID);

        return str;
    }
    public void readData(string[] data)
    {
        int start_index = 1 + FileConfig.FIVE_DT_NODE_NUM;
        //骨节点
        for (int i = 0; i < FileConfig.KINECT_NODE_NUM; ++i)
        {
            jointRotation[i].x = float.Parse(data[start_index + i * 4]);
            jointRotation[i].y = float.Parse(data[start_index + i * 4 + 1]);
            jointRotation[i].z = float.Parse(data[start_index + i * 4 + 2]);
            jointRotation[i].w = float.Parse(data[start_index + i * 4 + 3]);
        }

        start_index = start_index + FileConfig.KINECT_NODE_NUM * 4;

        //模型整体位置
        userPosition.x = float.Parse(data[start_index]);
        userPosition.y = float.Parse(data[start_index + 1]);
        userPosition.z = float.Parse(data[start_index + 2]);

        start_index = start_index + 3;
        //左右手腕位置
        HandLeftPos.x = float.Parse(data[start_index]);
        HandLeftPos.y = float.Parse(data[start_index + 1]);
        HandLeftPos.z = float.Parse(data[start_index + 2]);
        HandRightPos.x = float.Parse(data[start_index + 3]);
        HandRightPos.y = float.Parse(data[start_index + 4]);
        HandRightPos.z = float.Parse(data[start_index + 5]);

        start_index = start_index + 6;

        //用户ID
        UserID = uint.Parse(data[start_index]);
    }

    public BodyCtrlData()
    {
        jointRotation = new Quaternion[22];
    }

    public BodyCtrlData(BodyCtrlData data)
    {
        jointRotation = new Quaternion[22];
        for(int i=0;i<jointRotation.Length;++i)
        {
            jointRotation[i] = data.jointRotation[i];
        }
        userPosition = data.userPosition;
        HandLeftPos = data.HandLeftPos;
        HandRightPos = data.HandRightPos;
        UserID = data.UserID;
    }

}
public class WristCtrlData
{
    public Vector3 left_wrist_rotate;
    public Vector3 right_wrist_rotate;
    //public Vector3 left_wrist_accelerometer;
    //public Vector3 right_wrist_accelerometer;
    //public Vector3 left_wrist_angular_velocity;
    //public Vector3 right_wrist_angular_velocity;

    public WristCtrlData()
    {
        left_wrist_rotate = new Vector3();
        right_wrist_rotate = new Vector3();
    }

    public WristCtrlData(WristCtrlData data)
    {
        left_wrist_rotate = data.left_wrist_rotate;
        right_wrist_rotate = data.right_wrist_rotate;
    }
    public string toStr()
    {
        string str = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t",
                        left_wrist_rotate.x,
                        left_wrist_rotate.y,
                        left_wrist_rotate.z,
                        right_wrist_rotate.x,
                        right_wrist_rotate.y,
                        right_wrist_rotate.z);

        return str;
    }
    public void readData(string[] data)
    {
        int start_index = 1 + FileConfig.FIVE_DT_NODE_NUM + FileConfig.KINECT_NODE_NUM * 4 + 3 + 6 + 1;
        //左右手腕旋转量
        left_wrist_rotate.x = float.Parse(data[start_index]);
        left_wrist_rotate.y = float.Parse(data[start_index + 1]);
        left_wrist_rotate.z = float.Parse(data[start_index + 2]);
        right_wrist_rotate.x = float.Parse(data[start_index + 3]);
        right_wrist_rotate.y = float.Parse(data[start_index + 4]);
        right_wrist_rotate.z = float.Parse(data[start_index + 5]);
    }
}