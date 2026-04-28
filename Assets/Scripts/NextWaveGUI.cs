using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class NextWaveGUI : MonoBehaviour
    {
        [SerializeField] private Text bonusAmount;
        private EnemyWaveManager manager;
        private float timeToNextWave;
        void Start()
        {
            manager = FindObjectOfType<EnemyWaveManager>();
            EnemyWave.OnWavePrepare += (float time) =>  //Здесь я говорю, что по подготовке волны будет специальная такая ф-ия
            {
                timeToNextWave = time;

            };
        }

        public void CallWave()
        {
            manager.ForceNextWave();
        }
        private void Update()
        {
            var bonus = (int)timeToNextWave;
            if (bonus < 0)bonus = 0;
            bonusAmount.text = bonus.ToString();
            timeToNextWave -= Time.deltaTime;
        }
    }
}

