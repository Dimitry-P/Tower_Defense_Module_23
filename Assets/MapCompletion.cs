using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Хранит в себе инфо о том, на сколько очков был выполнен каждый из уровней.
//Инфо о завершении карты будет храниться внутри MapCompletion
namespace TowerDefense
{
    public class MapCompletion : MonoSingleton<MapCompletion>
    {
        public const string filename = "completion.dat";

        [Serializable]
        private class EpisodeScore
        {
            public Episode episode;
            public int score;
        }

        public static void SaveEpisodeResult(int levelScore)
        {
            if (Instance)
            {
                Instance.SaveResult(LevelSequenceController.Instance.CurrentEpisode, levelScore);
            }
            else
            {
                Debug.Log($"Episode complete with score {levelScore}");
            }
        }

        private void SaveResult(Episode currentEpisode, int levelScore)
        {
            foreach (var item in completionData)
            {
                if (item.episode == currentEpisode)
                {
                    if (levelScore > item.score)
                    {
                        item.score = levelScore;
                        Saver<EpisodeScore[]>.Save(filename, completionData);
                    } 
                }
            }
        }

        [SerializeField] private EpisodeScore[] completionData;
        private int totalScore;
        public int TotalScore { get { return totalScore; } }

        private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(filename, ref completionData);  //Обращаемся к какому-то классу и дёргаем на нём 
            //статичную функцию TryLoad, которая должна из конкретного файла с конкретным именем загрузить данные completionData
            //при этом мы completionData передаём как ref для того чтобы в случае если нам удаётся успешно загрузить, у нас эта
            //completionData загрузилась. А если не удаётся, то осталась бы такой, какой она была изначально.
            foreach (var episodeScore in completionData)
            {
                totalScore += episodeScore.score;
            }
        }

        public int GetEpisodeScore(Episode m_episode)
        {
            foreach (var data in completionData)
            {
                if(data.episode == m_episode)
                {
                    return data.score;
                }
            }
            return 0;
        }
    }
}

