using SpaceShooter;
using System;
using System.Collections;
using TowerDefense;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace TowerDefense
{
    public class Abilities : MonoSingleton<Abilities>
    {
        private UpgradeAsset usingThisAbilityAssetNow;
        private float divisor = 0;

        private void OnEnable()
        {
            Enemy.OnEnemyKilled += OnEnemyKilledHandler;
        }
        private void OnEnemyKilledHandler(Enemy enemy)
        {
            AddEnergy(20f); 
        }
      
        private void OnDisable()
        {
            Enemy.OnEnemyKilled -= OnEnemyKilledHandler;

            EnemyWaveManager.OnEnemySpawn -= Slow;

            m_IsTimeAbilityOnCooldown = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Abilities.Instance.AddEnergy(100);
                Debug.Log("100%%");
            }
        }
        public bool IsUnlocked(UpgradeAsset abilityAsset)
        {
            usingThisAbilityAssetNow = abilityAsset;
           return Upgrades.GetUpgradeLevel(abilityAsset) > 0;
        }

        //Это точка входа — сюда приходит UI (когда ты нажал кнопку)
        //обычный публичный метод, его вызывает AbilitiesView
        //MonoBehaviour runner
        //объект, который умеет запускать корутины
        //обычно это AbilitiesView(UI)
        //Button button -- кнопка, которую надо заблокировать
        //float duration -- сколько длится эффект(замедление)
        //float cooldown, сколько длится перезарядка
        public void UseTimeAbility(MonoBehaviour runner, UnityEngine.UI.Button button, float duration, float cooldown)
        {
            if (m_IsTimeAbilityOnCooldown)
                return;
          

            divisor = 1f;
            if (Upgrades.GetUpgradeLevel(usingThisAbilityAssetNow) > 1)
            {
                for (int i = 1; i < Upgrades.GetUpgradeLevel(usingThisAbilityAssetNow); i++)
                {
                    divisor /= 2;
                }
            }

            if (TDPlayer.Instance.Gold < m_TimeAbility.Cost)
                return;

            m_IsTimeAbilityOnCooldown = true;

            TDPlayer.Instance.ChangeGold(-m_TimeAbility.Cost);

            runner.StartCoroutine(TimeRoutine(button, duration, cooldown));
            //"запусти корутину TimeRoutine"
            //корутина запускается НЕ в Singleton, а в runner(UI объекте)
            //ВАЖНЫЕ МОМЕНТЫ:
            //1. Почему нужен runner? Coroutine можно запускать только из MonoBehaviour
            //2. Почему передаём кнопку?  Singleton НЕ должен знать про UI
            //3. Почему нужна отписка? - Иначе все будущие враги будут ВСЕГДА замедлены
            Debug.Log("INSTANCE 777= " + m_TimeAbility);
            Debug.Log("COST 777= " + m_TimeAbility.Cost);
        }


        private void Slow(Enemy ship) //"если появится враг, то замедли его"
        {
            var spaceShip = ship.GetComponent<SpaceShip>();

            if (spaceShip != null && divisor > 0)
                spaceShip.SetSlowMultiplier(divisor);
        }

        //это корутина — метод, который выполняется во времени
        private IEnumerator TimeRoutine(UnityEngine.UI.Button button, float duration, float cooldown)
        {
            //button.interactable = false; //кнопка становится неактивной, игрок не может нажать повторно

            if (divisor > 0)
            {
                foreach (var enemy in FindObjectsOfType<Enemy>())
                {
                    var ship = enemy.GetComponent<SpaceShip>();
                    if (ship != null)
                        ship.SetSlowMultiplier(divisor);
                }
                //Unity находит всех врагов на сцене и каждому уменьшает скорость
            }
              

            EnemyWaveManager.OnEnemySpawn += Slow;  //подписка на новых врагов //каждый новый враг - автоматически замедляется
           
            yield return new WaitForSeconds(duration);  //ждём длительность эффекта //игра продолжает идти но этот метод "засыпает" на duration секунд

            if (divisor > 0)
            {
                foreach (var ship in FindObjectsOfType<SpaceShip>())  //возвращаем скорость //всем врагам возвращаем нормальную скорость
                {
                    ship.SetSlowMultiplier(1f);
                }
            }

            EnemyWaveManager.OnEnemySpawn -= Slow; //отписка. больше НЕ замедляем новых врагов, если забыть это — будет баг!!!

            //yield return new WaitForSeconds(cooldown); //ждём кулдаун. ещё пауза — уже для кнопки

            //button.interactable = true; //включаем кнопку обратно. теперь игрок снова может нажать способность
            divisor = m_FireAbility.Damage;
            m_IsTimeAbilityOnCooldown = false;
        }

        private bool m_IsTimeAbilityOnCooldown;
        public bool IsTimeAbilityOnCooldown => m_IsTimeAbilityOnCooldown;



        //ПОЛНАЯ ЦЕПОЧКА
        //1. Игрок нажал кнопку
        //2. Кнопка выключилась
        //3. Все враги замедлились
        //4. Новые враги тоже замедляются
        //5. Ждём duration
        //6. Скорость возвращается
        //7. Новые враги больше НЕ замедляются
        //8. Ждём cooldown
        //9. Кнопка снова активна


        [SerializeField] private FireAbility m_FireAbility; //они теперь хранят данные из инспектора
        [SerializeField] private TimeAbility m_TimeAbility; //они теперь хранят данные из инспектора
                                                            //КЛЮЧЕВОЕ ОСОЗНАНИЕ
                                                            //"класс хранит данные → система выполняет действие"

        public int FireAbilityGold => m_FireAbility.FireAbilityGold;

        [Serializable]
        public class FireAbility
        {
            [SerializeField] private int m_Cost = 5;
            public int FireAbilityGold => m_Cost;
            [SerializeField] private int m_Damage = 2;
            [SerializeField] private Color m_TargetingColor;

            public int Damage => m_Damage;
            public Color TargetingColor => m_TargetingColor;
        }

        public int TimeAbilityGold => m_TimeAbility.TimeAbilityGold;

        [Serializable]
        public class TimeAbility
        {
            [SerializeField] private int m_Cost = 10;
            public int Cost => m_Cost;
            private int m_MinDemandedGold = 20;
            public int TimeAbilityGold => m_MinDemandedGold;
            [SerializeField] private int m_Duration = 5;

            public int Duration => m_Duration;
        }


        //private int m_DamagePoints = 0;
        //public int DamagePoints { get { return m_DamagePoints; } set { m_DamagePoints = value; } }
        //public int alreadySavedDamage = 0;
        public void UseFireAbility()
        {
            int damage = m_FireAbility.Damage;

            ClickProtection.Instance.Activate((Vector2 v) =>
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(v);
                worldPos.z = 0;
                Debug.Log("SCREEN: " + v);
                Debug.Log("WORLD: " + worldPos);

                foreach (var collider in Physics2D.OverlapCircleAll(worldPos, 5))
                {
                    Debug.Log("HIT OBJECT: " + collider.name);

                    var enemy = collider.GetComponent<Enemy>();

                    if (enemy == null)
                        enemy = collider.GetComponentInParent<Enemy>();

                    if (enemy != null)
                    {
                        Debug.Log("DAMAGE TO: " + enemy.name);
                        Debug.Log("666 GetupgradeLevel= " + Upgrades.GetUpgradeLevel(usingThisAbilityAssetNow));
                        if (Upgrades.GetUpgradeLevel(usingThisAbilityAssetNow) > 1)
                        {
                            for(int i = 1; i < Upgrades.GetUpgradeLevel(usingThisAbilityAssetNow); i++)
                            {
                                damage += 2;
                                Debug.Log($"666789 {i} = " + damage);
                            }
                        }
                        Debug.Log("Damage without ENERGY ABILITY is: " + damage);
                        // ENERGY БОНУС
                        if (IsEnergyFull)
                        {
                            damage *= 2;
                            Debug.Log("ENERGY BONUS ACTIVATED!");
                            ResetEnergy();
                            Debug.Log("BECAUSE OF ENERGY ABILITY DAMAGE*2 = " + damage);
                        }

                        enemy.TakeDamage(damage, TDProjectile.DamageType.Magic);
                        //m_DamagePoints = damage;
                        damage = m_FireAbility.Damage;
                        //Debug.Log("666 1= " + m_DamagePoints);
                        Debug.Log("666 2= " + damage);
                       
                    }
                }
            });
            Debug.Log("ClickProtection = " + ClickProtection.Instance);
        }



        //новый вид ресурса, отображающийся в интерфейсе игры
      
        [SerializeField] private float m_MaxEnergy = 100f;
        private float m_Energy;

        public float Energy01 => m_Energy / m_MaxEnergy;

        public bool IsEnergyFull => m_Energy >= m_MaxEnergy;

        public void AddEnergy(float amount)
        {
            m_Energy += amount;

            if (m_Energy > m_MaxEnergy)
                m_Energy = m_MaxEnergy;

            Debug.Log("Energy: " + m_Energy);
        }

        public void ResetEnergy()
        {
            m_Energy = 0f;
        }

    }
}







        //-------СТАРЫЙ ВАРИАНТ. ВАРИАНТ МЕНТОРА--------

        //[Serializable]
        //public class FireAbility
        //{
        //    [SerializeField] private int m_Cost = 5;
        //    [SerializeField] private int m_Damage = 2;
        //    [SerializeField] private Color m_TargetingColor;
        //    public void Use()
        //    {
        //        //Vector2 - это координаты мышки
        //        ClickProtection.Instance.Activate((Vector2 v) =>
        //        {
        //            Vector3 position = v;
        //            position.z = -Camera.main.transform.position.z;
        //            position = Camera.main.ScreenToWorldPoint(position);
        //            foreach (var collider in Physics2D.OverlapCircleAll(position, 5))
        //            {
        //                if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
        //                {
        //                    enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Magic);
        //                }
        //            }
        //        });
        //    }
        //}

        //[Serializable]
        //public class TimeAbility
        //{
        //    public void Use()
        //    {
        //        void Slow(Enemy ship)
        //        {
        //            ship.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
        //        }

        //        foreach (var ship in FindObjectsOfType<SpaceShip>())
        //            ship.HalfMaxLinearVelocity();
        //        EnemyWaveManager.OnEnemySpawn += Slow;
        //        IEnumerator Restore()
        //        {
        //            yield return new WaitForSeconds(m_Duration);
        //            foreach (var ship in FindObjectsOfType<SpaceShip>())
        //                ship.RestoreMaxLinearVelocity();
        //            EnemyWaveManager.OnEnemySpawn -= Slow;
        //        }
        //        Instance.StartCoroutine(Restore());

        //        IEnumerator TimeAbilityButton()
        //        {
        //            Instance.m_TimeButton.interactable = false;
        //            yield return new WaitForSeconds(m_Cooldown);
        //            Instance.m_TimeButton.interactable = true;
        //        }
        //        Instance.StartCoroutine(TimeAbilityButton());
        //    }
        //}


       
        //public void UseFireAbility() => m_FireAbility.Use();
        //public void UseTimeAbility() => m_TimeAbility.Use();




