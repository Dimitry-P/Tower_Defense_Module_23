using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using SpaceShooter;

namespace TowerDefense
{
    public class BuildSite : MonoBehaviour, IPointerDownHandler
    {
        public TowerAsset[] buildableTowers;
        public void SetBuildableTowers(TowerAsset[] towers) 
        { 
            if(towers == null || towers.Length == 0)
            {
                Destroy(transform.parent.gameObject);
                //gameObject.SetActive(false);
            }
            else
            {
                buildableTowers = towers;
            }
        }
        public static event Action<BuildSite> OnClickEvent;
        public static void HideControls()
        {
            OnClickEvent(null);
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            print($"нажато {transform.root.name}");
                OnClickEvent(this);
                Debug.Log(transform.root);
        }
    }
}


