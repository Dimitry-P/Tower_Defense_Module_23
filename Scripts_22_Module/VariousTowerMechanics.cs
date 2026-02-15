//using SpaceShooter;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TowerDefense;
//using UnityEngine;
//using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.RuleTile.TilingRuleOutput;

//namespace TowerDefense
//{
//    public class VariousTowerMechanics : MonoBehaviour
//    {
//        public Destructible target;
//        private Tower tower;
//        private string nameOfTower;
//        private Turret[] turrets;
//        private Projectile projectile; 
        

//        private void Start()
//        {
//            turrets = GetComponentsInChildren<Turret>();
//        }


//        public void ApplyTowersMechanics(Tower specificTower, string towerName)
//        {
//            tower = specificTower;
//            nameOfTower = towerName;
//        }

//        public void Tower_UseSpecificMechanic(Destructible destructible)
//        {
//            target = destructible;
//            if (nameOfTower == "dps")
//            {
//                if (target != null)
//                {
//                    if (target.isDead)
//                    {
//                        target.isDead = false;
//                        target = null;
//                        return;
//                    }
//                    else
//                    {
                       
//                    }
//                }
//            }
//            //if (nameOfTower == "dot")
//            //{
//            //    if (target != null)
//            //    {
//            //        if (target.TargetWasHitWithPoison)
//            //        {
//            //            target.isDead = false;
//            //            target = null;
//            //            return;
//            //        }
//            //        else
//            //        {
//            //            Vector2 targetVector = target.transform.position - transform.position;
//            //            Enemy enemy = target.GetComponent<Enemy>();
//            //            Debug.Log(enemy.enemyName);
//            //            if (targetVector.magnitude <= tower.Radius)
//            //            {
//            //                foreach (var turret in turrets)
//            //                {
//            //                    turret.transform.up = targetVector;
//            //                    turret.Fire();
//            //                }
//            //            }
//            //        }
//            //    }
//            //}
//        }
//        public void EnemyIsDead()
//        {
//            target.isDead = true;
//        }
//        public void EnemyIsPoisoned()
//        {

//        }
//    }
//}
