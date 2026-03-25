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

        public override void ApplyPlayer(TDPlayer player, int level)
        {
            if(level > 0 )player.ModifyNumLives(multipliersByLevel[level-1]);
            Debug.Log("ЙФ");
        }
    }
}
