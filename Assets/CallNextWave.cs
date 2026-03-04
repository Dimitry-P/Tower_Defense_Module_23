using UnityEngine;

namespace TowerDefense
{
    public class CallNextWave : MonoBehaviour
    {
        private EnemyWaveManager manager;
        void Start()
        {
            manager = FindObjectOfType<EnemyWaveManager>();
        }

        public void CallWave()
        {
            manager.ForceNextWave();
        }
    }
}

