using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceShooter;

namespace TowerDefense
{
    public class PauseMenuManager : MonoBehaviour
    {
        public GameObject pauseMenu;

        bool isPaused = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                    isPaused = false;
                }
                else
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    isPaused = true;
                }
            }
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void LoadLevelMap()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("LevelMap");
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
