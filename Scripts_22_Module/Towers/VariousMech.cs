using SpaceShooter;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using Unity.VisualScripting;

namespace Towers.std
{
    public abstract class VariousMech : MonoBehaviour
    {
        public int _ammoUs;
        protected Tower tower;
        protected Turret turret;
        protected Projectile projectile;

        internal virtual void Init(Tower owner)
        {
            tower = owner;
            tower.InitVariousMech(this);
        }

        public virtual void OnEnemyKilled()
        {
            TDPlayer.Instance.ChangeTotalKilledCount(DPSGlobalManager.TotalKilledByAllTowers);
        }

        public abstract void UseSpecificMechanic(TurretProperties turretProperties);


        public abstract void TryApplyDamage(Destructible destructible);


        public virtual bool LoseTargetAfterHit => false;

        //public abstract void TryCreateParticle(Transform target);

        public virtual void IncreaseDamage(int CurrentUpgrade)
        {
            
        }
    }
}
