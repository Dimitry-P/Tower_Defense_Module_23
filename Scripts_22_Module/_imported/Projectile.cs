using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using Towers;
using Towers.std;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace SpaceShooter
{
    /// <summary>
    /// Скрипт прожектайла. Кидается на топ префаба прожектайла.
    /// </summary>
    public class Projectile : Entity
    {
        /// <summary>
        /// Линейная скорость полета снаряда.
        /// </summary>
        public float m_Velocity;

        /// <summary>
        /// Время жизни снаряда.
        /// </summary>
        public float m_Lifetime;

        /// <summary>
        /// Повреждения наносимые снарядом.
        /// </summary>
        public int m_Damage;

        /// <summary>
        /// Эффект попадания от что то твердое. 
        /// </summary>
        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

        private float m_Timer;
        private EVariousMech _variousMechType; 
        public float _towerRadius;
        public VariousMech _variousMech;

        public Turret turret;

        private bool hasHit = false;

        //private Destructible theHitTarget;
        //public VariousTowerMechanics variousTowerMechanics;

        //private void Start()
        //{
        //    variousTowerMechanics = GetComponent<VariousTowerMechanics>();
        //}

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            // не забыть выключить в свойствах проекта, вкладка Physics2D иначе не заработает
            // disable queries hit triggers
            // disable queries start in collider
            if (hit)
            {
                if (hasHit) return;
                hasHit = true;
                Debug.Log($"HIT frame={Time.frameCount} collider={hit.collider.name}");
                Debug.Log("ПУЛЯ СТОЛКНУЛАСЬ С: " + hit.collider.gameObject.name +
          " | layer: " + hit.collider.gameObject.layer);
                var destructible = hit.collider.transform.root.GetComponent<Destructible>();

                if (destructible != null && destructible.gameObject != m_Parent?.gameObject)
                {
                    destructible.GetComponent<Enemy>().m_gold = m_Damage;
                    // здесь наносим урон / эффект
                    _variousMech?.TryApplyDamage(destructible);

                    // добавляем ЭТУ строку — регистрируем попадание от конкретной башни
                    destructible.RegisterHitFromTower(_ownerTower);

                    int hitLayer = hit.collider.transform.root.gameObject.layer;

                    if (hitLayer == LayerMask.NameToLayer("Enemy"))
                    {
                         OnProjectileLifeEnd(hit.collider, hit.point);
                    }
                }

                // #Score
                // добавляем очки за уничтожение
                if (Player.Instance != null && destructible.HitPoints < 0)
                    {
                        // проверяем что прожектайл принадлежит кораблю игрока. 
                        // здесь есть нюанс - если мы выстрелим прожектайл и после умрем
                        // то новый корабль игрока будет другим, в случае если прожектайл запущенный из предыдущего шипа
                        // добьет то очков не дадут. Можно отправить пофиксить на ДЗ. (например тупо воткнув флаг что прожектайл игрока)
                        if (m_Parent == Player.Instance.ActiveShip)
                        {
                            Player.Instance.AddScore(destructible.ScoreValue);
                        }
                    }
                return; // ВАЖНО: прерываем Update
            }

            m_Timer += Time.deltaTime;

            if(m_Timer > m_Lifetime)
            {
                Destroy(gameObject);
            }
            if (!hasHit)
            {
                transform.position += new Vector3(step.x, step.y, 0) * 0.5f;
            }
        }

        private void OnProjectileLifeEnd(Collider2D collider, Vector2 pos)
        {
            if(m_ImpactEffectPrefab != null)
            {
                var impact = Instantiate(m_ImpactEffectPrefab.gameObject);
                impact.transform.position = pos;
            }
            Destroy(this.gameObject);
        }

        private SpaceShip m_Parent;

        public void SetParentShooter(SpaceShip parent)
        {
            m_Parent = parent;
        }

        public void SetTarget(Destructible target)
        {

        }

        internal object GetChild(int v)
        {
            throw new NotImplementedException();
        }

        //public void Use(ProjectileAsset projectileAsset)
        //{
        //    var e = GetComponentInChildren<SpriteRenderer>();

        //    if (e == null)
        //    {
        //        Debug.LogError("Projectile: SpriteRenderer not found!");
        //        return;
        //    }

        //    //e.sprite = projectileAsset.sprite;
        //    //Debug.Log("Projectile instantiated: " + projectile.name);
        //    Debug.Log("SpriteRenderer: " + e);
        //    m_Damage = projectileAsset.damage;
        //    m_Velocity = projectileAsset.velocity;
        //    m_Lifetime = projectileAsset.lifetime;
        //}

        private Tower _ownerTower;

        public void Init(Tower tower)
        {
            _ownerTower = tower;
        }

        public void InitTurretSpecificSettings(EVariousMech variousType, float towerRadius, TurretProperties turretProperties)
        {
            _variousMechType = variousType;
            _towerRadius = towerRadius;
            CreateTowerSpecificMechanic(turretProperties);
        }
        
        private void CreateTowerSpecificMechanic(TurretProperties turretProperties )
        {
            if (_variousMechType != EVariousMech.None && _towerRadius != 0)
            {
                switch (_variousMechType)
                {
                    case EVariousMech.DoT_Poison:
                        _variousMech = gameObject.AddComponent<VariousTowerMechanicsPoisonTower>();
                        break; 
                    case EVariousMech.Dps:
                        _variousMech = gameObject.AddComponent<VariousTowerMechanicsDPSTower>();
                        break;
                    case EVariousMech.Archer:
                        _variousMech = gameObject.AddComponent<VariousTowerMechanicsArcherTower>();
                        break;
                    case EVariousMech.AoE:
                        _variousMech = gameObject.AddComponent<VariousTowerMechanicsAoETower>();
                        break;
                    case EVariousMech.Ice:
                        _variousMech = gameObject.AddComponent<VariousTowerMechanicsIceTower>();
                        break;
                    case EVariousMech.Single:
                        _variousMech = gameObject.AddComponent<VariousTowerMechanicsSingleTower>();
                        break;
                    case EVariousMech.SlowDown:
                        _variousMech = gameObject.AddComponent<VariousTowerMechanicsSlowDownTower>();
                        break;
                    case EVariousMech.Boss:
                        _variousMech = gameObject.AddComponent<VariousTowerMechanicsBossTower>();
                        break;
                }
                _variousMech?.Init(_ownerTower);
                _variousMech?.UseSpecificMechanic(turretProperties);
            }
        }
    }
}

