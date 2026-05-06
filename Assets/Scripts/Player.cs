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
        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        protected override void Awake()
        {
            base.Awake();

            if (m_NumLives <= 0)
                m_NumLives = baseNumLives; // без события
        }

        private void Start()
        {
            if (m_Ship) SubscribeToShip(m_Ship);
        }

        private void OnEnable()
        {
            // Сразу прмиенить апргрейды
            NumLives += HealthUpgradeBonusSaver.bonus;
        }

        //public void ApplyPlayerUpgrades()
        //{
        //    if (Upgrades.Instance == null || Upgrades.Instance.save == null) return; // Сброс к базовому значению (важно!) 
        //    //NumLives = baseNumLives; 
        //    // должно быть какое-то базовое значение
        //    foreach (var savedUpgrade in Upgrades.Instance.save) 
        //    {
        //        if (savedUpgrade.upgradeSO != null) 
        //        {
        //            savedUpgrade.upgradeSO.ApplyPlayer(savedUpgrade.level);
        //        } 
        //    } 
        //}

        private void SubscribeToShip(SpaceShip ship)
        {
            ship.EventOnDeath.RemoveListener(OnShipDeath);
            ship.EventOnDeath.AddListener(OnShipDeath);
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(m_PlayerShipPrefab.gameObject);
            m_Ship = newPlayerShip.GetComponent<SpaceShip>();
            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }

       




        [SerializeField] private GameObject loosePanel;
        public GameObject victoryPanel;
        [SerializeField] protected Tower towerPrefab;
        public SpaceShip ActiveShip => m_Ship;

        //public void UpdateNumLivesInBranchLevels()
        //{
        //    if (MapCompletion.Instance != null && MapCompletion.Instance.Data != null)
        //    {
        //        NumLives = MapCompletion.Instance.Data.numLivesTotal;
        //    }
        //}

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (m_Ship != null)
                m_Ship.EventOnDeath.RemoveListener(OnShipDeath);
        }

        #region Score (current level only)
        public int Score { get; private set; }

        public void AddScore(int num)
        {
            Score += num;
        }

        protected void TakeDamage(int numLives_damage)
        {
            NumLives -= numLives_damage;
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
