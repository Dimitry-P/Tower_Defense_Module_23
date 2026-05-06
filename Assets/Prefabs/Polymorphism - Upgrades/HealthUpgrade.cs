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
        private int lastAppliedLevel = 0;

        public override void ApplyPlayer(int level)
        {
            if (level <= lastAppliedLevel) return;

            int delta = 0;

            for (int i = lastAppliedLevel + 1; i <= level; i++)
            {
                delta += multipliersByLevel[Mathf.Clamp(i - 1, 0, multipliersByLevel.Length - 1)];
            }

            lastAppliedLevel = level;


            HealthUpgradeBonusSaver.bonus += delta;
            Debug.Log("devil " + delta);
            Debug.Log("devil " + lastAppliedLevel);
            HealthUpgradeBonusSaver.Instance.ShowHowMuchIsBonus();
            delta = 0;
        }
    }
}
