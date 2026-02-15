using TowerDefense;
using Towers;
using Towers.std;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace SpaceShooter
{
    /// <summary>
    /// Турелька корабля. Требует аудио источник для выдачи спецэффекта при стрельбе.
    /// Требует на верхенм уровне скрипт SpaceShip для вычитания патронов и энергии.
    /// </summary>
    public class Turret : MonoBehaviour
    {
        /// <summary>
        /// Тип турели, первичный или вторичный.
        /// </summary>
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        /// <summary>
        /// Текущие патроны в турели.
        /// </summary>
        [SerializeField] private TurretProperties m_TurretProperties;

        private EVariousMech _variousMechType; 
        private float _towerRadius; 

        /// <summary>
        /// Таймер повторного выстрела.
        /// </summary>
        private float m_RefireTimer = 0f;

        /// <summary>
        /// Стрелять можем? 
        /// </summary>
        public bool CanFire => m_RefireTimer <= 0;

        /// <summary>
        /// Кешированная ссылка на родительский шип.
        /// </summary>
        private SpaceShip m_Ship;

        


        #region Unity events

        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;
            else if (Mode == TurretMode.Auto)
                Fire();
        }

        #endregion

        #region Public API

        public void InitTurretSpecificSettings(EVariousMech variousType, float towerRadius)
        {
            _variousMechType = variousType;
            _towerRadius = towerRadius;
        }

        public Tower tow;
        public void Init(Tower tower)
        {
            tow = tower;
        }

        private Projectile projectile;
        /// <summary>
        /// Метод стрельбы турелью. 
        /// </summary>
        public void Fire()
        {
            if (m_RefireTimer > 0)
                return;

            if (m_TurretProperties == null)
            {
                return;
            }
               


            if (m_Ship)
            {
                // кушаем энергию
                if (!m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage))
                    return;

                // кушаем патроны 
                if (!m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage))
                    return;
            }




            // инстанцируем прожектайл который уже сам полетит.
            projectile = Instantiate(m_TurretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();

            projectile.transform.up = transform.up;

            projectile.Init(tow);

            projectile.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = m_TurretProperties.ProjectileSprite;
           
            projectile.m_Velocity = m_TurretProperties.ProjectileSpeed;

            projectile.m_Damage = m_TurretProperties.Damage;
           
            projectile.m_Lifetime = 5f;

            projectile.InitTurretSpecificSettings(_variousMechType, _towerRadius, m_TurretProperties);

            projectile.transform.position = transform.position;
           
            // метод выставления данных прожектайлу о том кто стрелял для избавления от попаданий в самого себя
            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

            {
                // SFX на домашку
            }
        }

      
        public void AssignLoadout2(TowerAsset towerAsset)
        {
            m_TurretProperties = towerAsset.turretProperties;
        }



        /// <summary>
        /// Установка свойств турели. Будет использовано в дальнейшем для паверапки.
        /// </summary>
        /// <param name="props"></param>
        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode)
                return;

            m_TurretProperties = props;
            m_RefireTimer = 0;
        }
        #endregion
    }
}



   

