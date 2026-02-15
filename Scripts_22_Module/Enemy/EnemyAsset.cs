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
        public string nameOfEnemy;

        [Header("Игровые параметры")]
        public float moveSpeed;
        public bool isBoss;
        public int hp;
        public float radius = 0.19f;
        public int damage = 1;
        public int gold;
    }
}
//Я полностью расписал цепочку создания и появления в сцене разных врагов. Вот она: 
//1.Есть скрипт EnemyAsset(ScriptableObject).
//2.Создаю нужное кол-во SO  типа EnemyAsset.
//3. Внутри каждого SO вложен спрайт врага.
//4. В скриптах EnemySpawner, которые висят на объекте иерархии EnemyCamp,
//инстанциирую общий префаб врага (типа Enemy), который вложен в компонент EnemySpawner.
//5. В EnemySpawner также вложен ассет (SO конкретного врага).
//6. В скриптах EnemySpawner для инстанциированного префаба вызываю метод Use 
//из класса Enemy (поскольку префаб имеет тип Enemy), в аргументе передаю
//ассет.
//7. И уже в классе Enemy в методе Use достаю через .Find дочерний SpriteRenderer префаба
//и для поля sprite у SpriteRenderer-а задаю спрайт из ассета.