using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense
{
public class BuildSite : MonoBehaviour, IPointerDownHandler
{
    public static event Action<Transform> OnClickEvent;
    public static void HideControls()
    {
        OnClickEvent(null);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        print($"нажато {transform.root.name}");
        OnClickEvent(transform.root);
    }
}
}
