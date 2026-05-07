using System;
using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefense
{
    [CreateAssetMenu(menuName = "Upgrades/Tower/Radius Upgrade")]
    public class RadiusUpgrade : TowerUpgrade
    {
        [SerializeField] private float[] multipliersByLevel = { 2f, 3f, 4f, 5f, 6f, 7f };

        public override void Apply(Tower tower, int level)
        {
            if (level < 1 || level >= multipliersByLevel.Length) return;

            // Можно сделать публичный метод в Tower, чтобы не лезть в private поле
            RadiusUpgradeBonusSaver.radiusUpgradeBonus = multipliersByLevel[level - 1];
        }
    }
}
