using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public class TDProjectile : Projectile
    {
        public enum DamageType { Base, Magic }  //Базовый и магический тип переменной
        [SerializeField] private DamageType m_DamageType;
        [SerializeField] private Sound m_ShotSound = Sound.Arrow;
        [SerializeField] private Sound m_HitSound = Sound.Arrowhit;

        private void Start()
        {
            m_ShotSound.Play();
        }

        protected override void OnHit(RaycastHit2D hit)
        {
            m_HitSound.Play();
            var enemy = hit.collider.transform.root.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(m_Damage, m_DamageType);
            }
        }
    }
}
