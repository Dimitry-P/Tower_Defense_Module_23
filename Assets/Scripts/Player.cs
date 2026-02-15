using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    /// <summary>
    /// Класс сущности игрока. Содержит в себе все что связано с игроком.
    /// </summary>
    public class Player : MonoSingleton<Player>
    {
        [SerializeField] private int m_NumLives;
        public int NumLives {  get { return m_NumLives; }}
        public event Action OnPlayerDead;

        [SerializeField] private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;
        [SerializeField] private GameObject loosePanel;
        public GameObject victoryPanel;

        private void Start()
        {
            if (m_Ship)
                SubscribeToShip(m_Ship);
        }

        private void SubscribeToShip(SpaceShip ship)
        {
            ship.EventOnDeath.RemoveListener(OnShipDeath);
            ship.EventOnDeath.AddListener(OnShipDeath);
        }

        private void OnDestroy()
        {
            if (m_Ship != null)
                m_Ship.EventOnDeath.RemoveListener(OnShipDeath);
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
                Respawn();
            else
            {
                LevelSequenceController.Instance.FinishCurrentLevel(false);
                //GameReset.ResetStatics();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(m_PlayerShipPrefab.gameObject);
            m_Ship = newPlayerShip.GetComponent<SpaceShip>();
            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }
            
        #region Score (current level only)

        public int Score { get; private set; }

        public int NumKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
        }

        int smallEnemyCounter = 0;
        int middleEnemyCounter = 0;
        int bossEnemyCounter = 0;
        protected void TakeDamage(int m_damage)
        {
            m_NumLives -= m_damage;

            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDead?.Invoke();
            }
        }
        #endregion
    }
}