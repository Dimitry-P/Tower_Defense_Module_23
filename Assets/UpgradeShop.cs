using System;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefense
{
    public class UpgradeShop : MonoBehaviour
    {

        [SerializeField] private int money;
        [SerializeField] private Text moneyText;
        [SerializeField] private BuyUpgrade[] sales;

        private void Start()
        {
            UpdateMoney();
            foreach (var slot in sales)
            {
                slot.Initialize();
                var button = slot.transform.Find("Button");

                if (button == null)
                {
                    Debug.LogError($"Button not found in {slot.name}");
                    continue;
                }

                var btnComponent = button.GetComponent<Button>();

                if (btnComponent == null)
                {
                    Debug.LogError($"No Button component on {button.name}");
                    continue;
                }
                slot.transform.Find("Button").GetComponent<Button>().onClick.AddListener(UpdateMoney);
            }
        }
        public void UpdateMoney()
        {
            money = MapCompletion.Instance.TotalScore;
            money -= Upgrades.GetTotalCost();
            moneyText.text = money.ToString();
            foreach (var slot in sales)
            {
                slot.CheckCost(money);
            }
        }
    }
}
