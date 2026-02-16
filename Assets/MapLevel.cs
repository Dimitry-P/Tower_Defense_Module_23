using UnityEngine;
using SpaceShooter;

//Этот скрипт загружает следующий уровень, по которому я кликнул

namespace TowerDefense
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Episode episode;
        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(episode);
        }
    }
}
