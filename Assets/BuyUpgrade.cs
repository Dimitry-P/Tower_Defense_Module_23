using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefense
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Image ugradeIcon;
        [SerializeField] private Text level, cost;
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
                cost.text = "X";
            }
            else
            {
                level.text = $"Lvl: {savedLevel + 1}";
                cost.text = asset.costByLevel[savedLevel].ToString();
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
    }
}

