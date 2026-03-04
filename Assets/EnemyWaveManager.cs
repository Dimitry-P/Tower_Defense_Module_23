using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefense
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private Path[] paths;
        [SerializeField] private EnemyWave currentWave;
        [SerializeField] private Enemy m_EnemyPrefab;

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
                        e.Use(asset);
                        e.GetComponent<TDController>().SetPath(paths[PathIndex]);
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
            TDPlayer.Instance.ChangeGold((int)currentWave.GetRemainingTime());
            //1. Здесь мы хотим принудительно завершить текущую волну
            //2. Затем вызвать следующую волгну
            SpawnEnemies();
            //Награда за форс волны
        }
    }
}
