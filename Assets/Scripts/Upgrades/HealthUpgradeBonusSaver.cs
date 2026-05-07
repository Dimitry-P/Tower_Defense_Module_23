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
        public void ShowHowMuchIsBonus()
        {
            Debug.Log("555" + bonus);
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
