using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Паверап смены оружия. Оружия только два типа и выставляются автматически в зависимости от турели.
    /// </summary>
    public class PowerupWeapon : Powerup
    {
        [SerializeField] private TurretProperties m_Properties;

        protected override void OnPickedUp(SpaceShip playerShip)
        {
            playerShip.AssignWeapon(m_Properties);
        }
    }
}