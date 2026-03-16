using UnityEngine;
using UnityEngine.UI;

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
            level.text = $"Lvl: {savedLevel + 1}";
            cost.text = asset.costByLevel[savedLevel].ToString();
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(asset);
        }
    }
}

