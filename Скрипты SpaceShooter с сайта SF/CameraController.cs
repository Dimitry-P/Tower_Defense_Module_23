using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{

    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// Камера на сцене которую будет контролировать.
        /// </summary>
        [SerializeField] private Camera m_Camera;

        /// <summary>
        /// Объект за которым следим.
        /// Тут важно показать что это просто трансформа.
        /// За счет этого можно наблюдать за любым объектом на сцене.
        /// Так же можно добавить внутри корабля пустую трансформу смещенную вперед.
        /// За счет этого получится сделать обзор "дальше".
        /// </summary>
        [SerializeField] private Transform m_Target;

        /// <summary>
        /// Линейная интерполяция. Настраиваем по вкусу.
        /// </summary>
        [SerializeField] private float m_InterpolationLinear;

        /// <summary>
        /// Угловая интерполяция.
        /// </summary>
        [SerializeField] private float m_InterpolationAngular;

        /// <summary>
        /// Определяет дистанцию камеры до плоскости 2D. -1 хватит.
        /// </summary>
        [SerializeField] private float m_CameraZOffset;

        /// <summary>
        /// Смещение вперед. Для удобства обзора.
        /// </summary>
        [SerializeField] private float m_ForwardOffset;

        private void Update()
        {
            if (m_Target == null || m_Camera == null)
                return;

            Vector2 camPos = m_Camera.transform.position;
            Vector2 targetPos = m_Target.position + m_Target.up * m_ForwardOffset;
            Vector2 newCamPos = Vector2.Lerp(camPos, targetPos, m_InterpolationLinear * Time.deltaTime);

            m_Camera.transform.position = new Vector3(newCamPos.x, newCamPos.y, m_CameraZOffset);

            if (m_InterpolationAngular > 0)
            {
                m_Camera.transform.rotation = Quaternion.Slerp(m_Camera.transform.rotation, m_Target.rotation, m_InterpolationAngular * Time.deltaTime);
            }
        }

        public void SetTarget(Transform newTarget)
        {
            m_Target = newTarget;
        }
    }
}