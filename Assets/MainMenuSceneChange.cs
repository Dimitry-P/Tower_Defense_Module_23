using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace TowerDefense
{
    public class MainMenuSceneChange : MonoBehaviour
    {
        public void Change()
        {
            SceneManager.LoadScene(0);
        }
    }
}
