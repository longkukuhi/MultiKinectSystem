using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCompairListItem : MonoBehaviour {

    private string strFilePath;
    private GameObject m_StudyModeCanvas;
    private GameObject m_CompairMovieList;

    private void Start()
    {
        m_StudyModeCanvas = GameObject.FindGameObjectWithTag("StudyCanvas");
        m_CompairMovieList = GameObject.FindGameObjectWithTag("CompairMovieList");
    }
    public void OnClickCompairListItem()
    {
        m_StudyModeCanvas.GetComponent<StudyModelUIPanel>().SetFileName(strFilePath);
        m_CompairMovieList.SetActive(false);
    }

    public void SetFilePath(string astrFilePath)
    {
        strFilePath = astrFilePath;
    }
}
