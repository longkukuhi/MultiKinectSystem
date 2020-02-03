using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecodModePanel : MonoBehaviour
{
    public GameObject m_BtnCalibrate;
    public GameObject m_BtnBeginRecord;
    public GameObject m_BtnStopRecord;
    public GameObject m_SaveControlPanel;
    public GameObject m_StartCanvas;
    public GameObject m_WarningText;
    public Text m_TimeCount;

    private GameObject m_ChartCanvas;
    private RecordManager m_RecordManager;
    private bool bIsChart = false;
    private void Start()
    {
        m_RecordManager = GetComponent<RecordManager>();
        m_ChartCanvas = m_RecordManager.GetRecordModeChartCanvas();
    }

    private void Update()
    {
        if(m_RecordManager.IsStartRecord())
        {
            m_TimeCount.text = ToolFunction.TranslateToMMSS(m_RecordManager.GetTimeCount());
        }
    }

    public void BtnBeginRecord()
    {
        m_BtnStopRecord.SetActive(true);
        m_RecordManager.StartOrStopRecord();
        Destroy(m_BtnBeginRecord);
    }

    public void BtnCalibrate()
    {
        if (m_RecordManager.InitDevice())
        {
            m_WarningText.SetActive(false);
            m_BtnBeginRecord.SetActive(true);
            m_RecordManager.StartCalibrate();
            Destroy(m_BtnCalibrate);
        }
        else
        {
            m_WarningText.SetActive ( true);
        }
    }

    public void BtnStopRecord()
    {
        m_SaveControlPanel.SetActive(true);
        m_RecordManager.StartOrStopRecord();
        m_RecordManager.DisconnnectDevice();
        Destroy(m_BtnStopRecord);
    }

    public void BtnReturn()
    {
        Instantiate(m_StartCanvas);
        m_RecordManager.DisconnnectDevice();
        Destroy(gameObject);
    }

    public void BtnChart()
    {
        bIsChart = !bIsChart;
        m_ChartCanvas.SetActive(bIsChart);
    }
}
