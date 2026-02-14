using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Спавнер мусора. Отличается константным респавном в случае уничтожения и заданием начальной скорости.
    /// </summary>
    public class EntitySpawnerDebris : MonoBehaviour
    {
        [SerializeField] private Destructible[] m_DebrisPrefabs;

        [SerializeField] private int m_NumDebris;

        [SerializeField] private CircleArea m_Area;

        [SerializeField] private float m_RandomSpeed;

        private void Start()
        {
            for(int i = 0; i < m_NumDebris; i++)
            {
                SpawnDebris();
            }
        }

        private void SpawnDebris()
        {
            var prefab = m_DebrisPrefabs[UnityEngine.Random.Range(0, m_DebrisPrefabs.Length)];
            var debris = Instantiate(prefab);

            debris.transform.position = m_Area.RandomInsideZone;
            debris.GetComponent<Destructible>().EventOnDeath.AddListener(OnDebrisDead);

            var rigid = debris.GetComponent<Rigidbody2D>();

            if(rigid!= null && m_RandomSpeed > 0)
                rigid.velocity = UnityEngine.Random.insideUnitCircle * m_RandomSpeed;
        }

        private void OnDebrisDead()
        {
            SpawnDebris();
        }
    }
}