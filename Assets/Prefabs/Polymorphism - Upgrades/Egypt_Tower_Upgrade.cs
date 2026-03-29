using System;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    [CreateAssetMenu(menuName = "Upgrades/Tower/Egypt Tower Upgrade")]
    public class Egypt_Tower_Upgrade : TowerUpgrade
    {
        [SerializeField] private int[] multipliersByLevel;
        public static bool UpgradeActivated { get; set; } = false;

        public override void Egypt_Tower_Apply(Tower tower, int level)
        {
            if (!UpgradeActivated) return;
            EgyptTower egypt = FindObjectOfType<EgyptTower>();

            if (egypt != null)
            {
                egypt.SetUpgradeLevel(level);
            }
            Debug.Log("Egypt upgrade applied");
            // Передаём команду фабрике на сцене
            EgyptTowerSpawner.Instance.SpawnOrUpgrade(level);
        }
    }
}