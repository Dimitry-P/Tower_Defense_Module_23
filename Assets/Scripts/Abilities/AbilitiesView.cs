using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using static TowerDefense.Abilities;

namespace TowerDefense
{
    public class AbilitiesView : MonoBehaviour
    {
        [SerializeField] private GameObject m_View;
        [SerializeField] private UpgradeAsset abilityAsset;
        [SerializeField] private UnityEngine.UI.Image m_TargetingCircle;
        [SerializeField] private UnityEngine.UI.Button m_TimeButton;
        [SerializeField] private UnityEngine.UI.Button m_FireButton;
        [SerializeField] private UnityEngine.UI.Slider m_EnergyBar;


        private void Start()
        {
            bool unlocked = Abilities.Instance.IsUnlocked(abilityAsset);
            m_View.SetActive(unlocked);

            if (m_View != null)
            {
                m_View.SetActive(unlocked);
            }
        }

      

        private void Update()
        {
            if (m_TimeButton == null) return;
            m_TimeButton.interactable = !Abilities.Instance.IsTimeAbilityOnCooldown &&
            TDPlayer.Instance.Gold >= Abilities.Instance.TimeAbilityGold;

            m_FireButton.interactable = TDPlayer.Instance.Gold >= Abilities.Instance.TimeAbilityGold;

            m_EnergyBar.value = Abilities.Instance.Energy01;
        }

      

        public void OnTimeClicked()
        {
            Debug.Log("Abilities: " + Abilities.Instance);
            Debug.Log("Button: " + m_TimeButton);
            Debug.Log("abilityManager = " + Abilities.Instance.name);
            Abilities.Instance.UseTimeAbility(abilityAsset, this, m_TimeButton, 5f, 5f);
        }

        [SerializeField] private FireAbility m_FireAbility;
        public void OnFireButtonClick()
        {
            Debug.Log("Abilities.Instance = " + Abilities.Instance);
            Abilities.Instance.UseFireAbility(abilityAsset);
            TDPlayer.Instance.ChangeGold(-Abilities.Instance.FireAbilityGold);
        }
    }
}

