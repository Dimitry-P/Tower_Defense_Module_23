using UnityEngine;
using SpaceShooter;


namespace TowerDefense
{
    public class TDLevelController : LevelController
    {
        private int levelScore = 3;
       
        private int totalCount = 0;
       
        protected override void Start()
        {
            base.Start();
            //подписка
            TDPlayer.Instance.OnPlayerDead += OnPlayerDeadHandler;
            m_EventLevelCompleted.AddListener(OnLevelCompleted);
        }

        //сами методы
         private void OnPlayerDeadHandler()
         {
            StopLevelActivity();
            LevelResultController.Instance.Show(false);
         }
        private void OnLevelCompleted()
        {
            StopLevelActivity();

            if (m_ReferenceTime <= Time.time)
            {
                levelScore -= 1;
            }

            print(levelScore);

            totalCount = TDPlayer.Instance.Gold;

            MapCompletion.SaveEpisodeResult(levelScore, Player.Instance.NumLives);
        }

        //отписка 
        protected override void OnDestroy()
        {
            if (TDPlayer.Instance != null)
            {
                m_EventLevelCompleted.RemoveListener(OnLevelCompleted);
                TDPlayer.Instance.OnPlayerDead -= OnPlayerDeadHandler;
            }  
        }

        private void StopLevelActivity()
        {
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.GetComponent<SpaceShip>().enabled = false;
                enemy.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var obj in FindObjectsOfType<T>())
                {
                    obj.enabled = false;
                }
            }
            DisableAll<EnemyWave>();
            DisableAll<Projectile>();
            DisableAll<Tower>();
            DisableAll<NextWaveGUI>();
        }
    }
}
