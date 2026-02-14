using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Контроллер выбора корабля игрока. Идея аналогична EpisodeSelectionController.
    /// </summary>
    public class PlayerShipSelectionController : MonoBehaviour
    {
        [SerializeField] private SpaceShip m_Prefab;

        public void OnShipSelected()
        {
            LevelSequenceController.PlayerShipPrefab = m_Prefab;
        }
    }
}