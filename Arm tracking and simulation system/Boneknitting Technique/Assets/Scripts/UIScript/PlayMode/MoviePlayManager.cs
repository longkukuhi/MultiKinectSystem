using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviePlayManager : MonoBehaviour
{
    public GameObject m_HumenModelPrefab;
    public GameObject m_ChartCanvasPrefab;

    private GameObject m_HumenModel;
    private GameObject m_ChartCanvas;

    private string m_strFileName;
    private PlayController m_PlayController;
    private FileReader m_FileReader;
    private VideoRateCtrl m_VIdeoRateController;
    private ChartController m_PlayModeChartController;
    private bool bIsPlay;
    private float fTimeClock; //计时器以毫秒为单位

    private void Awake()
    {
        m_HumenModel = Instantiate(m_HumenModelPrefab);
        m_ChartCanvas = Instantiate(m_ChartCanvasPrefab);
        InitPlayModeChart();

        m_strFileName = null;
        m_PlayController = new PlayController(m_HumenModel);
        m_FileReader = null;
        m_VIdeoRateController = null;

        bIsPlay = false;
        fTimeClock = 0f;
    }

    private void Update()
    {
        if (bIsPlay)
        {
            var modelCtrlData = m_FileReader.PraseDataByFrameCount((int)m_VIdeoRateController.nCurrentFrame);
            m_PlayController.Update(modelCtrlData);
            m_PlayModeChartController.UpdateLineChart(ChartType.CHART_SPEED, m_VIdeoRateController.nCurrentFrame, TrailCurveDrawCtrl.Instance().lastSpeed(TrailType.EG_S1));
            m_PlayModeChartController.UpdateLineChart(ChartType.CHART_ACCELERATE, m_VIdeoRateController.nCurrentFrame, TrailCurveDrawCtrl.Instance().lastAcceleration(TrailType.EG_S1));
            //m_PlayModeChartController.UpdateLineChart(ChartType.CHART_CURVATURE, m_VIdeoRateController.nCurrentFrame, TrailCurveDrawCtrl.Instance().lastCurvature(TrailType.EG_S1));
            //m_PlayModeChartController.UpdateLineChart(ChartType.CHART_TORSION, m_VIdeoRateController.nCurrentFrame, TrailCurveDrawCtrl.Instance().lastTorsion(TrailType.EG_S1));

            m_VIdeoRateController.nCurrentFrame += m_VIdeoRateController.nAccelerate;
            if (m_VIdeoRateController.nCurrentFrame >= m_VIdeoRateController.nTotalFrameCount)
            {
                GetComponent<PlayRateControl>().OnStartOrStopClick();
            }
        }
    }

    public void SetFileName(string strFileName)
    {
        m_strFileName = strFileName;
        m_FileReader = new FileReader(strFileName);
        var tempDataHead = FileReader.GetHeadFromFile(strFileName);
        m_VIdeoRateController = new VideoRateCtrl(tempDataHead.nTotalFrameCount, 1000f / tempDataHead.nFPS);
    }

    public void StartOrStop()
    {
        bIsPlay = !bIsPlay;
    }

    public bool IsStart()
    {
        return bIsPlay;
    }

    public void Accelerate()
    {
        m_VIdeoRateController.nAccelerate *= 2;
    }

    public void Deaccelerate()
    {
        m_VIdeoRateController.nAccelerate *= 0.5f;
    }

    public float GetAccelerate()
    {
        return m_VIdeoRateController.nAccelerate;
    }

    public void SetCurrentFrame(float fRate)
    {
        m_VIdeoRateController.nCurrentFrame = (int)(m_VIdeoRateController.nTotalFrameCount * fRate);
    }

    public float GetCurrentTime()
    {
        return m_VIdeoRateController.nCurrentFrame * m_VIdeoRateController.fIntervalTime;
    }

    /// <summary>
    /// 获取录像总时间
    /// </summary>
    /// <returns></returns>
    public float GetTotalTime()
    {
        return m_VIdeoRateController.nTotalFrameCount * m_VIdeoRateController.fIntervalTime;
    }

    /// <summary>
    /// 返回当前播放比例
    /// </summary>
    /// <returns></returns>
    public float GetCurrentPlayRate()
    {
        float temp = m_VIdeoRateController.nCurrentFrame;
        return temp / m_VIdeoRateController.nTotalFrameCount;
    }

    public PlayController GetPlayController()
    {
        return m_PlayController;
    }

    private void InitPlayModeChart()
    {
        m_PlayModeChartController = m_ChartCanvas.GetComponent<ChartController>();
        m_PlayModeChartController.InitChart();
        m_PlayModeChartController.HideRefLineChart(ChartType.CHART_SPEED);
        m_PlayModeChartController.HideRefLineChart(ChartType.CHART_ACCELERATE);
        m_PlayModeChartController.HideRefLineChart(ChartType.CHART_CURVATURE);
        m_PlayModeChartController.HideRefLineChart(ChartType.CHART_TORSION);
    }

    public GameObject GetChartCanvas()
    {
        return m_ChartCanvas;
    }

    private void OnDestroy()
    {
        Destroy(m_HumenModel.transform.parent.gameObject);
        Destroy(m_ChartCanvas);
        m_PlayController.Destory();
    }
}
