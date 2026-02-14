using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceShooter
{
    /// <summary>
    /// настройка ограничения мира по радиусу от начала глобальных координат.
    /// </summary>
    public class LevelBoundary : MonoSingleton<LevelBoundary>
    { 
        /// <summary>
        /// Радиус мира от нуля.
        /// </summary>
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        /// <summary>
        /// Режим ограничения.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Простое ограничение по дистанции от нуля.
            /// </summary>
            Limit,

            /// <summary>
            /// телепортируем на противоположный край
            /// </summary>
            Teleport
        }

        /// <summary>
        /// Актуальный режим ограничения.
        /// </summary>
        [SerializeField] private Mode m_LimitMode;
        public Mode LimitMode => m_LimitMode;

        /// <summary>
        /// Отрисовка хендлами едитора. Чисто для визуализации границы.
        /// </summary>
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, m_Radius);
        }
#endif
    }
}