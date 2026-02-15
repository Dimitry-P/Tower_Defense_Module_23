using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense;
using Towers;
using Towers.std;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static Unity.VisualScripting.Member;


namespace SpaceShooter
{
    /// <summary>
    /// Уничтожаемый объект на сцене. То что может иметь хит поинты.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties

        /// <summary>
        /// Объект игнорирует повреждения.
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Стартовое кол-во хитпоинтов.
        /// </summary>
        [SerializeField] private int m_HitPoints;
        
        /// <summary>
        /// Текущие хит поинты
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        // Прогресс по пути от 0 (старт) до 1 (конец)
        public float PathProgress { get; set; }

        #endregion

        #region Unity events

        private bool m_isBoss;

        public VariousMech lastDamageSource;

        public bool IsBoss 
        {
            get {  return m_isBoss; } 
            set { m_isBoss = value; }
        }

        private void Awake()
        {
            isPoinsoned = false;
        }

        public VariousMech PoisonSource;

        // Новое поле — множество башен, которые уже попали в этого врага
        private HashSet<Tower> _hitByTowers = new HashSet<Tower>();

        // Методы для работы с этим множеством
        public bool WasAlreadyHitBy(Tower tower) => _hitByTowers.Contains(tower);

        public void RegisterHitFromTower(Tower tower)
        {
            _hitByTowers.Add(tower);
        }

        // Желательно очищать при смерти/удалении, но не обязательно
        private void OnDestroy()
        {
            _hitByTowers.Clear();
            IsDestroyed = true;
        }

        public bool IsDestroyed { get; private set; }


        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
            transform.localScale = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        #region Безтеговая коллекция скриптов на сцене

        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
        }

        //protected virtual void OnDestroy()
        //{
        //    m_AllDestructibles.Remove(this);
        //}

        #endregion 

        #endregion

        #region Public API

        public bool isDead = false;

        /// <summary>
        /// Применение дамага к объекту.
        /// </summary>
        /// <param name="damage"></param>

      

        private float timer = 7f;



        //  Новый ApplyDamage — с источником урона - для башни DPS
        public void ApplyDamage(int damage, VariousMech source)
        {
            if (m_Indestructible)
                return;

            lastDamageSource = source;

            ShowDamage(damage);
            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
            {
                DPSGlobalManager.RegisterKill(lastDamageSource);
                lastDamageSource?.OnEnemyKilled();
                OnDeath();
            }
        }

        public GameObject damagePopupPrefab;

        private int damagePopupIndex = 0;
        private float popupResetDelay = 0.25f;
        private float popupTimer;

        private void ShowDamage(int damage)
        {
            float xOffset = UnityEngine.Random.Range(-0.2f, 0.2f);
            float yOffset = 1f + damagePopupIndex * 0.8f;

            Vector3 spawnPos = transform.position + new Vector3(xOffset, yOffset, 0f);

            GameObject popup = Instantiate(
                damagePopupPrefab,
                spawnPos,
                Quaternion.identity
            );

            popup.GetComponent<DamagePopUp>().Setup(damage);

            damagePopupIndex++;
            popupTimer = popupResetDelay;
        }

        private void Update()
        {
            if (popupTimer > 0f)
            {
                popupTimer -= Time.deltaTime;
                if (popupTimer <= 0f)
                {
                    damagePopupIndex = 0;
                }
            }
        }

        public void AddHitPoints(float hp)
        {
            m_CurrentHitPoints = (int)Mathf.Clamp(m_CurrentHitPoints + hp, 0, m_HitPoints);
        }

        #endregion

        /// <summary>
        /// Перепоределяемое событие уничтожения объекта, когда хит поинты ниже нуля.
        /// </summary>
        /// 

        public void SetColorTemporary(Color color, float duration)
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite == null) return;

            sprite.color = color;

            CancelInvoke(nameof(ResetColor));
            Invoke(nameof(ResetColor), duration);
        }

        private void ResetColor()
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null)
                sprite.color = Color.white;
        }

        private Coroutine poisonCoroutine;

        public void ApplyPoison(int damagePerSecond, float duration, VariousMech source)
        {
            if (poisonCoroutine != null)
                StopCoroutine(poisonCoroutine);

            poisonCoroutine = StartCoroutine(PoisonCoroutine(damagePerSecond, duration, source));
            IsPoisoned = false;
        }
        private bool isPoinsoned;
        public bool IsPoisoned
        {
            get {return isPoinsoned;}
            set {isPoinsoned = value;}
        }

        private IEnumerator PoisonCoroutine(int damage, float duration, VariousMech source)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                ApplyDamage(damage, source);
                yield return new WaitForSeconds(1f);
                elapsed += 1f;
            }
                poisonCoroutine = null;
        }

        protected virtual void OnDeath()
        {
            if (m_ExplosionPrefab != null)
            {
                var explosion = Instantiate(m_ExplosionPrefab.gameObject);
                explosion.transform.position = transform.position;
            }

            var enemyName = gameObject.GetComponent<Enemy>();
            string nm = enemyName.enemyName;
            if (nm == "boss")
            {
                Debug.Log("You are the winner");
                GameOver();
                return;
            } 
                Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        private void GameOver()
        {
            Destroy(gameObject);
            if (SceneManager.GetActiveScene().name == "Level_1")
            {
                GameReset.ResetStatics();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            }
            if (SceneManager.GetActiveScene().name == "Level_2")
            {
                Player.Instance.victoryPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        [SerializeField] private ImpactEffect m_ExplosionPrefab;

        #region Teams

        /// <summary>
        /// Полностью нейтральный тим ид. Боты будут игнорировать такие объекты.
        /// </summary>
        public const int TeamIdNeutral = 0;

        /// <summary>
        /// ИД стороны. Боты будут атаковать всех кто не свой.
        /// </summary>
        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        #endregion

        #region Score

        /// <summary>
        /// Кол-во очков за уничтожение. Также проверяю настройку UTF-8
        /// </summary>
        private int m_ScoreValue;

        public int ScoreValue
        {
            get
            {
                 return m_ScoreValue;
            } 
            set 
            {
               
            }
        }
            
        #endregion

        protected void Use(EnemyAsset asset)
        {
            m_HitPoints = asset.hp;
            if (asset.nameOfEnemy == "small")m_ScoreValue = 10;
            if (asset.nameOfEnemy == "middle")m_ScoreValue = 20;
            if (asset.nameOfEnemy == "boss")m_ScoreValue = 50;
        }
    }
}