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
            public int totalCount;
        }

        public static void SaveEpisodeResult(int levelScore, int totalCount)
        {
            Instance.SaveResult(LevelSequenceController.Instance.CurrentEpisode, levelScore, totalCount);
        }

        private void SaveResult(Episode currentEpisode, int levelScore, int totalCount)
        {
            foreach (var item in completionData)
            {
                if (item.episode == currentEpisode)
                {
                    if (levelScore > item.score)
                    {
                        item.score = levelScore;
                        item.totalCount = totalCount;
                        //TDPlayer.Instance.totalGold = totalCount;
                        Saver<EpisodeScore[]>.Save(filename, completionData);
                        Debug.Log("asdf" + completionData[0].totalCount);
                    } 
                }
            }
        }

        [SerializeField] private EpisodeScore[] completionData;

        private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(filename, ref completionData);  //Обращаемся к какому-то классу и дёргаем на нём 
            //статичную функцию TryLoad, которая должна из конкретного файла с конкретным именем загрузить данные completionData
            //при этом мы completionData передаём как ref для того чтобы в случае если нам удаётся успешно загрузить, у нас эта
            //completionData загрузилась. А если не удаётся, то осталась бы такой, какой она была изначально.
        }

        public bool TryIndex(int id, out Episode episode, out int score)
        {
            if(id >= 0 && id < completionData.Length)
            {
                episode = completionData[id].episode;
                score = completionData[id].score;
                return true;
            }
            else
            {
                episode = null;
                score = 0;
                return false;
            }
        }
    }
}

