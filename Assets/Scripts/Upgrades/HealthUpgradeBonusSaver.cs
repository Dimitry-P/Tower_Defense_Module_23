using System;
using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefense
{
    public class HealthUpgradeBonusSaver : MonoSingleton<HealthUpgradeBonusSaver>
    {
        public static int bonus = 0;
        protected override void Awake()
        {
            base.Awake(); // ВАЖНО!
        }
        public void SetLastAppliedLevel(int level)
        {
            if (bonus != 0) return;
            else
            {
                HealthUpgrade.lastAppliedLevel = level;
            }
        }
    }
    public class RadiusUpgradeBonusSaver : MonoSingleton<RadiusUpgradeBonusSaver>
    {
        public static float radiusUpgradeBonus = 1f;
       
        protected override void Awake()
        {
            base.Awake(); // ВАЖНО!
        }
    }
}
