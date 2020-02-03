using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour
{
    public GameObject m_SavePanel;
    public Image m_PortraitImage;
    public GameObject m_InfoSaved;
    public DoubleEndSlider m_CutSlider;
    public InputField m_InputDoctorName;
    public InputField m_InputFilePath;

    private RecordManager m_RecordManager;

    private string m_FilePath;
    private float m_fLeftSliderValue;
    private float m_fRightSliderValue;
    private bool bIsLeft;

    private void Start()
    {
        m_CutSlider.eValueChange += CutSlider_OnValueChange;
        m_RecordManager = GetComponent<RecordManager>();

        m_FilePath = null;
        m_fLeftSliderValue = 0.0f;
        m_fRightSliderValue = 1.0f;
        bIsLeft = false;
    }

    private void CutSlider_OnValueChange(float value, bool bIsLeft)
    {
        if (bIsLeft)
        {
            m_fLeftSliderValue = value;
        }
        else
        {
            m_fRightSliderValue = value;
        }
        int tempTotalFrameCount = m_RecordManager.GetFrameCount();
        ModelCtrlData tempModelCtrlData = m_RecordManager.GetModelCtrlDataByTime((int)(tempTotalFrameCount * value));
        m_RecordManager.GetRecordController().GetPlayController().Update(tempModelCtrlData);
    }

    public void Btn_Confirm()
    {
        m_SavePanel.SetActive(true);
        m_CutSlider.gameObject.SetActive(false);
    }

    public void PortraitPath()
    {
        string filter = "*.jpg,*.png";
        string title = "选择头像";
        string extension = ".png";
        string selectPortraitPath = ToolFunction.OpenFilePath(filter, title, extension);
        var tempSprite = ToolFunction.CreateSpriteFromImage(selectPortraitPath);
        m_PortraitImage.overrideSprite = tempSprite;
    }

    public void Btn_Save()
    {
        if (string.IsNullOrEmpty(m_InputDoctorName.text) ||
            string.IsNullOrEmpty(m_InputFilePath.text))
        {
            m_InputDoctorName.placeholder.GetComponent<Text>().color = Color.red;
            m_InputFilePath.placeholder.GetComponent<Text>().color = Color.red;
        }
        else
        {
            //根据输入的文件名和选择的头像，将文件存储到默认存储文件夹，将头像图片复制到默认头像存储文件夹
            m_FilePath = ToolFunction.GetMovieSaveFilePath(m_InputFilePath.text, ".txt");
            var tempPortrait = ToolFunction.GenerateStringID();
            ToolFunction.ImageSaveLocal(m_PortraitImage.mainTexture, ToolFunction.GetDefaultPortraitPathByName(tempPortrait, ".jpg"));
            int tempTimeCount = m_RecordManager.GetFrameCount();
            int tempStartTime = (int)(tempTimeCount * m_fLeftSliderValue);
            int tempEndTime = (int)(tempTimeCount * m_fRightSliderValue);
            MovieHeadData tempData = new MovieHeadData(
                "MOVIE_DATA",
     m_InputDoctorName.text,
     tempPortrait,
     System.DateTime.Now.ToString("MM/dd/yyyy H:mm:ss"),
     tempEndTime - tempStartTime,
     0,
     ConfigCenter.Instance().GetFPS()
    );
            m_RecordManager.SaveDataToFile(
                tempData,
                m_FilePath,
                tempStartTime,
                tempEndTime
                );

            m_SavePanel.SetActive(false);
            m_InfoSaved.SetActive(true);
        }

    }



}
