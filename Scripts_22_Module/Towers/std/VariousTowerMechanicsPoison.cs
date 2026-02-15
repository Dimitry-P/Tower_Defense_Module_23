using SpaceShooter;
using System.Collections;
using System.Threading;
using TowerDefense;
using UnityEngine;
using UnityEngine.U2D;

namespace Towers.std
{
    public class VariousTowerMechanicsPoisonTower : VariousMech
    {
        private int baseDamage;

        private int enemyLayerMask;

        private void Awake()
        {
            enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        }

      

        public override void TryApplyDamage(Destructible destructible)
        {
            if (destructible.IsBoss == true) return;
            destructible.SetColorTemporary(Color.green, 7f);
            destructible.ApplyPoison(2, 7f, this);
            destructible.IsPoisoned = true; 
        }

        public override void UseSpecificMechanic(TurretProperties turretProperties)
        {
            baseDamage = turretProperties.Damage;
        }
    }
}
