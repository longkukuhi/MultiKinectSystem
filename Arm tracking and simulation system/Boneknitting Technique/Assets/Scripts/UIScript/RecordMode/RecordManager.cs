using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager:MonoBehaviour
{
    public GameObject m_HumenModelPrefab;
    public GameObject m_ChartCanvasPrefab;

    private RecordController m_RecordController;
    private bool bIsStartRecord;
    private bool bIsCalibrate;

    private VideoRateCtrl m_RecordRateController;

    private FileWriter m_FileWriter;
    private GameObject m_HumenModel;
    private GameObject m_ChartCanvas;
    private ChartController m_RecordModeChartController;

    private void Awake()
    {
        m_HumenModel = Instantiate(m_HumenModelPrefab);
        m_HumenModel.GetComponent<KinectManager>().enabled = true;

        m_ChartCanvas = Instantiate(m_ChartCanvasPrefab);
        InitRecordModelChartController();

        m_RecordController = new RecordController(m_HumenModel);
        bIsStartRecord = false;
        bIsCalibrate = false;
        m_RecordRateController = new VideoRateCtrl(0, 1000 / 30, 0);
        m_FileWriter = new FileWriter();
    }

    private void Update()
    {
        if (bIsCalibrate)
        {
            m_RecordController.Update();
        }
        if (bIsStartRecord)
        {
            ModelCtrlData modelCtrlData = m_RecordController.GetCurrentData();
            m_FileWriter.CacheData(modelCtrlData);
            m_RecordModeChartController.UpdateLineChart(ChartType.CHART_SPEED, m_RecordRateController.nTotalFrameCount, TrailCurveDrawCtrl.Instance().lastSpeed(TrailType.EG_S1));
            m_RecordModeChartController.UpdateLineChart(ChartType.CHART_ACCELERATE, m_RecordRateController.nTotalFrameCount, TrailCurveDrawCtrl.Instance().lastAcceleration(TrailType.EG_S1));
            m_RecordModeChartController.UpdateLineChart(ChartType.CHART_CURVATURE, m_RecordRateController.nTotalFrameCount, TrailCurveDrawCtrl.Instance().lastCurvature(TrailType.EG_S1));
            m_RecordModeChartController.UpdateLineChart(ChartType.CHART_TORSION, m_RecordRateController.nTotalFrameCount, TrailCurveDrawCtrl.Instance().lastTorsion(TrailType.EG_S1));

            m_RecordRateController.nTotalFrameCount += 1;  //录制帧编号从0开始
        }
    }

    public RecordController GetRecordController()
    {
        return m_RecordController;
    }

    public bool InitDevice()
    {
        return m_RecordController.InitDevice();
    }

    public void DisconnnectDevice()
    {
        m_RecordController.DisconnectDevice();
    }

    public bool IsStartRecord()
    {
        return bIsStartRecord;
    }

    public void StartCalibrate()
    {
        bIsCalibrate = true;
    }

    public void StartOrStopRecord()
    {
        bIsStartRecord = !bIsStartRecord;
    }

    public void SaveDataToFile(MovieHeadData headData, string strFileName, int nStartTime = -1, int nEndTime = -1)
    {
        m_FileWriter.SaveDataToFile(headData, strFileName, nStartTime, nEndTime);
    }

    public ModelCtrlData GetModelCtrlDataByTime(int nFrameCount)
    {
        return m_FileWriter.GetModelCtrlDataByTime(nFrameCount);
    }

    /// <summary>
    /// 返回一个以毫秒为单位的录制时间长度
    /// </summary>
    /// <returns></returns>
    public float GetTimeCount()
    {
        return m_RecordRateController.nTotalFrameCount*m_RecordRateController.fIntervalTime;
    }

    public int GetFrameCount()
    {
        return m_RecordRateController.nTotalFrameCount;
    }

    private void InitRecordModelChartController()
    {
        m_RecordModeChartController = m_ChartCanvas.GetComponent<ChartController>();
        m_RecordModeChartController.InitChart();
        m_RecordModeChartController.HideRefLineChart(ChartType.CHART_SPEED);
        m_RecordModeChartController.HideRefLineChart(ChartType.CHART_ACCELERATE);
        m_RecordModeChartController.HideRefLineChart(ChartType.CHART_CURVATURE);
        m_RecordModeChartController.HideRefLineChart(ChartType.CHART_TORSION);
    }

    public GameObject GetRecordModeChartCanvas()
    {
        return m_ChartCanvas;
    }

    private void OnDestroy()
    {
        Destroy(m_HumenModel.transform.parent.gameObject);
        Destroy(m_ChartCanvas);
        m_RecordController.GetPlayController().Destory();
    }

    /// <summary>
    /// 在2018版的Unity中可以通过监听Application.quitting事件来实现监听程序退出
    /// </summary>
    private void OnApplicationQuit()
    {
        DisconnnectDevice();
    }
}
