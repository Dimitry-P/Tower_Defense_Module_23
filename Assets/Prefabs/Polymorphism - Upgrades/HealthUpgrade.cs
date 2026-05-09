using System;
using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefense
{
    [CreateAssetMenu(menuName = "Upgrades/Tower/Health Upgrade")]
    public class HealthUpgrade : TowerUpgrade
    {
        [SerializeField] private int[] multipliersByLevel;
        public static int lastAppliedLevel = 0;

        public override void ApplyPlayer(int level)
        {
            if (level <= lastAppliedLevel) return;

            int delta = 0;

            for (int i = lastAppliedLevel + 1; i <= level; i++)
            {
                delta += multipliersByLevel[Mathf.Clamp(i - 1, 0, multipliersByLevel.Length - 1)];
            }

           


            HealthUpgradeBonusSaver.bonus = delta;
            HealthUpgradeBonusSaver.Instance.SetLastAppliedLevel(level);
            delta = 0;
        }
    }
}
