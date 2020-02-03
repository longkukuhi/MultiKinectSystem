using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class StudyController
{
    // Update is called once per frame
    abstract public bool Ready();
    abstract public void Update(int nFrameCountRef, int nFrameCount);
    abstract public void Destory();
    abstract public float GetAppraiseResult();
}

internal class StudyControllerFileFile : StudyController
{
    private PlayController m_PlayControllerRef;
    private ModelCtrl m_PlayController;
    private FileReader m_ReaderRef;
    private FileReader m_Reader;
    private TrailCurveAppraiseCtrl m_TrailCurveAppraise;


    public StudyControllerFileFile(GameObject modelRef, GameObject model,string strFileNameRef,string strFileName)
    {
        Init(modelRef, model,strFileNameRef,strFileName);
    }

    bool Init(GameObject modelRef, GameObject model, string strFileNameRef, string strFileName)
    {
        m_PlayControllerRef = new PlayController(modelRef);
        m_PlayController = model.GetComponent<ModelCtrl>();
        m_ReaderRef = new FileReader(strFileNameRef);
        m_Reader = new FileReader(strFileName);
        m_TrailCurveAppraise = new TrailCurveAppraiseCtrl();
        return true;
    }

    public override void Update(int nFrameCountRef, int nFrameCount)
    {
        var modelDataRef = m_ReaderRef.PraseDataByFrameCount(nFrameCountRef);
        var modelData = m_Reader.PraseDataByFrameCount(nFrameCount);
        m_PlayControllerRef.Update(modelDataRef);
        m_PlayController.MoveModel(modelData);
        m_TrailCurveAppraise.RecvCompairTrailData(modelDataRef, modelData);
    }

    public override float  GetAppraiseResult() 
    {
        return m_TrailCurveAppraise.AppraiseTrailCurve();
    }

    public override bool Ready()
    {
        return true;
    }

    public override void Destory()
    {
        m_PlayControllerRef.Destory();
    }
}

internal class StudyControllerFileRecord :StudyController
{
    private PlayController m_PlayControllerRef;
    private ModelCtrl m_RecordController;
    private DeviceCtrl m_DeviceController;
    private TrailCurveAppraiseCtrl m_TrailCurveAppraise;
    private FileReader m_ReaderRef;
    public StudyControllerFileRecord(GameObject modelRef, GameObject model, string strFileNameRef)
    {
        Init(modelRef, model,strFileNameRef);
    }

    bool Init(GameObject modelRef, GameObject model, string strFileNameRef)
    {
        m_PlayControllerRef = new PlayController(modelRef);
        m_RecordController = model.GetComponent<ModelCtrl>();
        m_DeviceController = new DeviceCtrl();
        m_TrailCurveAppraise = new TrailCurveAppraiseCtrl();
        m_ReaderRef = new FileReader(strFileNameRef);
        return true;
    }

    public override void Update(int nFrameCountRef, int nFrameCount)
    {
        var modelDataRef = m_ReaderRef.PraseDataByFrameCount(nFrameCountRef);
        var modelData = m_DeviceController.AcquireData();
        m_PlayControllerRef.Update(modelDataRef);
        m_RecordController.MoveModel(modelData);
        m_TrailCurveAppraise.RecvCompairTrailData(modelDataRef, modelData);
    }

    public override float GetAppraiseResult()
    {
        return m_TrailCurveAppraise.AppraiseTrailCurve();
    }

    public override bool Ready()
    {
         return m_DeviceController.InitDevice();
    }

    public override void Destory()
    {
        m_DeviceController.DisconnectDevice();
        m_PlayControllerRef.Destory();
    }
}