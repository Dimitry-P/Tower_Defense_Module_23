using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_MainPanel;
        [SerializeField] private GameObject m_LevelSelectionPanel;

        private void Start()
        {
            ShowMainPanel();
        }

        public void ShowMainPanel()
        {
            m_MainPanel.SetActive(true);
            m_LevelSelectionPanel.SetActive(false);
        }

      

        public void EX_ShowLevelSelection()
        {
            m_LevelSelectionPanel.SetActive(true);
            m_MainPanel.SetActive(false);
        }

        public void EX_Quit()
        {
            Application.Quit();
        }
    }
}
