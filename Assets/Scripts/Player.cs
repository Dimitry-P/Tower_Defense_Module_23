using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Класс сущности игрока. Содержит в себе все что связано с игроком.
    /// </summary>
    public class Player : MonoSingleton<Player>
    {

        [SerializeField] private int m_NumLives;
        [SerializeField] private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;

        private void Start()
        {
            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
                Respawn();
            else
                LevelSequenceController.Instance.FinishCurrentLevel(false);
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(m_PlayerShipPrefab.gameObject);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            //m_CameraController.SetTarget(m_Ship.transform);
            //m_MovementController.SetTargetShip(m_Ship);

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

        #endregion
    }
}