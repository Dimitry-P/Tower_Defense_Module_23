using System;
using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefense
{
    [CreateAssetMenu(menuName = "Upgrades/Tower/Mage Upgrade")]
    public class MageUpgrade : TowerUpgrade
    {
        [SerializeField] private int[] multipliersByLevel;
        //= { 2, 3, 4, 5, 6, 7 }

        public override void ApplyPlayer(int level)
        {
           
        }
    }
}
