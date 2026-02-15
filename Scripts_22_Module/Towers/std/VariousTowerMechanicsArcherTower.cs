using SpaceShooter;
using TowerDefense;
using UnityEngine;

namespace Towers.std
{
    public class VariousTowerMechanicsArcherTower : VariousMech
    {
        private float radiusOfDamage;
        private int baseDamage;

        public override void TryApplyDamage(Destructible destructible)
        {
            if (destructible.IsBoss == true) return;
            Debug.Log(radiusOfDamage);
            if(destructible != null)
            {
                destructible.ApplyDamage(baseDamage, this);
            }
        }

        public override void UseSpecificMechanic(TurretProperties turretProperties)
        {
            baseDamage = turretProperties.Damage;
            radiusOfDamage = GetComponent<Projectile>()._towerRadius;
        }
    }
}
