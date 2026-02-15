using SpaceShooter;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Towers.std
{
    public class VariousTowerMechanicsSingleTower : VariousMech
    {
        private int baseDamage;

        public override void TryApplyDamage(Destructible destructible)
        {
            if (destructible.IsBoss == true) return;
            if (destructible == null) return;

            destructible.ApplyDamage(baseDamage, this);
        }

        public override void UseSpecificMechanic(TurretProperties turretProperties)
        {
            baseDamage = turretProperties.Damage;
        }
    }
}
