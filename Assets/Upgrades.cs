using SpaceShooter;
using System;
using UnityEngine;
using static TowerDefense.MapCompletion;

namespace TowerDefense
{
    public class Upgrades : MonoSingleton<Upgrades> //Почему моносинглтон? Потому что должен путешествовать между сценами
    {
        public const string filename = "upgrades.dat"; 
        [Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset asset;
            public int level = 0;
        }
        [SerializeField] private UpgradeSave[] save;

        private new void Awake()
        {
            base.Awake();

            UpgradeSave[] loaded = null;
            Saver<UpgradeSave[]>.TryLoad(filename, ref loaded);

            if (loaded != null && loaded.Length == save.Length)
            {
                for (int i = 0; i < save.Length; i++)
                {
                    save[i].level = loaded[i].level;
                }
            }
        }


        //private new void Awake()
        //{
        //    base.Awake();
        //    Saver<UpgradeSave[]>.TryLoad(filename, ref save);
        //}

        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset == asset)
                {
                    upgrade.level += 1;
                    Saver<UpgradeSave[]>.Save(filename, Instance.save);
                }
            }
        }

        public static int GetTotalCost()
        {
            int result = 0;
            foreach(var upgrade in Instance.save)
            {
                for(int i = 0; i < upgrade.level; i++)
                {
                    result += upgrade.asset.costByLevel[i];
                }
            }
            return result;
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.save)
            {
                if (upgrade.asset == asset)
                {
                   return upgrade.level;
                }
            }
            return 0;
        }
    }
}

