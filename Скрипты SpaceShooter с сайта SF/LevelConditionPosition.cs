using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Условие появления игрока внутри какой то круговой зоны.
    /// Срабатывает один раз.
    /// </summary>
    public sealed class LevelConditionPosition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private CircleArea m_Area;

        private bool m_Reached;

        bool ILevelCondition.IsCompleted
        {
            get
            {
                if(Player.Instance != null && Player.Instance.ActiveShip != null)
                {
                    if(m_Area.IsInside(Player.Instance.ActiveShip.transform.position))
                    {
                        m_Reached = true;
                    }
                }

                return m_Reached;
            }
        }
    }
}