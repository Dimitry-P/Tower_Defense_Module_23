using System;
using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefense
{
    [CreateAssetMenu(menuName = "Upgrades/Tower/Health Upgrade")]
    public class HealthUpgrade : TowerUpgrade
    {
        [SerializeField] private int[] multipliersByLevel = { 2, 3, 4, 5, 6, 7 };

        public override void Apply(Tower tower, int level)
        {
            if (level < 1 || level >= multipliersByLevel.Length) return;


            Player.Instance.NumLives += (multipliersByLevel[level - 1]);
        }
    }
}
