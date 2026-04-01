using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TowerDefense.TextUpdate;
using static Unity.VisualScripting.Member;

namespace SpaceShooter
{
    /// <summary>
    /// Класс сущности игрока. Содержит в себе все что связано с игроком.
    /// </summary>
    public class Player : MonoSingleton<Player>
    {
        protected virtual void Awake()
        {
            Instance = this;
            // Сначала устанавливаем базовое значение

            Debug.Log("Awake base lives = " + baseNumLives);

            if(m_NumLives <= 0)NumLives = baseNumLives;

            // Затем сразу загружаем из сохранения, если MapCompletion уже готов
            if (MapCompletion.Instance != null && MapCompletion.Instance.Data != null)
            {
                Debug.Log("Load from save = " + MapCompletion.Instance.Data.numLivesTotal);
                NumLives = MapCompletion.Instance.Data.numLivesTotal;
            }
        }

        private int baseNumLives = 10;
      
        private static int m_NumLives;
        public int NumLives 
        {  
            get { return m_NumLives; }
            set
            {
                m_NumLives = Mathf.Max(0, value);
                TDPlayer.RaiseLifeUpdate(m_NumLives); // просто вызываем событие
            }
        }

        public event Action OnPlayerDead;

        [SerializeField] private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;
        [SerializeField] private GameObject loosePanel;
        public GameObject victoryPanel;
        [SerializeField] protected Tower towerPrefab;

        private void Start()
        {
            if (m_Ship)
                SubscribeToShip(m_Ship);
            ApplyPlayerUpgrades();
        }

        public void ApplyPlayerUpgrades()
        {
            if (Upgrades.Instance == null || Upgrades.Instance.save == null) return;

            // Сброс к базовому значению (важно!)
            //NumLives = baseNumLives;   // должно быть какое-то базовое значение

            foreach (var savedUpgrade in Upgrades.Instance.save)
            {
                if (savedUpgrade.upgradeSO != null)
                {
                    savedUpgrade.upgradeSO.ApplyPlayer(this, savedUpgrade.level);
                }
            }
            Debug.Log("ApplyPlayerUpgrades called");
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
            NumLives--;
            Debug.Log(m_NumLives);
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
            NumLives -= m_damage;
            Debug.Log(m_NumLives);
            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDead?.Invoke();
            }
        }
        #endregion
    }
}
