using System;
using TowerDefense;
using UnityEngine;
using SpaceShooter;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UIElements;

namespace TowerDefense
{
    [RequireComponent(typeof(Destructible))]
    [RequireComponent(typeof(TDController))]
    public class Enemy : MonoBehaviour
    {
        public static System.Action<Enemy> OnEnemyKilled;
        public enum ArmorType { Base=0, Mage=1} // Здесь важно явно прописать, что у конкретных видов брони есть номера: 0 и 1
        private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions =
        { //Func - это очень похоже на Action, только последним параметром у него является 
            //возвращаемый тип значения.
            // Смысл в том, что мы для каждого индекса, который соответствует каждому типу брони,
            // пропишем его собственную функцию.
            (int power, TDProjectile.DamageType type, int armor) =>
            { // ArmorType.Base
                switch (type)
                {
                    case TDProjectile.DamageType.Magic: return power;
                    default: return Mathf.Max(power - armor, 1);
                }
            },
            (int power, TDProjectile.DamageType type, int armor) =>
            { // ArmorType.Magic
                if(TDProjectile.DamageType.Base == type)armor = armor / 2;
                return Mathf.Max(power - armor, 1);
            },
        };

        private int m_NumLives_damage;
        private string m_name;
        [SerializeField] private int m_armor = 1;
        [SerializeField] private ArmorType m_ArmorType;

        private Destructible m_destructible;
        private void Awake()
        {
            m_destructible = GetComponent<Destructible>();
        }

        public event Action OnEnd;
        private void OnDestroy() { print(name);  OnEnd?.Invoke(); }

        public void Use(EnemyAsset asset)//эта функция осущ-ет подцепление настроек для врагов.
        {
            var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);

            sr.sprite = asset.sprite;

            m_name = asset.enemyName;
            m_NumLives_damage = asset.numLives_Damage;
            m_armor = asset.armor;
            m_ArmorType = asset.armorType;

            //Для того, чтобы можно было переключать ассеты в инспекторе префаба, нужно
            //закомментить передачу анимации:
            //Анимация перекрывает спрайт, поэтому тут мы её не передаём
            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animations;

            GetComponent<SpaceShip>().Use(asset);

            GetComponentInChildren<CircleCollider2D>().radius = asset.radius;
        }

        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_NumLives_damage, m_name);
        }

        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(1);
            //(Player.Instance as TDPlayer).ChangeGold(m_gold);//достань игрока, представь его в виде класса TDPlayer и измени на нём золото.
            //Т.е. когда запускаем сцену, TDPlayer сохраняется в Player.Instance, но Player.Instance - это переменная типа Player,
            //поэтому просто так достать нельзя, когда он достаётся, он забывает, что он - TDPlayer. 
            //но мы ему напоминаем, что он - TDPlayer вот этой строкой: Player.Instance as TDPlayer.
            //после чего мы меняем на нём золото.
        }
        public void TakeDamage(int damage, TDProjectile.DamageType damageType)
        {
            m_destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_armor));
            //В этой строчке: мы будем получать повреждения ApplyDamage. Но повреждение будет рассчитываться по 
            //функциям брони. А функции будут храниться в некотором массиве [m_ArmorType], к кторому мы будем
            //иметь доступ по m_ArmorType. А ArmorType под капотом у нас целые числа (то есть enum).
            //СМЫСЛ В ТОМ, что мы сейчас делаем массив функций, которые будут возвращать кол-во полученных нами повреждений,
            //в зависимости от damage, damageType и m_armor.
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
            if (a)
            {
                (target as Enemy).Use(a);
            }
        }
    }

#endif
}
