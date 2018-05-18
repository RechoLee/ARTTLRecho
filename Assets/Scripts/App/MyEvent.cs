using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 自定义Unity事件类 用于传值
/// </summary>
public class MyEvent:EventTrigger
{
    public Action<GameObject> DoAction;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if(DoAction!=null)
            DoAction(this.gameObject);
    }
}
