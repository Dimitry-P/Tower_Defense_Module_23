using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefense
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public static event Action<Enemy> OnEnemySpawn;
        [SerializeField] private Path[] paths;
        [SerializeField] private EnemyWave currentWave;
        [SerializeField] private Enemy m_EnemyPrefab;
        private int activeEnemyCount = 0;
        public event Action OnAllWavesDead;
        private void RecordEnemyDead()
        {
            if(--activeEnemyCount == 0)
            {
                 ForceNextWave();
            }
        }

        private void Start()
        {
            currentWave.Prepare(SpawnEnemies);  //Текущая волна - готовься! 
            //Т.о. мы избавляемся от необходимости разбираться с ПОДПИСКАМИ
            //внутри EnemyWaveManager. А дальше мы с подписками можем 
            // разбираться внутри EnemyWave.
        }

        private void SpawnEnemies()
        {
            // создать врагов
            foreach ((EnemyAsset asset, int count, int PathIndex) in currentWave.EnumerateSquads())
            {
                if(PathIndex < paths.Length)
                {
                    for(int i = 0; i < count; i++)
                    { 
                        var e = Instantiate(m_EnemyPrefab, paths[PathIndex].StartArea.RandomInsideZone, Quaternion.identity);
                        e.OnEnd += RecordEnemyDead;
                        e.Use(asset);
                        e.GetComponent<TDController>().SetPath(paths[PathIndex]);
                        activeEnemyCount += 1;
                        OnEnemySpawn?.Invoke(e);
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid pathIndex in {name}");
                }
            }
            currentWave = currentWave.PrepareNext(SpawnEnemies); //Текущая волна - готовься! 
            // SpawnEnemies -- вот тебе, как ты будешь спавниться.
        }

        public void ForceNextWave()
        {
            if (currentWave)
            {
                TDPlayer.Instance.ChangeGold((int)currentWave.GetRemainingTime());
                //1. Здесь мы хотим принудительно завершить текущую волну
                //2. Затем вызвать следующую волну
                SpawnEnemies();
                //Награда за форс волны
            }
            else
            {
                if(activeEnemyCount == 0)
                {
                    OnAllWavesDead?.Invoke();
                }
            }
        }
    }
}
