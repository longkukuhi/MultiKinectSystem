using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        ConfigCenter.Instance().ConfigDataInit(DataPath.strConfigFilePath);
        Application.targetFrameRate = ConfigCenter.Instance().GetFPS();
    }


}
