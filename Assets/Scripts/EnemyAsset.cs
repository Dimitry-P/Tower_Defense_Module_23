using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    

    [CreateAssetMenu]
    public sealed class EnemyAsset : ScriptableObject
    {
        [Header("Внешний вид")]
        public Sprite sprite;
        public Color color = Color.white;
        public Vector2 spriteScale = new Vector2(3, 3);
        public RuntimeAnimatorController animations;

        [Header("Игровые параметры")]
        public float moveSpeed = 1f;
        public int hp = 1;
        public int score = 1;
        public float radius = 0.19f;
        public int damage;
        public string enemyName;
    }
}