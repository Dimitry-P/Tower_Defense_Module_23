using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class EgyptTowerSpawner : MonoBehaviour
    {
        public static EgyptTowerSpawner Instance;

        [SerializeField] private EgyptTower egyptianTowerPrefab; // prefab египетской башни
        private EgyptTower currentTower; // текущая египетская башня
        private int currentUpgradeLevel = 0; // сохраняем общий уровень апгрейдов

        private void Awake() => Instance = this;

        // Вызываем из SO при покупке апгрейда
        public void SpawnOrUpgrade(int level)
        {
            if (level <= 0) return; // игнорируем чужие апгрейды

            currentUpgradeLevel = level;

            if (currentTower != null)
            {
                currentTower.SetUpgradeLevel(currentUpgradeLevel);
                return;
            }

            // **Если башня ещё не создана, создаём её только после апгрейда**
            if (egyptianTowerPrefab == null) return;

            Tower[] allTowers = FindObjectsOfType<Tower>();
            if (allTowers.Length == 0) return;

            int ranNum = Random.Range(0, allTowers.Length);
            Tower oldTower = allTowers[ranNum];

            // Создать египетскую башню на месте старой
            currentTower = Instantiate(
                egyptianTowerPrefab,
                oldTower.transform.position,
                oldTower.transform.rotation
            );

            // Вставить в иерархию старой башни
            currentTower.transform.parent = oldTower.transform.parent;

            // Установить уровень апгрейда (заряды)
            currentTower.SetUpgradeLevel(currentUpgradeLevel);

            // Удалить старую башню
            Destroy(oldTower.gameObject);
        }

        // Этот метод нужно вызывать в начале каждого уровня
        public void ResetChargesForNewLevel()
        {
            if (currentTower != null)
            {
                currentTower.SetUpgradeLevel(currentUpgradeLevel);
                Debug.Log("Egypt Tower charges reset for new level");
            }
        }
    }
}