using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Класс подбираемой паверапки для изменения хитпоинтов патронов и энергии.
    /// </summary>
    public class PowerupStats : Powerup
    {
        public enum EffectType
        {
            AddHitpoints,
            AddAmmo,
            AddEnergy
        }

        /// <summary>
        /// Тип эффекта.
        /// </summary>
        [SerializeField] private EffectType m_EffectType;

        /// <summary>
        /// Кол-во в зависимости от типа.
        /// </summary>
        [SerializeField] private float m_Value;

        protected override void OnPickedUp(SpaceShip playerShip)
        {
            switch (m_EffectType)
            {
                case EffectType.AddHitpoints:
                    playerShip.AddHitPoints(m_Value);
                    break;
                case EffectType.AddAmmo:
                    playerShip.AddAmmo((int)m_Value);
                    break;
                case EffectType.AddEnergy:
                    playerShip.AddEnergy((int)m_Value);
                    break;
            }

        }
    }
}