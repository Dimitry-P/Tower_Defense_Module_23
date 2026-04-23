using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEditor.VersionControl;
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

        [SerializeField] private int m_Cost = 10;
        [SerializeField] private int m_Duration = 5;
        [SerializeField] private float m_Cooldown = 5f;
        [SerializeField] private UnityEngine.UI.Slider m_EnergyBar;


        private void Start()
        {
            bool unlocked = Abilities.Instance.IsUnlocked(abilityAsset);
            m_View.SetActive(unlocked);
            Debug.Log(m_View + " john");
            Debug.Log(unlocked + " john");


            if (m_View != null)
            {
                Debug.Log("john BEFORE activeSelf = " + m_View.activeSelf);
                Debug.Log("john BEFORE activeInHierarchy = " + m_View.activeInHierarchy);

                m_View.SetActive(unlocked);

                Debug.Log("john AFTER activeSelf = " + m_View.activeSelf);
                Debug.Log("john AFTER activeInHierarchy = " + m_View.activeInHierarchy);
            }
        }

      

        private void Update()
        {
            if (m_TimeButton == null) return;
            m_TimeButton.interactable = !Abilities.Instance.IsTimeAbilityOnCooldown &&
     TDPlayer.Instance.Gold >= Abilities.Instance.wwwTimeAbilityGold;

            m_FireButton.interactable = TDPlayer.Instance.Gold >= Abilities.Instance.wwwTimeAbilityGold;

            m_EnergyBar.value = Abilities.Instance.Energy01;
        }

      

        public void OnTimeClicked()
        {
            Debug.Log("Abilities: " + Abilities.Instance);
            Debug.Log("Button: " + m_TimeButton);
            Debug.Log("abilityManager = " + Abilities.Instance.name);
            Abilities.Instance.UseTimeAbility(this, m_TimeButton, 5f, 5f);
        }

        [SerializeField] private FireAbility m_FireAbility;
        public void OnFireButtonClick()
        {
            Debug.Log("Abilities.Instance = " + Abilities.Instance);
            Abilities.Instance.UseFireAbility();
            TDPlayer.Instance.ChangeGold(-Abilities.Instance.FireAbilityGold);
        }
    }
}

