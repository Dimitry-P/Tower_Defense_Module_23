using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

//Ведь по сути что у меня происходит в коде: когда игрок кликает мышкой по сцене,
//вызывается метод TryBuild, который создает башню (для башни есть префаб),
//а в скрипте башни в Awake уже имеется массив public Turret[] turrets,
//который фактически состоит из одного элемента Turret.
//При создании башни мы здесь же в скрипте Tower проходимся по массиву turrets,
//для элемента которого вызываем метод из скрипта Turret,
//и в этом методе я задаю для переменной _variousMechType тип конкретной башни (по enum),
//и дальше уже при стрельбе этот тип башни (перем. _variousMechType) передаётся в скрипт Projectile. 

namespace TowerDefense
{
    public class TowerBuyControl : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_TowerAsset;

        public void SetTowerAsset(TowerAsset asset)
        {
            m_TowerAsset = asset;
            var button = GetComponentInChildren<Button>();
            var image = button.GetComponentInChildren<Image>();
            image.sprite = asset.GUISprite;
            Debug.Log(asset.GUISprite+"$$$$$$$$");
        }

        //m_TowerAsset = asset;
        //var image = m_button.GetComponentInChildren<Image>();
        //image.sprite = m_TowerAsset.GUISprite;

        [SerializeField] private Text m_text;
        [SerializeField] private Button m_button;
        [SerializeField] private Transform buildSite;

        public void SetBuildSite(Transform value)
        {
            buildSite = value;
        }

        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck);
            m_text.text = m_TowerAsset.gold.ToString();
            m_button.transform.GetChild(1).GetComponent<Image>().sprite = m_TowerAsset.GUISprite;
        }
        private void GoldStatusCheck(int gold)
        {
            if (gold >= m_TowerAsset.gold != m_button.interactable)
            {
                m_button.interactable = !m_button.interactable;
                m_text.color = m_button.interactable ? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TowerAsset, buildSite);
            BuildSite.HideControls();
        }
    }
}

