using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayModeControlPanel : MonoBehaviour
{
    public GameObject m_StartCanvasPrefab;
    private GameObject m_ChartCanvas;
    private PlayController m_PlayerController;
    private void Start()
    {
        m_PlayerController = GetComponent<MoviePlayManager>().GetPlayController();
        m_ChartCanvas = GetComponent<MoviePlayManager>().GetChartCanvas();
    }

    bool S1 = true;
    bool S2 = true;
    bool S3 = true;
    bool S4 = true;
    bool bIsChart = false;

    public void BtnS1()
    {
        S1 = !S1;
        m_PlayerController.SwitchTrail(TrailType.EG_S1, S1);
    }

    public void BtnS2()
    {
        S2 = !S2;
        m_PlayerController.SwitchTrail(TrailType.EG_S2, S2);
    }

    public void BtnS3()
    {
        S3 = !S3;
        m_PlayerController.SwitchTrail(TrailType.EG_S3, S3);
    }

    public void BtnS4()
    {
        S4 = !S4;
        m_PlayerController.SwitchTrail(TrailType.EG_S4, S4);
    }


    // Use this for initialization
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

}
