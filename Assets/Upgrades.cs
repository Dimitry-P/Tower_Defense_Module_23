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
        public class UpgradeSave
        {
            public UpgradeAsset asset;
            public int level = 0;
            public TowerUpgrade upgradeSO;   // ссылка на RadiusUpgrade, DamageUpgrade и т.д.
            public string name; // если нужно для UI, можно оставить
        }

        [SerializeField] public UpgradeSave[] save;

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

            Debug.Log("Runtime save length = ***********" + save.Length);
            for (int i = 0; i < save.Length; i++)
            {
                Debug.Log($"****************i={i}, asset={(save[i].asset != null ? save[i].asset.name : "NULL")}, level={save[i].level}");
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
            //Что здесь реально происходит?
            //сравнивается d wbrkt массив save с пришедшим в аргументе UpgradeAsset asset
            //В реальности же значения массива save сейчас выглядят так:
                    //save[0]->asset = HealthUpgrade
                    //save[1]->asset = null
                    //save[2]->asset = null
                    //save[3]->asset = null
            //Первый элемент НЕ равен null, потому что программа его берёт из инспектора у компонента Upgrades.
            //// Null-элементы пропускаются, так что сравнение происходит только с реально назначенными апгрейдами
            Debug.Log("Save.Length = " + Instance.save.Length);
            for (int i = 0; i < Instance.save.Length; i++)
            {
                if (Instance.save[i].asset == null)
                    continue;
                if (Instance.save[i].asset == asset) //ЗДЕСЬ СРАВНИВАЮТСЯ ССЫЛКИ!!!
                {
                    Debug.Log(";;;;;;;;;;;;;;;;;" + Instance.save[i].asset.name);
                    Debug.Log("*;;;;;;;;;;;;;;;;*MATCH FOUND: level=" + Instance.save[i].level);
                    Debug.Log("PARAM TYPE = ;;;;;;;;" + asset.GetType().Name);
                    Debug.Log("PARAM NAME = ;;;;;;;;;;;;;;;" + asset.name);
                    //UpgradeAsset-Ы ДАННОГО МАССИВА ЭТОТ МЕТОД БЕРЁТ ИЗ ИНСПЕКТОРА,
                    //НЕСМОТРЯ НА ТО ЧТО ФАЙЛ upgrade.dat ЕЩЁ НЕ СОЗДАН.
                    return Instance.save[i].level;
                }
            }
            return 0;
        }
    }
}

