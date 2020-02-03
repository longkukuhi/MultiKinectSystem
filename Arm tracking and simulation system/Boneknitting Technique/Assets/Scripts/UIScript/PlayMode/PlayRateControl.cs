using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayRateControl : MonoBehaviour
{
    public Slider m_TimeSlider;
    public Text m_TimeCount;
    public Text m_AccelerateCount;
    public Button m_BtnStartOrStop;
    public Sprite m_Playing;
    public Sprite m_Stopping;

    private MoviePlayManager m_MoviePlayManager;

    private void Start()
    {
        m_MoviePlayManager = GetComponent<MoviePlayManager>();
    }

    public void LateUpdate()
    {
        if (m_MoviePlayManager.IsStart())
        {
            m_TimeCount.text = ToolFunction.TranslateToMMSS( m_MoviePlayManager.GetCurrentTime()) + "/" + ToolFunction.TranslateToMMSS(m_MoviePlayManager.GetTotalTime());
            m_TimeSlider.value = m_MoviePlayManager.GetCurrentPlayRate();
        }
    }

    public void OnTimeChanged()
    {
        m_MoviePlayManager.SetCurrentFrame(m_TimeSlider.value);
    }

    public void OnAccelerateClick()
    {
        m_MoviePlayManager.Accelerate();
        m_AccelerateCount.text = "x" + m_MoviePlayManager.GetAccelerate();
    }

    public void OnStartOrStopClick()
    {
        m_MoviePlayManager.StartOrStop();
        if(m_MoviePlayManager.IsStart())
        {
            m_BtnStartOrStop.image.overrideSprite = m_Playing;
        }
        else
        {
            m_BtnStartOrStop.image.overrideSprite = m_Stopping;
        }
    }

    public void OnDeaccelerateClick()
    {
        m_MoviePlayManager.Deaccelerate();
        m_AccelerateCount.text = "x" + m_MoviePlayManager.GetAccelerate();
    }
}
