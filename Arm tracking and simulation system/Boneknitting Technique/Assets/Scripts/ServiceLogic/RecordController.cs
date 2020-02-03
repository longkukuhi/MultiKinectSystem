using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordController
{
    private DeviceCtrl m_DeviceController;
    private PlayController m_PlayController;
    private ModelCtrlData CurrentFrameModelCtrlData;

    public RecordController(GameObject model)
    {
        Init(model);
    }

    public bool Init(GameObject model)
    {
        m_DeviceController = new DeviceCtrl();

        m_PlayController = new PlayController(model);
        return true;
    }

    // Update is called once per frame
    public void Update()
    {
        CurrentFrameModelCtrlData = m_DeviceController.AcquireData();
        m_PlayController.Update(CurrentFrameModelCtrlData);
    }

    public bool InitDevice()
    {
        return m_DeviceController.InitDevice();
    }

    public void DisconnectDevice()
    {
        m_DeviceController.DisconnectDevice();
    }

    public PlayController GetPlayController()
    {
        return m_PlayController;
    }

    public ModelCtrlData GetCurrentData()
    {
        return CurrentFrameModelCtrlData;
    }
}
