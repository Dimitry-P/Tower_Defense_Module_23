using System;
using System.Diagnostics;
using TowerDefense;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceShooter
{
    /// <summary>
    /// Контроллер переходов между уровнями. Должен быть с пометкой DoNotDetroyOnLoad
    /// И лежать в сцене с главным меню. LevelController дернет завершение уровня.
    /// </summary>
    public class LevelSequenceController : MonoSingleton<LevelSequenceController>
    {
        public static string MainMenuSceneNickname = "LevelMap";

        /// <summary>
        /// Текущий эпизод. Выставляется контроллером выбора эпизода перед началом игры.
        /// </summary>
        public Episode CurrentEpisode { get; private set; }

        /// <summary>
        /// Текущий уровень эпизода. Идшник относительно текущего выставленного эпизода.
        /// </summary>
        public int CurrentLevel { get; private set; }

        [SerializeField] private Episode m_TestEpisode;

        /// <summary>
        /// Метод запуска первого уровня эпизода.
        /// </summary>
        /// <param name="e"></param>
        /// 

        private void Start()
        {
            if (CurrentEpisode == null && m_TestEpisode != null)
            {
                CurrentEpisode = m_TestEpisode;
                CurrentLevel = 0;
            }
            levDisCont = transform.parent
               .Find("Levels")
               .GetComponent<LevelDisplayController>();
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
            if (scene.name == MainMenuSceneNickname)
            {
                var map = FindObjectOfType<TowerDefense.LevelDisplayController>();

                if (map != null)
                {
                    map.RefreshLevels();
                }
            }
        }

        public void StartEpisode(Episode e)
        {
            CurrentEpisode = e;
            CurrentLevel = 0;

            // сбрасываем статы перед началом эпизода.
            LevelResultController.ResetPlayerStats();

            // запускаем первый уровень эпизода.
            SceneManager.LoadScene(e.Levels[CurrentLevel]);
        }

        /// <summary>
        /// Принудительный рестарт уровня.
        /// </summary>
        public void RestartLevel()
        {
            UnityEngine.Debug.Log("Instance: " + Instance);
            UnityEngine.Debug.Log("m_CurrentLevel: " + CurrentLevel);
            UnityEngine.Debug.Log("m_LevelSequence: " + CurrentEpisode);
            SceneManager.LoadScene(MainMenuSceneNickname);
            //SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        /// <summary>
        /// Завершаем уровень. В зависимости от результата будет показано окошко результатов.
        /// </summary>
        /// <param name="success">успешность или поражение</param>


        public void FinishCurrentLevel(bool success)
        {
            print(SceneManager.GetActiveScene().name);
            if (LevelResultController.Instance == null)
            {
                print("LevelResultController not found in scene");
                return;
            }

            LevelResultController.Instance.Show(success);
        }
        //public void FinishCurrentLevel(bool success)
        //{
        //    // после организации переходов
        //    LevelResultController.Instance.Show(success);
        //}

        private LevelDisplayController levDisCont;

        /// <summary>
        /// Запускаем следующий уровень или выходим в главное меню если больше уровней нету.
        /// </summary>
        public void AdvancedLevel()
        {
            CurrentLevel++;

            // конец эпизода вываливаемся в главное меню.
            if (CurrentEpisode.Levels.Length <= CurrentLevel)
            {
                SceneManager.LoadScene(MainMenuSceneNickname);
                //// Немедленно обновляем branchLevels на сцене:
                foreach (var branch in levDisCont.BranchLevels)
                {
                    branch.TryActivate();
                }
            }
            else
            {
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
            }
        }

        #region Ship select

        /// <summary>
        /// Выбранный игроком корабль для прохождения.
        /// </summary>
        public static SpaceShip PlayerShipPrefab { get; set; }

        #endregion
    }
}