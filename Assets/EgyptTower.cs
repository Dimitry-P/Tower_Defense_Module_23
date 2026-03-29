using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EgyptTower : Tower
    {
        [SerializeField] private float triggerRadius = 5f;

        private int charges; // количество выстрелов

        // Устанавливаем количество выстрелов по уровню апгрейда
        private int maxCharges;

        public void SetUpgradeLevel(int level)
        {
            maxCharges = level;
            charges = maxCharges;
            enabled = charges > 0;
        }

        private void Update()
        {
            if (charges <= 0) return;

            // Проверяем, есть ли враги на сцене
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            if (allEnemies.Length == 0) return;

            // Проверяем, есть ли враг в радиусе
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, triggerRadius);
            foreach (var hit in hits)
            {
                Enemy enemy = hit.transform.root.GetComponent<Enemy>(); // на случай, если коллайдер в дочернем объекте
                if (enemy != null)
                {
                    DestroyAllEnemies(allEnemies);
                    break;
                }
            }
        }

        private void DestroyAllEnemies(Enemy[] allEnemies)
        {
            foreach (var enemy in allEnemies)
            {
                if (enemy != null)
                    Destroy(enemy.gameObject);
            }

            charges--;

            if (charges < 0)
                enabled = false;
        }
    }
}