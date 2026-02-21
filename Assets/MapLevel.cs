using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

//Этот скрипт загружает следующий уровень, по которому я кликнул

namespace TowerDefense
{
    public class MapLevel : MonoBehaviour
    {
        private Episode m_episode;
        [SerializeField] private Text text;
        private int totalCount = 0;
        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_episode);
        }
        public void SetLevelData(Episode episode, int score)
        {
            m_episode = episode;
            text.text = $"{score}/3";
        }
    }
}
