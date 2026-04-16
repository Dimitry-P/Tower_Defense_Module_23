using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEditor.VersionControl;
using UnityEngine;

namespace TowerDefense
{
    public class AbilitiesView : MonoBehaviour
    {
        [SerializeField] private GameObject m_View;
        [SerializeField] private UpgradeAsset abilityAsset;
        [SerializeField] private UnityEngine.UI.Image m_TargetingCircle;
        [SerializeField] private UnityEngine.UI.Button m_TimeButton;

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


      
    }
}

