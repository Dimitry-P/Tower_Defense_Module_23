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

        [SerializeField] private UpgradeAsset healthUpgrade;

        private int baseGoldCount = 135;
        protected override void Awake()
        {
            base.Awake();
            if (Gold <= 0)
                Gold = baseGoldCount;
        }

        public static event Action<int> OnGoldUpdate;
        public static void GoldUpdateSubscribe(Action<int> act)
        {
            Debug.Log("GoldChanged INVOKE from: " + Environment.StackTrace);
            OnGoldUpdate += act;
            act(Player.Instance.Gold);
        }
        public static void GoldUpdateUnsubscribe(Action<int> act)
        {
            OnGoldUpdate -= act;
        }

        public static event Action<int> OnLifeUpdate;
        public static void RaiseLifeUpdate(int lives) //метод-обёртка внутри TDPlayer, который будет вызывать событие.
        {
            OnLifeUpdate?.Invoke(lives);
        }

        public static void RaiseGoldUpdate(int gold) //метод-обёртка внутри TDPlayer, который будет вызывать событие.
        {
            OnGoldUpdate?.Invoke(gold);
        }

        public static void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Player.Instance.NumLives);
        }
        public static void LifeUpdateUnsubscribe(Action<int> act)
        {
            OnLifeUpdate -= act;
        }

        public void ChangeGold(int change)
        {
            Gold += change;
            OnGoldUpdate?.Invoke(Gold);
        }

        public void ReduceLife(int numLives_damage, string enemyName)
        {
            TakeDamage(numLives_damage);
            OnLifeUpdate?.Invoke(Player.Instance.NumLives);
        }

        public void TryBuild(TowerAsset towerAsset, Transform buildSite)
        {
            ChangeGold(-towerAsset.gold);
            var tower = Instantiate(towerPrefab, buildSite.position, Quaternion.identity);
            tower.Use(towerAsset);
            Destroy(buildSite.gameObject);
        } 
    }
}

