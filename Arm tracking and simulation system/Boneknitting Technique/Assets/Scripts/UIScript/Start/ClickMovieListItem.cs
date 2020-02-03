using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ClickMovieListItem : MonoBehaviour
{
    public GameObject PlayModeCanvas;
    public GameObject WarningTextPrefab;
    private GameObject StartModeCanvas;
    private string strFilePath;
   

    private void Start()
    {
        StartModeCanvas = GameObject.FindGameObjectWithTag("StartCanvas");
    }
    public void OnClickMovieListItem()
    {
        GameObject tPlayModeCanvas= Instantiate(PlayModeCanvas);
        tPlayModeCanvas.GetComponent<MoviePlayManager>().SetFileName(strFilePath);
        Destroy(StartModeCanvas);
    }

    public void OnClickDelet_Btn()
    {
        var WarningText = Instantiate(WarningTextPrefab, StartModeCanvas.transform);
        WarningText.GetComponent<StartDeleteWarningText>().SetDeleteObj(gameObject);
    }

    public void SetFilePath(string astrFilePath)
    {
        strFilePath = astrFilePath;
    }

    public string GetFilePath()
    {
        return strFilePath;
    }
}
