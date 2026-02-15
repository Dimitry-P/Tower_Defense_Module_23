using SpaceShooter;
using System;
using TowerDefense;
using UnityEngine;
using UnityEngine.XR;

namespace SpaceShooter
{
    public abstract class Spawner : MonoBehaviour
    {
        protected abstract GameObject GenerateSpawnedEntity();

        /// <summary>
        /// Зона спавна.
        /// </summary>
        [SerializeField] private CircleArea m_Area;

        /// <summary>
        /// Режим спавна.
        /// </summary>
        public enum SpawnMode
        {
            /// <summary>
            /// На методе Start()
            /// </summary>
            Start,

            /// <summary>
            /// Периодически используя время m_RespawnTime
            /// </summary>
            Loop
        }

        [SerializeField] private SpawnMode m_SpawnMode;

        /// <summary>
        /// Кол-во объектов которые будут разом заспавнены.
        /// </summary>
        [SerializeField] private int m_NumSpawns;

        /// <summary>
        /// Время респавна. Здесь важно заметить что респавн таймер должен быть больше времени жизни объектов.
        /// </summary>
        [SerializeField] private float m_RespawnTime;

        public float m_Timer;

        private float bossTimer = 25f;

        private bool bossSpawned = false;

        //private void Start()
        //{
        //    if(m_SpawnMode == SpawnMode.Start)
        //    {
        //        SpawnEntities();
        //    }
        //}

        //private void Update()
        //{
        //    if (m_Timer > 0)
        //        m_Timer -= Time.deltaTime;

        //    if(m_SpawnMode == SpawnMode.Loop && m_Timer <= 0)
        //    {
        //        SpawnEntities();
        //        m_Timer = m_RespawnTime;
        //    }
        //}

        private void Update()
        {
            // обычные враги
            if (m_SpawnMode == SpawnMode.Loop)
            {
                m_Timer -= Time.deltaTime;

                if (m_Timer <= 0f)
                {
                    SpawnEntities();
                    m_Timer = m_RespawnTime;
                }
            }

            // босс
            if (m_SpawnMode == SpawnMode.Start && !bossSpawned)
            {
                bossTimer -= Time.deltaTime;

                if (bossTimer <= 0f)
                {
                    SpawnBoss();
                    bossSpawned = true;
                }
            }
        }

        private void SpawnBoss()
        {
            // Твой код создания босса
            SpawnEntities();
            Debug.Log("BOSS SPAWNED");
        }


        private void SpawnEntities()
        {
            for(int i = 0; i < m_NumSpawns; i++)
            {
                var e = GenerateSpawnedEntity();
                e.transform.position = m_Area.RandomInsideZone;
            }
        }
    }
}