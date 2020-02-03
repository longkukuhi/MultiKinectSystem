using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleEndSlider : MonoBehaviour
{
    public RectTransform m_LeftHandle;
    public RectTransform m_RightHandle;
    public RectTransform m_HandleArea;

    public float m_LeftValue;
    public float m_RightValue;

    public delegate void ValueChange(float value,bool bIsLeft );
    public event ValueChange eValueChange;
    // Use this for initialization
    void Start()
    {
        m_LeftValue = 0.0f;
        m_RightValue = 1.0f;

        eValueChange += DoubleEndSlider_eValueChange;

        var LeEventTrigger= m_LeftHandle.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry LeEntry = new EventTrigger.Entry();
        LeEntry.eventID = EventTriggerType.Drag;
        LeEntry.callback.AddListener((data) => { OnLeDragDelegate((PointerEventData)data); });
         LeEventTrigger.triggers.Add(LeEntry);

        var RhEventTrigger = m_RightHandle.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry RhEntry = new EventTrigger.Entry();
        RhEntry.eventID = EventTriggerType.Drag;
        RhEntry.callback.AddListener((data) => { OnRhDragDelegate((PointerEventData)data); });
        RhEventTrigger.triggers.Add(RhEntry);
    }

    private void DoubleEndSlider_eValueChange(float value,bool bIsLeft)
    {
        ;
    }

    private void OnLeDragDelegate(PointerEventData data)
    {
        m_LeftHandle.localPosition += new Vector3(data.delta.x, 0, 0);
        //限制不能左右滑块交错
        if (m_LeftHandle.localPosition.x-m_RightHandle.localPosition.x>-m_LeftHandle.rect.width)
        {
            m_LeftHandle.localPosition = new Vector3(m_RightHandle.localPosition.x - m_LeftHandle.rect.width, 0, 0);
        }
        //限制不能超出边界
        if(m_LeftHandle.localPosition.x< m_LeftHandle.rect.width / 2 - m_HandleArea.rect.width / 2)
        {
            m_LeftHandle.localPosition =new Vector3( m_LeftHandle.rect.width / 2 - m_HandleArea.rect.width / 2,0,0);
        }
        m_LeftValue= (m_LeftHandle.localPosition.x - (m_LeftHandle.rect.width / 2 - m_HandleArea.rect.width / 2)) / ( m_HandleArea.rect.width- m_LeftHandle.rect.width );
        eValueChange(m_LeftValue,true);
    }

    private void OnRhDragDelegate(PointerEventData data)
    {
        m_RightHandle.localPosition += new Vector3(data.delta.x, 0, 0);
        //限制不能左右滑块交错
        if (m_LeftHandle.localPosition.x - m_RightHandle.localPosition.x > -m_LeftHandle.rect.width)
        {
            m_RightHandle.localPosition= new Vector3( m_LeftHandle.localPosition.x + m_LeftHandle.rect.width, 0, 0);
        }
        //限制不能超出边界
        if(m_RightHandle.localPosition.x> m_HandleArea.rect.width / 2-m_RightHandle.rect.width/2)
        {
            m_RightHandle.localPosition=new Vector3( m_HandleArea.rect.width / 2 - m_RightHandle.rect.width / 2,0,0);
        }
        m_RightValue= (m_RightHandle.localPosition.x - (m_LeftHandle.rect.width / 2 - m_HandleArea.rect.width / 2)) / (m_HandleArea.rect.width - m_RightHandle.rect.width);
        eValueChange(m_RightValue,false);
    }
}
