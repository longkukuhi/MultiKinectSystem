using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FDTGloveUltraCSharpWrapper;


public class DTGloveManager : MonoBehaviour {
    
    public static DTGloveManager instance;
    public CfdGlove glove=new CfdGlove();
    public static bool caliFileLoaded;
    //public static float[] DTvalues = new float[20];
    
    // Use this for initialization
    public bool InitDevice () {
        //glove = new CfdGlove();
        glove.Open("USB0");
        caliFileLoaded = glove.LoadCalibration("Assets\\Cal\\right.cal");
        return true;
    }
    public void DisconnectDevice() {
        if (glove.IsOpen())
        {
            glove.Close();
        }
        else {
            Debug.Log( "The 5DTGlove has been disconnected!");
        }
    }

   
    public void AcquireHandData(ref HandCtrlData Data)
    {
        glove.GetSensorScaledAll(ref Data.HandData);
    }

    void Start()
    {
        instance = this;
        InitDevice();
    }
}
