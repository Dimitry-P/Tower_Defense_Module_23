using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using Unity.Profiling;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Летательный аппарат 2Д.
    /// NOTE: важно учесть соотношение сил и скоростей чтобы физический движок не выдал пакость.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// Масса для автоматической установки у ригида.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Толкающая вперед сила.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Вращающая сила.
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        public float MaxLinearVelocity
        {
            get { return m_MaxLinearVelocity; }
            set { m_MaxLinearVelocity = value; }
        }


        /// <summary>
        /// Максимальная вращательная скорость. В градусах/сек
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;

        /// <summary>
        /// Сохраненная ссылка на ригид.
        /// </summary>
        private Rigidbody2D m_Rigid;
        public Rigidbody2D Rigid => m_Rigid;

       

        [SerializeField] private Transform targetPoint; // база игрока

        private float startDistance;


        #region Public API

        /// <summary>
        /// Управление линейной тягой. -1.0 до +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. -1.0 до +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        private Animator m_Animator;

        public bool FreezeImmune { get; set; }
        private bool isFrozen;
        private float freezeTimer;

        public void Freeze(float duration)
        {
            if (FreezeImmune) return;

            isFrozen = true;
            freezeTimer = duration;
        }

        private AIController controller;
        private void Awake()
        {
            controller = GetComponent<AIController>();
        }

        #region Unity events

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            // единичная инерция для того чтобы упростить баланс кораблей.
            // либо неравномерные коллайдеры будут портить вращение
            // решается домножением торка на момент инерции
            m_Rigid.inertia = 1;
            m_Animator = GetComponentInChildren<Animator>();
            // ВАЖНО
            if (targetPoint != null)
                startDistance = Vector2.Distance(transform.position, targetPoint.position);
            //InitOffensive();
        }

        public void SetTargetPoint(Transform target)
        {
            targetPoint = target;
            startDistance = Vector2.Distance(transform.position, targetPoint.position);
        }

       

        private void FixedUpdate()
        {
            if (isFrozen)
            {
                freezeTimer -= Time.fixedDeltaTime;
                if (freezeTimer <= 0f)
                    isFrozen = false;
            }

            if (slowTimer > 0f)
            {
                slowTimer -= Time.fixedDeltaTime;
                if (slowTimer <= 0f)
                {
                    slowMultiplier = 1f;
                }
            }

          
            UpdateRigidbody();
            //UpdateEnergyRegen();

            // ВОТ ОНО
            if (targetPoint != null && startDistance > 0f)
            {
                float currentDistance = Vector2.Distance(transform.position, targetPoint.position);
                PathProgress = Mathf.Clamp01(1f - currentDistance / startDistance);
            }
        }

        private void UpdateRigidbody()
        {
            if (controller == null)
                return; // ← КРИТИЧЕСКИ ВАЖНО

            if (isFrozen)
            {
                m_Rigid.velocity = Vector2.zero;
                m_Rigid.angularVelocity = 0f;
                if (m_Animator != null)
                    m_Animator.speed = 0f;
                return;
            }
           
            if (m_Animator != null)
                m_Animator.speed = 1f;
            Vector2 dir = (controller.PatrolPoint.transform.position - transform.position).normalized;

            m_Rigid.velocity = dir * m_MaxLinearVelocity * SpeedMultiplier;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            m_Rigid.MoveRotation(angle);
        }

        #endregion





        //НАМ НУЖНА КОМПОЗИЦИЯ ЭФФЕКТОВ:
        // временное замедление (ice tower)
        private float slowMultiplier = 1f;
        private float slowTimer = 0f;

        // базовый множитель (на будущее)
        [SerializeField]
        private float baseSpeedMultiplier = 1f;

        // постоянные ауры
        private readonly HashSet<float> speedAuras = new HashSet<float>();

        public float SpeedMultiplier
        {
            get
            {
                float auraMul = 1f;
                foreach (var m in speedAuras)
                    auraMul *= m;

                return baseSpeedMultiplier * slowMultiplier * auraMul;
            }
        }

        //ПРАВИЛЬНОЕ ЗАМЕДЛЕНИЕ(SlowDown Tower)
        public void ApplySlow(float multiplier, float duration)
        {
            slowMultiplier = Mathf.Clamp(multiplier, 0f, 1f);
            slowTimer = duration;
        }

        public void AddSpeedAura(float multiplier)
        {
            speedAuras.Add(multiplier);
        }

        public void RemoveSpeedAura(float multiplier)
        {
            speedAuras.Remove(multiplier);
        }




      

        //#region Offensive

        //private const int StartingAmmoCount = 10;

        ///// <summary>
        ///// Ссылки на турели корабля. Турели класть отдельными геймобъектами.
        ///// Каждая турель ест патроны и энергию.
        ///// </summary>
        //[SerializeField] private Turret[] m_Turrets;

        ///// <summary>
        ///// Максимум энергии на корабле.
        ///// </summary>
        //[SerializeField] private int m_MaxEnergy;

        ///// <summary>
        ///// Максимум патронов на корабле.
        ///// </summary>
        //[SerializeField] private int m_MaxAmmo;

        ///// <summary>
        ///// Скорость регенерации энергии в секунду.
        ///// </summary>
        //[SerializeField] private int m_EnergyRegenPerSecond;

        ///// <summary>
        ///// Кол-ыо энергии на корабле. float чтоб был смысл в свойстве реген в секунду.
        ///// </summary>
        //private float m_PrimaryEnergy;

        //public void AddEnergy(int e)
        //{
        //    m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        //}

        ///// <summary>
        ///// Кол-во патронов.
        ///// </summary>
        //private int m_SecondaryAmmo;

        //public void AddAmmo(int ammo)
        //{
        //    m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        //}

        ///// <summary>
        ///// Инициализация начальный свойств корабля.
        ///// </summary>
        //private void InitOffensive()
        //{
        //    m_PrimaryEnergy = m_MaxEnergy;
        //    m_SecondaryAmmo = StartingAmmoCount;
        //}

        ///// <summary>
        ///// Обновляем статы корабля. Можно воткнуть например автопочинку.
        ///// </summary>
        //private void UpdateEnergyRegen()
        //{
        //    m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
        //}

        /// <summary>
        /// TODO: заменить временный метод-заглушку
        ///  /// Используется турелями
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true если патроны были скушаны</returns>
        public bool DrawAmmo(int count)
        {
              return true;
        }

        /// <summary>
        /// TODO: заменить временный метод-заглушку
        /// Используется турелями
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true если патроны были скушаны</returns>
        public bool DrawEnergy(int count)
        {
             return true;
        }

        /// <summary>
        /// TODO: заменить временный метод-заглушку
        /// Используется ИИ
        /// </summary>
        public void Fire(TurretMode mode)
        {
           return;
        }

        public void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.moveSpeed;
            base.Use(asset);
        }

        //#endregion

        //public void AssignWeapon(TurretProperties props)
        //{
        //    foreach (var v in m_Turrets)
        //        v.AssignLoadout(props);
        //}
    }
}



//using SpaceShooter;
//using System.Collections;
//using System.Collections.Generic;
//using TowerDefense;
//using UnityEngine;
//using Towers;

//public class TowerSpawner : MonoBehaviour
//{
//    [SerializeField] private Projectile projectile;
//    [SerializeField] private TowerAsset towerAsset;
//    private void Start()
//    {
//        Use();
//    }


//    private void Use()
//    {
//        var e = Instantiate(projectile);
//        e.Use(towerAsset);
//    }
//}
