using SpaceShooter;
using System;
using TowerDefense;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;


namespace TowerDefense
{
    [RequireComponent(typeof(TDController))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int m_damage;
        public int m_gold;
        [SerializeField] private AuraGizmos m_auraGizmos;
        public string enemyName;
        public Projectile projectile;
       

        public void Use(EnemyAsset asset)//эта функция осущ-ет подцепление настроек для врагов.
        {
           
            var destructible = gameObject.GetComponent<Destructible>();

            destructible.IsBoss = asset.isBoss;

            if(destructible.IsBoss == true &&  SceneManager.GetActiveScene().name == "Level_2")
            {
                var speedAura = destructible.AddComponent<BossSpeedAura>();
                m_auraGizmos.Init();
                speedAura.Init(m_auraGizmos.AuraCollider);
            }
            if(destructible.IsBoss == false || destructible.IsBoss == true &&
                SceneManager.GetActiveScene().name == "Level_1") m_auraGizmos.Disabled();

            var sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
              
            sr.color = asset.color;
            sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);
            
            sr.sprite = asset.sprite;

            //Для того, чтобы можно было переключать ассеты в инспекторе префаба, нужно
            //закомментить передачу анимации:
            //Анимация перекрывает спрайт, поэтому тут мы её не передаём
            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animations;

            GetComponent<SpaceShip>().Use(asset);

            GetComponentInChildren<CircleCollider2D>().radius = asset.radius;

            m_damage = asset.damage;
            //m_gold = projectile.m_Damage;
            enemyName = asset.nameOfEnemy;
        }

        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_damage, enemyName);
        }
        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(m_gold);
            //(Player.Instance as TDPlayer).ChangeGold(m_gold);//достань игрока, представь его в виде класса TDPlayer и измени на нём золото.
            //Т.е. когда запускаем сцену, TDPlayer сохраняется в Player.Instance, но Player.Instance - это переменная типа Player,
            //поэтому просто так достать нельзя, когда он достаётся, он забывает, что он - TDPlayer. 
            //но мы ему напоминаем, что он - TDPlayer вот этой строкой: Player.Instance as TDPlayer.
            //после чего мы меняем на нём золото.
        }
    }
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
}
