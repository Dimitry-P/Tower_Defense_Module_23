using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{

    /// <summary>
    /// Данный скрипт будет повреждать Destructible к которому прицеплен.
    /// </summary>
    public sealed class CollisionDamageApplicator : MonoBehaviour
    {
        /// <summary>
        /// Коллайдеры с этим тегом будет игнорироваться.
        /// Удобно для определения стен или границ об которые не хочется убиваться.
        /// </summary>
        public static string IgnoreTag = "WorldBoundary";

        /// <summary>
        /// Повреждения кратно относительной скорости столкновения.
        /// </summary>
        [SerializeField] private float m_VelocityDamageModifier;

        /// <summary>
        /// Фиксированное кол-во повреждения которое будет применено всегда на столкновении.
        /// </summary>
        [SerializeField] private float m_DamageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == IgnoreTag)
                return;

            var destructible = transform.root.GetComponent<Destructible>();

            if (destructible != null)
            {
                destructible.ApplyDamage((int)m_DamageConstant + (int)(m_VelocityDamageModifier * collision.relativeVelocity.magnitude));
            }
        }
    }
}
