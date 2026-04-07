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
        [SerializeField] private Sprite m_DefaultSprite;

        [SerializeField] private Button m_Button;
        [SerializeField] private Text m_text;
        [SerializeField] private Transform buildSite;
        private Image m_Image;

        private void Awake()
        {
            m_Button = GetComponentInChildren<Button>();
            m_Image = m_Button.GetComponentInChildren<Image>();
        }

        public void SetTowerAsset(TowerAsset asset)
        {
            m_TowerAsset = asset;
            m_Button.interactable = true; // можно нажимать
            m_Image.sprite = asset.GUISprite;
        }

        public void SetBuildSite(Transform value)
        {
            buildSite = value;
        }

        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck);
            m_text.text = m_TowerAsset.gold.ToString();
            m_Button.interactable = false;
        }
        private void GoldStatusCheck(int gold)
        {
            if (gold >= m_TowerAsset.gold != m_Button.interactable)
            {
                m_Button.interactable = !m_Button.interactable;
                m_text.color = m_Button.interactable ? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TowerAsset, buildSite);
            BuildSite.HideControls();
        }
    }
}

