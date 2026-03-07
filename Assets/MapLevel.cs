using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

//Этот скрипт загружает следующий уровень, по которому я кликнул

namespace TowerDefense
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Episode m_episode;
        [SerializeField] private RectTransform resultPanel;
        [SerializeField] private Image[] resultImages;
        //[SerializeField] private Text text;
        private int totalCount = 0;

        public bool IsComplete { get { return 
                    gameObject.activeSelf && resultPanel.gameObject.activeSelf; } }

        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_episode);
        }

        public void Initialise()
        {
            var score = MapCompletion.Instance.GetEpisodeScore(m_episode);
            resultPanel.gameObject.SetActive(score > 0);
            for (int i = 0; i < score; i++)
            {
                resultImages[i].color = Color.white;
            }
            //text.text = $"{score}/3";
        }
    }
}
