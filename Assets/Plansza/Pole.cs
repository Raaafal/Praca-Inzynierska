using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Pole : MonoBehaviour, IPointerClickHandler
{
    public System.Action<int,int,GameObject> callback;
    int x, y;
    public void Init(System.Action<int,int,GameObject> callback,int x,int y)
    {
        this.callback = callback;
        this.y = y;
        this.x = x;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        callback?.Invoke(x,y,gameObject);
    }
}
