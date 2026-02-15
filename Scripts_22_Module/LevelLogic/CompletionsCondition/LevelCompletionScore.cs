using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;

namespace TowerDefense
{
    public class LevelCompletionScore : LevelCondition
    {
        [SerializeField] private int m_Score;
        public override bool IsCompleted
        {
            get 
            { 
                if(TDPlayer.Instance.ActiveShip == null) return false; 

                if(TDPlayer.Instance.Score >= m_Score)
                {
                    return true;
                }
                return false;
            }
        }
    }
}

