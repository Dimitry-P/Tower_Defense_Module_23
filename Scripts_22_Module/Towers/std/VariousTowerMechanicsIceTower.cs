using SpaceShooter;
using UnityEngine;

namespace Towers.std
{
    public class VariousTowerMechanicsIceTower : VariousMech
    {
        private int baseDamage;

        private int enemyLayerMask;

        private float m_Radius;

        private void Awake()
        {
            enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        }

        public override void TryApplyDamage(Destructible destructible)
        {
            if (destructible.IsBoss == true) return;
            var spaceShip = destructible.GetComponent<SpaceShip>();
            if (spaceShip == null) return;
            {
                spaceShip.Freeze(5f);
            }
        }
        public override void UseSpecificMechanic(TurretProperties turretProperties)
        {
            baseDamage = turretProperties.Damage;
            m_Radius = tower.Radius;
        }
    }
}
