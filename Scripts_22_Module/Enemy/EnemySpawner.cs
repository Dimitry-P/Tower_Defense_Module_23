using SpaceShooter;
using TowerDefense;
using Towers.std;
using UnityEngine;

namespace SpaceShooter
{
    public class EnemySpawner : Spawner
    {
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private Path m_path;
        [SerializeField] private EnemyAsset[] m_EnemyAssets;
        public Transform spawnPoint;   // точка появления врагов
        public Transform baseTransform; // база игрока


        protected override GameObject GenerateSpawnedEntity()
        {
            Vector2 offset2D = UnityEngine.Random.insideUnitCircle * 0.9f;
            Vector3 offset = new Vector3(offset2D.x, offset2D.y, 0f);
            var e = Instantiate(m_EnemyPrefab, spawnPoint.position + offset, Quaternion.identity);
            e.GetComponent<SpaceShip>().SetTargetPoint(baseTransform);
            e.Use(m_EnemyAssets[UnityEngine.Random.Range(0, m_EnemyAssets.Length)]);
            e.GetComponent<TDController>().SetPath(m_path);
            return e.gameObject;
        }
    }
}



