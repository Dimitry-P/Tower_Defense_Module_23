using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Ограничитель позиции. Работает в связке со скриптом LevelBoundary если таковой имеется на сцене.
    /// Кидается на объект который надо ограничить.
    /// </summary>
    public class LevelBoundaryLimiter : MonoBehaviour
    {
        private void Update()
        {
            if(LevelBoundary.Instance != null)
            {
                var lb = LevelBoundary.Instance;
                var radius = lb.Radius;

                if(transform.position.magnitude > radius)
                {
                    if(lb.LimitMode == LevelBoundary.Mode.Limit)
                        transform.position = transform.position.normalized * radius;
                    else if (lb.LimitMode == LevelBoundary.Mode.Teleport)
                        transform.position = -transform.position.normalized * radius;
                }
            }
        }
    }
}