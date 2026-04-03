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

            BuildSite[] allBuildSites = FindObjectsOfType<BuildSite>();
            if (allBuildSites.Length == 0) return;

            int ranNum = Random.Range(0, allBuildSites.Length);
            BuildSite exactBuildSite = allBuildSites[ranNum];

            // Создать египетскую башню на месте старой
            currentTower = Instantiate(
                egyptianTowerPrefab,
                exactBuildSite.transform.position,
                exactBuildSite.transform.rotation
            );

            // Вставить в иерархию старой башни
            //currentTower.transform.parent = exactBuildSite.transform.parent;

            // Установить уровень апгрейда (заряды)
            currentTower.SetUpgradeLevel(currentUpgradeLevel);

            //exactBuildSite.gameObject.SetActive(false);
            //exactBuildSite.gameObject.transform.localScale = Vector3.zero;

            //var sr = exactBuildSite.GetComponentInChildren<SpriteRenderer>();
            //if (sr != null)Destroy(sr.gameObject);
           
            Destroy(exactBuildSite.transform.root.gameObject);
            Destroy(exactBuildSite.transform.parent.gameObject);
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