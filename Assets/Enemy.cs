using TowerDefense;
using UnityEngine;
using SpaceShooter;
using UnityEditor;
using UnityEngine.UIElements;


namespace TowerDefense
{
    [RequireComponent(typeof(TDController))]
    public class Enemy : MonoBehaviour
    {
        public void Use(EnemyAsset asset)//эта функция осущ-ет подцепление настроек для врагов.
        {
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
