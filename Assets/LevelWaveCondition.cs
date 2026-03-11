using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class LevelWaveCondition : MonoBehaviour, ILevelCondition
    {
        private bool isCompleted;
        public bool IsCompleted { get { return isCompleted; } }

        void Start()
        {
            FindObjectOfType<EnemyWaveManager>().OnAllWavesDead += () => 
            { 
                isCompleted = true; 
            };
        }
    }
}

