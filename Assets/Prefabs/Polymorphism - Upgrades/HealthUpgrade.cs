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
        //= { 2, 3, 4, 5, 6, 7 }

        public override void ApplyPlayer(Player player, int level)
        {
            if (level > 0) player.NumLives += multipliersByLevel[Mathf.Clamp(level - 1, 0, multipliersByLevel.Length - 1)];
            Debug.Log(multipliersByLevel[Mathf.Clamp(level - 1, 0, multipliersByLevel.Length - 1)] + "444");
            Debug.Log(player.NumLives +"444");
        }
    }
}
