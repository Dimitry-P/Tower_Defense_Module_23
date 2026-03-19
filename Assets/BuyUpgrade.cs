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
                level.text += "(Max)";
                buyButton.interactable = false;
                buyButton.transform.Find("Image").gameObject.SetActive(false);
                buyButton.transform.Find("Text").gameObject.SetActive(false);
                costText.text = "X";
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
            //var savedLevel = Upgrades.GetUpgradeLevel(asset);
            //if (asset.costByLevel[savedLevel] < MapCompletion.Instance.TotalScore) 
            //{
            //    return;
            //}
            Upgrades.BuyUpgrade(asset);
            Initialize();
        }

        internal void CheckCost(int money)
        {
            buyButton.interactable = money >= costNumber;
        }
    }
}

