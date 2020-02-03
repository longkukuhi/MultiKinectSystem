using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StartDeleteWarningText : MonoBehaviour {

    private GameObject m_DeleteObj;
    public void SetDeleteObj(GameObject DeleteObj)
    {
        m_DeleteObj = DeleteObj;
    }

    public void BtnYes()
    {
        var filepath = m_DeleteObj.GetComponent<ClickMovieListItem>().GetFilePath();
        ConfigCenter.Instance().DeleteFileByPath(filepath);
        var temphead = FileReader.GetHeadFromFile(filepath);
        var portraitpath = ToolFunction.GetDefaultPortraitPathByName(temphead.strPortrait, ".jpg");
        if (File.Exists(portraitpath))
        {
            File.Delete(portraitpath);
        }
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        Destroy(m_DeleteObj);
        Destroy(gameObject);
    }

    public void BtnNo()
    {
        Destroy(gameObject);
    }
}
