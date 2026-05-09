using SpaceShooter;
using System.Collections;
using TowerDefense;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class LevelInitializer : MonoBehaviour
    {
        public static LevelInitializer Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return new WaitUntil(() => Player.Instance != null);
            yield return new WaitUntil(() => MapCompletion.Instance != null);

            InitializeLevel();
        }

        private void InitializeLevel()
        {
            Player.Instance.NumLives += HealthUpgradeBonusSaver.bonus;
            HealthUpgradeBonusSaver.bonus = 0;
       
            Debug.Log("LEVEL INITIALIZED");
        }
    }
}