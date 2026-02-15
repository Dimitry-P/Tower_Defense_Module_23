using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

namespace TowerDefense
{
    public class TDPlayer : Player
    {
        public static new TDPlayer Instance
        {
            get
            {
                return Player.Instance as TDPlayer;
            }
        }

      

        private static event Action<int> OnGoldUpdate;
        public static void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.m_gold);
        }
        public static void GoldUpdateUnsubscribe(Action<int> act)
        {
            OnGoldUpdate -= act;
        }

        private static event Action<int> OnDPSKilledEnemiesUpdate;
        public static void DPSKilledEnemiesUpdateSubscribe(Action<int> act)
        {
            OnDPSKilledEnemiesUpdate += act;
            act(Instance.m_DPSkilled);
        }
        public static void DPSKilledEnemiesUpdateUnSubscribe(Action<int> act)
        {
            OnDPSKilledEnemiesUpdate -= act;
        }




        private static event Action<int> OnLifeUpdate;
        public static void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Player.Instance.NumLives);
        }
        public static void LifeUpdateUnsubscribe(Action<int> act)
        {
            OnLifeUpdate -= act;
        }




        private static event Action<int> TotalEnemiesKilledUpdate;
        public static void TotalKilledEnemiesUpdateSubscribe(Action<int> act)
        {
            TotalEnemiesKilledUpdate += act;
            act(Instance.m_Totalkilled);
        }
        public static void TotalKilledEnemiesUpdateUnSubscribe(Action<int> act)
        {
            TotalEnemiesKilledUpdate -= act;
        }
        



        [SerializeField] private int m_gold;
        public void ChangeGold(int change)
        {
            m_gold += change;
            OnGoldUpdate?.Invoke(m_gold);
        }

        [SerializeField] private int m_DPSkilled;
        public void ChangeKilledCount(int change)
        {
            m_DPSkilled = change;
            OnDPSKilledEnemiesUpdate?.Invoke(m_DPSkilled);
        }

        public void ReduceLife(int change, string enemyName)
        {
            TakeDamage(change);
            OnLifeUpdate?.Invoke(Player.Instance.NumLives);
        }

        [SerializeField] private int m_Totalkilled;
        public void ChangeTotalKilledCount(int change)
        {
            m_Totalkilled = change;
            TotalEnemiesKilledUpdate?.Invoke(m_Totalkilled);
        }

        // TODO: ����� � �� ��� ������ �� ��������� ����������
        [SerializeField] private Tower m_towerPrefab;
        private string towerName;
        private Tower tower;

       
           
        

        //public void TryBuild(TowerAsset towerAsset, Transform buildSite)
        //{
        //    ChangeGold(-towerAsset.goldCost);
        //    var tower = Instantiate(m_towerPrefab, buildSite.position, Quaternion.identity);
        //    tower.GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.sprite;
        //    tower.Radius = towerAsset.radius;
        //    EVariousMech towerEnum = towerAsset.type;
        //    var towerScript = tower.GetComponent<Tower>();
            
        //    if (towerScript != null)
        //    {
        //        towerScript.InitTurretSpecificSettings(towerEnum, tower.Radius);
        //    }
                
        //    foreach (var turret in tower.GetComponentsInChildren<Turret>())
        //    {
        //        turret.AssignLoadout2(towerAsset);
        //    }
        //    Destroy(buildSite.gameObject);
        //}
    }
}

