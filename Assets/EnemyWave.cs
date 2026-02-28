using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EnemyWave: MonoBehaviour
    {
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


        [SerializeField] private float prepareTime = 2f;

        private void Awake()
        {
            enabled = false; //выключает на этом компоненте выполнение ф-ий Start и Update до тех пор пока он не будет включен.
        }

        private event Action OnWaveReady;

        public void Prepare(Action spawnEnemies)  //включает волну
        {
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
            yield return (groups[0].squads[0].asset, groups[0].squads[0].count, 0);
        }

        internal EnemyWave PrepareNext(Action spawnEnemies)
        {
            return null;
        }
    }
}