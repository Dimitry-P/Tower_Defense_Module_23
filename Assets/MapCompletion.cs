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
        [SerializeField] private Episode[] allEpisodes;  // Сюда в инспекторе перетащить все Episode

        [Serializable]
        public class EpisodeScore
        {
            public Episode episode;
            public int score;
        }

        [SerializeField] private CompletionData data;
        public CompletionData Data => data;

        [Serializable]
        public class CompletionData
        {
            public EpisodeScore[] episodes;
            public int unlockedLevels = 0;
            public int numLivesTotal;
        }

        public int UnlockedLevels => data.unlockedLevels;

        public static void SaveEpisodeResult(int levelScore, int numLives)
        {
            if (Instance)
            {
                Instance.SaveResult(LevelSequenceController.Instance.CurrentEpisode, levelScore, numLives);
            }
            else
            {
                Debug.Log($"Episode complete with score {levelScore}");
            }
        }

        private void SaveResult(Episode currentEpisode, int levelScore, int numLives)
        {
            foreach (var item in data.episodes)
            {
                if (item.episode == currentEpisode)
                {
                    if (levelScore > item.score)
                    {
                        totalScore += levelScore - item.score;
                        item.score = levelScore;
                        data.unlockedLevels++;
                        Debug.Log("Saving lives = " + numLives);
                        data.numLivesTotal = numLives;
                        Saver<CompletionData>.Save(filename, data);
                    } 
                }
            }
        }

        
        private int totalScore;
        public int TotalScore { get { return totalScore; } }


        //private new void Awake()
        //{
        //    base.Awake();
        //    Saver<CompletionData>.TryLoad(filename, ref data);  //Обращаемся к какому-то классу и дёргаем на нём 
        //    //статичную функцию TryLoad, которая должна из конкретного файла с конкретным именем загрузить данные completionData
        //    //при этом мы completionData передаём как ref для того чтобы в случае если нам удаётся успешно загрузить, у нас эта
        //    //completionData загрузилась. А если не удаётся, то осталась бы такой, какой она была изначально.
        //    foreach (var episodeScore in data.episodes)
        //    {
        //        totalScore += episodeScore.score;
        //    }
        //}

        private new void Awake()
        {
            base.Awake();

            Saver<CompletionData>.TryLoad(filename, ref data);

            if (data == null)
            {
                data = new CompletionData();

                data.episodes = new EpisodeScore[allEpisodes.Length];
                for (int i = 0; i < allEpisodes.Length; i++)
                {
                    data.episodes[i] = new EpisodeScore
                    {
                        episode = allEpisodes[i],
                        score = 0
                    };
                }
            }

            foreach (var episodeScore in data.episodes)
            {
                totalScore += episodeScore.score;
            }
        }

        private void Start()   // именно Start, а не Awake!
        {
            Saver<CompletionData>.TryLoad(filename, ref data);

            if (data == null)
            {
                data = new CompletionData();
                // ... инициализация эпизодов ...
                Player.Instance.NumLives = data.numLivesTotal;   // важно задать начальное
            }

            // Теперь безопасно применяем к Player
            if (Player.Instance != null)
            {
                Player.Instance.ApplyPlayerUpgrades();
            }

            // пересчёт totalScore и т.д.
        }


        public int GetEpisodeScore(Episode m_episode)
        {
            foreach (var data in data.episodes)
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

