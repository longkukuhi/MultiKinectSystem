using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBar : MonoBehaviour
{
    public GameObject m_PlayModeCanvas;
    public GameObject m_RecordModeCanvas;
    public GameObject m_StudyModeCanvas;

    public void OnRecordMode()
    {
        Instantiate(m_RecordModeCanvas);
        Destroy(gameObject);
    }

    public void OnStudyMode()
    {
        Instantiate(m_StudyModeCanvas);
        Destroy(gameObject);
    }

    public void OnImportFile()
    {
        string filePath = ToolFunction.OpenFilePath("*.txt", "打开文件", ".txt");
        ConfigCenter.Instance().AddHistoryFile(filePath);
        GetComponent<MovieCatalogueScript>().AddListItembByFileName(filePath);
    }
}
