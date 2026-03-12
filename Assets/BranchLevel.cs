using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using System;
using UnityEngine.UI;

namespace TowerDefense
{
    [RequireComponent(typeof(MapLevel))]
    public class BranchLevel : MonoBehaviour
    {
        [SerializeField] private Text pointText;
        [SerializeField] private MapLevel rootLevel;
        [SerializeField] private int needPoints = 3;


        internal void TryActivate()
        {
            gameObject.SetActive(rootLevel.IsComplete);
            if (needPoints > MapCompletion.Instance.TotalScore) 
            {
                pointText.text = needPoints.ToString();
            }
            else
            {
                pointText.transform.parent.gameObject.SetActive(false);
                GetComponent<MapLevel>().Initialise();
            }
        }
    }
}

