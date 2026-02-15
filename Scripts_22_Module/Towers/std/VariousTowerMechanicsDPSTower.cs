using SpaceShooter;
using System;
using TowerDefense;
using Towers;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

namespace Towers.std
{
    public class VariousTowerMechanicsDPSTower : VariousMech
    {
        private int baseDamage;
        private int damage;
       
        private void OnEnable()
        {
            DPSGlobalManager.OnKillsChanged += IncreaseDamage;
        }

        private void OnDisable()
        {
            DPSGlobalManager.OnKillsChanged -= IncreaseDamage;
        }

        public override void TryApplyDamage(Destructible destructible)
        {
            if (destructible.IsBoss == true) return;
            if (destructible == null) return;

            if (tower == null) return;

            destructible.ApplyDamage(damage, this);
        }

        public override void IncreaseDamage(int CurrentUpgrade)
        {
             damage =
     baseDamage += DPSGlobalManager.CurrentUpgrade * 10;
        }

        public override void OnEnemyKilled()
        {
            TDPlayer.Instance.ChangeKilledCount(DPSGlobalManager.KillsByDPSTowers);
            base.OnEnemyKilled();
        }

        public override void UseSpecificMechanic(TurretProperties turretProperties)
        {
            baseDamage = turretProperties.Damage;
            damage = baseDamage + DPSGlobalManager.CurrentUpgrade * 10;
        }
    }
}