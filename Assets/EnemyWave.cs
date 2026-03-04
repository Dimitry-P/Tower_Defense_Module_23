using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EnemyWave: MonoBehaviour
    {
        public static Action<float> OnWavePrepare; //Все, кто на этот OnWavePrepare подпишутся, 
        //будут получать инфу о том, сколько времени осталось до следующей волны
        [Serializable]
        private class Squad           //Это наш отряд. В каждой волне может быть много таких отрядов.
        {
            public EnemyAsset asset;
            public int count;
        }
        [Serializable]
        private class PathGroup  //Каждая группа путей будет содержать в себе массив отрядов
            {
                public Squad[] squads;
            }
        [SerializeField] private PathGroup[] groups;


        [SerializeField] private float prepareTime = 10f;
        public float GetRemainingTime()
        {
            return prepareTime - Time.time;
        }

        private void Awake()
        {
            enabled = false; //выключает на этом компоненте выполнение ф-ий Start и Update до тех пор пока он не будет включен.
        }

        private event Action OnWaveReady;

        public void Prepare(Action spawnEnemies)  //включает волну
        {
            OnWavePrepare?.Invoke(prepareTime);
            prepareTime += Time.time;
            enabled = true;
            OnWaveReady += spawnEnemies;
        }

        private void Update()
        {
            if(Time.time >= prepareTime)
            {
                enabled = false;
                OnWaveReady?.Invoke();
            }
        }

        public IEnumerable<(EnemyAsset asset, int count, int PathIndex)> EnumerateSquads() 
                //Здесь я в качестве возвращаемого типа данных указываю определённый интерфейс,
                //который позволяет саму эту функцию использовать с помощью foreach. Этот интерфейс наз-я IEnumerable
                //Этому интерфейсу в треугольных скобках нужно указать ТИП ПЕРЕЧИСЛЯЕМЫХ данных:
                //Например, КОРТЕЖ foreach ((EnemyAsset asset, int count, int PathIndex) in currentWave.EnumerateSquads())
        {
            for(int i = 0; i < groups.Length; i++)  //Здесь мы синхронизируем индексы групп с индексами путей
            {
                foreach(var squad in groups[i].squads)
                {
                    yield return (squad.asset, squad.count, i);
                    //Здесь написано: как именно мы будем эти объекты ПО ОДНОМУ выдавать.
                    //А именно: из наших отрядов вернётся самый первый.
                    //!!!!! yield return - это воз-ть возвращать из функции значения НЕ сразу, а по одному.!!!!!!!!!!!
                }
            }
        }

        [SerializeField] private EnemyWave next;
        public EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies;
            if(next) next.Prepare(spawnEnemies);
            return next;  //возвращаю эту волну следующую для того, чтобы WaveManager уже мог с ней разобраться.
        }
    }
}