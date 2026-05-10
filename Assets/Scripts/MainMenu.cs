using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

namespace TowerDefense
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        private void Start()
        {
            continueButton.interactable = FileHandler.HasFile(MapCompletion.filename);
        }
        public void NewGame()
        {
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrades.filename);
            SceneManager.LoadScene(1);
        }

        public void Continue()
        {
            SceneManager.LoadScene(1);
        }
        public void ExitGame()
        {
            Application.Quit();
            Debug.Log("Game exited (works only in build)");
        }
    }
}
