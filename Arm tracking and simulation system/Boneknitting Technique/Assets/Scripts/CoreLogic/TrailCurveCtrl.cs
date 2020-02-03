using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrajectoryData;

public class TrailCurveDrawCtrl
{
    //储存当前屏幕显示，即要操作的轨迹
    public HandMotion curMotion;
    public HandMotion studyMotion;
    //储存当前已经打开（读取）的轨迹
    //public List<HandMotion> motionList = new List<HandMotion>();
    public int curve_length = 50;
    private bool is_first = true;
    private bool is_linerenderer = true ;
    //轨迹数量
    private int trajs_num;

    private TrailCurveDrawCtrl()
    {
        trajs_num = 2;
        curMotion = new HandMotion();
        for (int i = 0; i < trajs_num; ++i)
        {
            curMotion.Add(new Trajectory());
        }
        studyMotion = new HandMotion();
        for (int i = 0; i < trajs_num; ++i)
        {
            studyMotion.Add(new Trajectory());
        }
    }
    private static TrailCurveDrawCtrl trajctrl;
    public static TrailCurveDrawCtrl Instance()
    {
        if (trajctrl == null)
            trajctrl = new TrailCurveDrawCtrl();

        return trajctrl;
    }
    public void resetState()
    {
        GameObject.FindGameObjectWithTag("DrawCurve").GetComponent<DrawCurvesWithGLColor>().enabled = false;
        GameObject.FindGameObjectWithTag("DrawCurve").GetComponent<DrawCurvesWithGL>().enabled = false;
        GameObject.FindGameObjectWithTag("DrawCurve").GetComponent<DrawCurvesWithLineRenderer>().enabled = false;
        curMotion.clearTrailData();
        studyMotion.clearTrailData();
        TrailCurveAppraiseCtrl.left_color_list.Clear();
        TrailCurveAppraiseCtrl.right_color_list.Clear();
        is_first = true;
    }
    //设置需要绘制曲线的长度
    public void setCurveLength(int length)
    {
        curve_length = length;
    }
    //设置需要绘制曲线的长度
    public void setIsLineRenderer(bool is_lr)
    {
        is_linerenderer = is_lr;
    }
    public void startDraw(bool is_study)
    {
        if (is_linerenderer && !is_study)
            GameObject.FindGameObjectWithTag("DrawCurve").GetComponent<DrawCurvesWithLineRenderer>().enabled = true;
        else if (is_study)
            GameObject.FindGameObjectWithTag("DrawCurve").GetComponent<DrawCurvesWithGLColor>().enabled = true;
        else
            GameObject.FindGameObjectWithTag("DrawCurve").GetComponent<DrawCurvesWithGL>().enabled = true;
    }

    /// <summary>
    /// 此接口用于设置那个轨迹要绘制哪个不绘制
    /// </summary>
    /// <param name="trailType">需要切换的轨迹类型</param>
    /// <param name="IsOn">该轨迹开启绘制还是关闭绘制</param>
    public void SwitchTrailCurve(TrailType trailType, bool IsOn)
    {
        Debug.Log(trailType + "---------" + IsOn);
        curMotion.getTraj(trailType.GetHashCode()).setActive(IsOn);
    }

    /// <summary>
    /// 接收用于绘制轨迹的参数，自行决定是否绘制轨迹每一帧调用
    /// </summary>
    /// <param name="modelCtrlData"></param>
    public void RecvTrailData(ModelCtrlData modelCtrlData)
    {
        curMotion.getTraj(0).push_back(modelCtrlData.toLeftTPose());
        curMotion.getTraj(1).push_back(modelCtrlData.toRightTPose());

        if (is_first)
        {
            startDraw(false);
            is_first = false;
        }

    }

