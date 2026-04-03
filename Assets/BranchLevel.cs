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
        [SerializeField] private Text m_PointText;
        [SerializeField] private MapLevel m_RootLevel;
        [SerializeField] private int m_NeedPoints = 3;


        /// <summary>
        /// Попытка активации ответвлённого уровня.
        /// Активация требует наличия очков и выполнения прошлого уровня.
        /// </summary>
        public void TryActivate()
        {
            gameObject.SetActive(m_RootLevel.IsComplete);
            if (m_NeedPoints > MapCompletion.Instance.TotalScore) 
            {
                m_PointText.text = m_NeedPoints.ToString();
            }
            else
            {
                m_PointText.transform.parent.gameObject.SetActive(false);
                GetComponent<MapLevel>().Initialise();
            }
        }
    }
}

