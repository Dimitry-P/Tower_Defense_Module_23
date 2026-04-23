using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        private float m_Lead = 0.3f;
        private Turret[] turrets;
        private Rigidbody2D target = null; //RigidBody2D знает всю свою физику, включая направление движения

        public void Use(TowerAsset asset)
        {
            //Через эту функцию Делаю Применение НАСТРОЕК БАШНИ целиком
            GetComponentInChildren<SpriteRenderer>().sprite = asset.sprite;
            turrets = GetComponentsInChildren<Turret>();
            foreach(var turret in turrets)
            {
                turret.AssignLoadout(asset.turretProperties);
            }
            GetComponentInChildren<BuildSite>().SetBuildableTowers(asset.m_UpgradesTo);
        }

        private void Start()
        {
            // Сразу прмиенить апргрейды
            ApplyAllUpgrades();
        }

        public void ModifyRadius(float levelNumber)
        {
            m_Radius *= levelNumber;
        }

        private void ApplyAllUpgrades()
        {
            if (Upgrades.Instance == null || Upgrades.Instance.save == null) return;

            foreach (var savedUpgrade in Upgrades.Instance.save)
            {
                if (savedUpgrade.upgradeSO != null) // upgradeSO название поля, может отличаться
                {
                    savedUpgrade.upgradeSO.Apply(this, savedUpgrade.level);
                    //В ЭТОЙ СТРОКЕ ПРОИСХОДИТ МАГИЯ!!!
                    //Проговорю всю цепочку действий:
                    //1.Игрок нажимает кнопку Buy.
                    //2.Внутри объекта Radius_Upgrade срабатывает Button и вызывается скрипт BuyUpgrade метод Buy.
                    //3.Внутри метода Buy вызывается Upgrades.BuyUpgrade(asset) и передаётся перетащенный в инспектор SO.
                    //4.Затем вызывается Initialize(); в savedLevel возвращается выбранный уровень апгрейда.
                    //5.Игрок жмёт на кнопку начала уровня. Уровень открывается.
                    //6.Выполняется скрипт Tower. Вызывается метод ApplyAllUpgrades();
                    //7.Достаётся из save нужный экземпляр класса апгрейда.
                    //8.У этого экземпляра смотрится вот это поле: TowerUpgrade upgradeSO
                    //9.Массив save находится внутри класса Upgrades, который висит на объекте MapLevelController
                    //и в каждом поле элемента массива находится конкретный SO собственного типа,
                    //но все они наследуются от типа TowerUpgrade.
                    //10.И для данного дочернего типа, которым является данный SO, вызывается метод
                    //savedUpgrade.upgradeSO.Apply(this, savedUpgrade.level);
                    //11.Apply - это метод абстрактного(родительского) класса и поэтому
                    //нас перекидывает в этот же метод но override (переопределённый), в конкретный дочерний метод Apply.
                    //12. И таким образом получается, что, к примеру, мой SO типа RadiusUpgrade и перекидывает меня в
                    //метод Apply, который тоже находится в классе(скрипте) RadiusUpgrade

                    savedUpgrade.upgradeSO.Egypt_Tower_Apply(this, savedUpgrade.level);
                }
            }
        }

        private void Update()
        {
            if (target)
            {
                if (Vector3.Distance(target.transform.position, transform.position) <= m_Radius)
                {
                    foreach (var turret in turrets)
                    {
                        turret.transform.up = target.transform.position 
                            - turret.transform.position + (Vector3)target.velocity * 0.1f * m_Lead; //из цели вычитаем положение турели
                        turret.Fire();
                    }
                }
                else
                {
                    target = null;
                }
            }
            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);
                if (enter)
                {
                    target = enter.transform.root.GetComponent<Rigidbody2D>();
                }
            }
            Debug.Log(m_Radius+"РАДИУС");
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }
    }
}