    //接收参数，剪切保留某一曲线上在某时间端内的轨迹
    public void ClipTrailWithinTime(int sFrame, int eFrame)
    {
        if (curMotion == null)
            return;

        for (int i = 0; i < curMotion.size(); ++i)
        {
            curMotion.getTraj(i).clip(sFrame, eFrame);
        }
    }
    //-------------轨迹图表----------------
    public List<float> speed(TrailType trailType)
    {
        return curMotion.getTraj(trailType.GetHashCode()).speed();
    }
    public List<float> acceleration(TrailType trailType)
    {
        return curMotion.getTraj(trailType.GetHashCode()).acceleration();
    }
    public List<float> curvature(TrailType trailType)
    {
        return curMotion.getTraj(trailType.GetHashCode()).curvature();
    }
    public List<float> torsion(TrailType trailType)
    {
        return curMotion.getTraj(trailType.GetHashCode()).torsion();
    }
    public float lastSpeed(TrailType trailType, bool is_study = false)
    {
        return is_study ? studyMotion.getTraj(trailType.GetHashCode()).lastSpeed() : curMotion.getTraj(trailType.GetHashCode()).lastSpeed();
    }
    public float lastAcceleration(TrailType trailType, bool is_study = false)
    {
        return is_study ? studyMotion.getTraj(trailType.GetHashCode()).lastAcceleration() : curMotion.getTraj(trailType.GetHashCode()).lastAcceleration();
    }
    public float lastCurvature(TrailType trailType, bool is_study = false)
    {
        return is_study ? studyMotion.getTraj(trailType.GetHashCode()).lastCurvature() : curMotion.getTraj(trailType.GetHashCode()).lastCurvature();
    }
    public float lastTorsion(TrailType trailType, bool is_study = false)
    {
        return is_study ? studyMotion.getTraj(trailType.GetHashCode()).lastTorsion() : curMotion.getTraj(trailType.GetHashCode()).lastTorsion();
    }
}

public class TrailCurveAppraiseCtrl
{
    public static List<float> left_color_list = new List<float>();
    public static List<float> right_color_list = new List<float>();
    private int cur_frame = 0;
    private Vec3 deta_left = new Vec3(0.0f, 0.0f, 0.0f);
    private Vec3 deta_right = new Vec3(0.0f, 0.0f, 0.0f);
    /// <summary>
    /// 接收两个轨迹数据，(用于轨迹分析)，每一帧调用
    /// </summary>
    /// <param name="refData">参考数据，此数据作为参考对象</param>
    /// <param name="AppraiseData">分析数据，需要对此数据进行分析</param>
    public void RecvCompairTrailData(ModelCtrlData refData, ModelCtrlData AppraiseData)
    {
        TrailCurveDrawCtrl.Instance().studyMotion.getTraj(0).add(AppraiseData.toLeftTPose());
        TrailCurveDrawCtrl.Instance().studyMotion.getTraj(1).add(AppraiseData.toRightTPose());

        if (cur_frame < 5)
        {
            deta_left += new Vec3(AppraiseData.bodyCtrlData.HandLeftPos - refData.bodyCtrlData.HandLeftPos);
            deta_right += new Vec3(AppraiseData.bodyCtrlData.HandRightPos - refData.bodyCtrlData.HandRightPos);

            if (cur_frame == 4)
            {
                deta_left /= 5;
                deta_right /= 5;
            }
            left_color_list.Add(0.0f);
            right_color_list.Add(0.0f);
            if (cur_frame == 0)
            {
                TrailCurveDrawCtrl.Instance().startDraw(true);
            }
        }
        else
        {
            left_color_list.Add(Math.Abs((float)(new Vec3(AppraiseData.bodyCtrlData.HandLeftPos - refData.bodyCtrlData.HandLeftPos) - deta_left).norm()));
            right_color_list.Add(Math.Abs((float)(new Vec3(AppraiseData.bodyCtrlData.HandRightPos - refData.bodyCtrlData.HandRightPos) - deta_right).norm()));
        }
        cur_frame++;
    }

