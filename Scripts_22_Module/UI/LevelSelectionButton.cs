using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefense
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [SerializeField] private LevelProperties m_LevelProperties;

        private void Start()
        {
            if (m_LevelProperties == null) return;
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }
    }
}

