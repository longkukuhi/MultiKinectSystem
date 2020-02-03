using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCtrl :MonoBehaviour
{
    private HandCtrl m_HandController;
    private BodyCtrl m_BodyController;

    private void Start()
    {
        m_HandController = GetComponentInChildren<HandCtrl>();
        m_BodyController = GetComponentInChildren<BodyCtrl>();
    }

    public void Init(GameObject model)
    {

    }

    //接收外部数据，移动模型
    public void MoveModel(ModelCtrlData modelCtrlData)
    {
        m_BodyController.MoveBody(modelCtrlData.bodyCtrlData);
        m_HandController.MoveHand(modelCtrlData.handCtrlData);
    }

}
