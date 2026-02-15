using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEditor;
using UnityEngine;

namespace TowerDefense
{
    public class LevelCompletionPosition : LevelCondition
    {
        [SerializeField] private float m_Radius;

        public override bool IsCompleted
        {
            
            get 
            {
                if (TDPlayer.Instance.ActiveShip == null) return false;

                if (Vector3.Distance(TDPlayer.Instance.ActiveShip.transform.position, transform.position ) <= m_Radius)
                {
                    return true;
                }
                return false;
            }
        }
#if UNITY_EDITOR
        private static Color GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Handles.color = GizmoColor;
            Handles.DrawSolidDisc(transform.position, transform.forward, m_Radius);
        }
        //        Этот метод вызывается только в редакторе, когда ты выделяешь объект с этим скриптом.
        //        Рисует плоский зелёный круг в сцене для наглядности.
        //        transform.forward — нормаль к плоскости круга (в 2D обычно это z-ось).
#endif
    }
}

