using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;
using System;

namespace TowerDefense
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Image ugradeIcon;
        private int costNumber = 0;
        [SerializeField] private Text level, costText;
        [SerializeField] private Button buyButton;

        public void Initialize()
        {
            ugradeIcon.sprite = asset.sprite;
            var savedLevel = Upgrades.GetUpgradeLevel(asset);
           
            if (savedLevel >= asset.costByLevel.Length)
            {
                level.text = $"Lvl: {savedLevel} (Max)";
                buyButton.interactable = false;
                buyButton.transform.Find("Image").gameObject.SetActive(false);
                buyButton.transform.Find("Text").gameObject.SetActive(false);
                costText.text = "X";
                costNumber = int.MaxValue;
            }
            else
            {
                level.text = $"Lvl: {savedLevel + 1}";
                costNumber = asset.costByLevel[savedLevel];
                costText.text = costNumber.ToString();
            }
        }

        public void Buy()
        {
            if (asset == null)
            {
                Debug.LogError("Buy() called, but asset is null!");
                return;
            }

            Debug.Log($"Buy() called for asset: {asset.name}, isEgyptTowerUpgrade: {asset.isEgyptTowerUpgrade}");

            if (asset.isEgyptTowerUpgrade)
            {
                Egypt_Tower_Upgrade.UpgradeActivated = true;// игрок активировал апгрейд
                Debug.Log("Egypt_Tower_Upgrade.UpgradeActivated set to TRUE");
            }

            // Проверим, дойдёт ли вызов до BuyUpgrade
            Debug.Log("Calling Upgrades.BuyUpgrade...");
            Upgrades.BuyUpgrade(asset);
            Debug.Log("Upgrades.BuyUpgrade finished");

            // Проверим, дойдёт ли вызов до Initialize
            Debug.Log("Calling Initialize...");
            Initialize();
            Debug.Log("Initialize finished");
        }

        internal void CheckCost(int money)
        {
            buyButton.interactable = money >= costNumber;
        }
    }
}