    //根据之前的分析给出评分
    public float AppraiseTrailCurve()
    {
        float deta = 0.0f;
        for (int i = 0; i < cur_frame; ++i)
        {
            deta += left_color_list[i] + right_color_list[i];
        }
        //deta /= 20;

        float DTW_score_left = compareTrailCurveWithDTW(TrailCurveDrawCtrl.Instance().curMotion.getTraj(0), TrailCurveDrawCtrl.Instance().studyMotion.getTraj(0));
        float DTW_score_right = compareTrailCurveWithDTW(TrailCurveDrawCtrl.Instance().curMotion.getTraj(1), TrailCurveDrawCtrl.Instance().studyMotion.getTraj(1));

        if ((DTW_score_left + DTW_score_right) / 2 > 0.0f)
        {
            return (DTW_score_left + DTW_score_right) / 2;
        }
        else
        {
            return (100.0f - deta) > 0.0f ? (100.0f - deta) : 0.0f;
        }
    }

    //DTW方式查看两条曲线的拟合度
    public static float compareTrailCurveWithDTW(Trajectory traj1, Trajectory traj2)
    {
        if (traj1.size() < 2 || traj2.size() < 2 || traj1.size() * traj2.size() > 400000)
            return -1.0f;

        float[,] distance_martix = new float[traj1.size(), traj2.size()];
        for (int i = 0; i < traj1.size(); ++i)
        {
            for (int j = 0; j < traj2.size(); ++j)
            {
                float temp = 0.0f;
                temp += Math.Abs(traj1.vec[i].azimuth - traj2.vec[j].azimuth);
                temp += Math.Abs(traj1.vec[i].roll - traj2.vec[j].roll);
                temp += Math.Abs(traj1.vec[i].elevation - traj2.vec[j].elevation);
                temp += (float)(traj1.vec[i].position - traj2.vec[j].position).norm();
                distance_martix[i, j] = temp;
            }
        }

        distance_martix[0, 0] *= 2;

        for (int i = 1; i < traj1.size(); ++i)
        {
            distance_martix[i, 0] = distance_martix[i, 0] + distance_martix[i - 1, 0];
        }
        for (int j = 1; j < traj2.size(); ++j)
        {
            distance_martix[0, j] = distance_martix[0, j] + distance_martix[0, j - 1];
        }

        for (int i = 1; i < traj1.size(); ++i)
        {
            for (int j = 1; j < traj2.size(); ++j)
            {
                distance_martix[i, j] = Math.Min(distance_martix[i - 1, j - 1] + 2 * distance_martix[i, j], Math.Min(distance_martix[i - 1, j] + distance_martix[i, j], distance_martix[i - 1, j] + distance_martix[i, j - 1]));
            }
        }

        List<float> path = new List<float>();
        path.Add(distance_martix[0, 0]);

        int row = 0;
        int col = 0;
        while (row < traj1.size() - 1 && col < traj2.size() - 1)
        {
            if (distance_martix[row + 1, col] <= distance_martix[row + 1, col + 1] && distance_martix[row + 1, col] <= distance_martix[row, col + 1])
            {
                path.Add(distance_martix[row + 1, col]);
                row += 1;
            }
            else if (distance_martix[row, col + 1] <= distance_martix[row + 1, col + 1] && distance_martix[row, col + 1] <= distance_martix[row + 1, col])
            {
                path.Add(distance_martix[row, col + 1]);
                col += 1;
            }
            else if (distance_martix[row + 1, col + 1] <= distance_martix[row, col + 1] && distance_martix[row + 1, col + 1] <= distance_martix[row + 1, col])
            {
                path.Add(distance_martix[row + 1, col + 1]);
                row += 1;
                col += 1;
            }
        }

        if (row == traj1.size() - 1)
        {
            while (col < traj2.size() - 1)
            {
                path.Add(distance_martix[row, col + 1]);
                col++;
            }
        }
        else if (col == traj2.size() - 1)
        {
            while (row < traj1.size() - 1)
            {
                path.Add(distance_martix[row + 1, col]);
                row++;
            }
        }

        float ave_distance = distance_martix[row, col] / path.Count;

        float variance = 0.0f;
        for (int i = 0; i < path.Count; ++i)
        {
            variance += (float)Math.Pow(path[i] - ave_distance, 2);
        }

        float score = 100 - (float)Math.Sqrt(variance / path.Count);
        return score > 0 ? score : 0;
    }
}
