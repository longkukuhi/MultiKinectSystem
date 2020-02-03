using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudyModelUIPanel : MonoBehaviour
{
    public GameObject m_StartCanvasPrefab;
    public GameObject m_ComPairMovieList;
    public GameObject m_StudyUIPanel;
    public GameObject m_StudyPanel;
    public Button m_BtnRefData;
    public GameObject m_CompairData;
    public Text m_WarnText;
    public Button m_StudyBtn;
    public GameObject m_Resoult;

    private StudyModeManager m_studyManager;
    private GameObject m_ChartCanvas;
    private bool bIsRef;
    private bool bIsRefFileSet;
    private bool bIsCompairFileSet;
    private bool bIsRecordSet;
    private bool bIsChart;
    // Use this for initialization
    void Start()
    {
        m_studyManager = GetComponent<StudyModeManager>();
        m_ChartCanvas = m_studyManager.GetStudyModeChartCanvas();
        bIsRef = true;
        bIsRefFileSet = false;
        bIsCompairFileSet = false;
        bIsRecordSet = false;
        bIsChart = false;
    }


    public void BtnRefData()
    {
        bIsRef = true;
        m_ComPairMovieList.SetActive(true);
    }

    public void BtnCurrentData()
    {
        bIsRef = false;
        m_ComPairMovieList.SetActive(true);
    }

    public void BtnRecord()
    {
        m_studyManager.SetRecordAsTrue();
        bIsRecordSet = true;
        m_CompairData.SetActive(true);
        m_CompairData.GetComponentInChildren<Text>().text = "将录制";
    }

    public void BtnPrepare()
    {
        bool bIsOK = true;
        if (!bIsRefFileSet)
        {
            m_WarnText.text = "请选择参考数据";
            bIsOK = false;
        }
        else if (!(bIsCompairFileSet || bIsRecordSet))
        {
            m_WarnText.text = "请选择现有数据或实时录制";
            bIsOK = false;
        }

        if (bIsOK)
        {
            if (!m_studyManager.Prepare())
            {
                m_WarnText.text = "请检查设备连接情况";
                return;
            }
            m_StudyUIPanel.SetActive(false);
            m_StudyPanel.SetActive(true);

        }
    }

    public void BtnStartStudy()
    {
        m_studyManager.StartOrStopStudy();
        m_StudyBtn.gameObject.SetActive(false);
    }

    public void BtnReturn()
    {
        Instantiate(m_StartCanvasPrefab);
        Destroy(gameObject);
    }

    public void BtnChart()
    {
        bIsChart = !bIsChart;
        m_ChartCanvas.SetActive(bIsChart);
    }

    public void StudyOver(float score)
    {
        m_Resoult.SetActive(true);
        m_Resoult.GetComponentInChildren<Text>().text = "学习结束\n得分："+string.Format("{0:F2}",score)+" 分";
    }

    public void SetFileName(string strFileName)
    {
        if (bIsRef)
        {
            m_studyManager.SetRefFileName(strFileName);
            bIsRefFileSet = true;
            var tempHeadData = FileReader.GetHeadFromFile(strFileName);
            m_BtnRefData.image.overrideSprite = ToolFunction.CreateSpriteFromImage(ToolFunction.GetDefaultPortraitPathByName(tempHeadData.strPortrait, ".jpg"));
            m_BtnRefData.GetComponentInChildren<Text>().text = tempHeadData.strDoctorName;
            m_BtnRefData.GetComponentInChildren<Text>().alignment = TextAnchor.LowerCenter;
        }
        else
        {
            m_studyManager.SetCompairFileName(strFileName);
            bIsCompairFileSet = true;
            m_CompairData.SetActive(true);
            var tempHeadData = FileReader.GetHeadFromFile(strFileName);
            m_CompairData.GetComponentInChildren<Image>().overrideSprite = ToolFunction.CreateSpriteFromImage(ToolFunction.GetDefaultPortraitPathByName( tempHeadData.strPortrait,".jpg"));
            m_CompairData.GetComponentInChildren<Text>().text = tempHeadData.strDoctorName;
        }
    }
}
