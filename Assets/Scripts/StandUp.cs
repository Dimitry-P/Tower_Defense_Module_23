using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Скрипт постобратботки:  после того,
//как все движения в кадре произошли,
//после этого мы хотим выровнять наш спрайт вверх.
//чтобы происходили движения в кадре
//после всех отсальных движений в кадре, нужно
//использовать метод LateUpdate
namespace TowerDefense
{
    public class StandUp : MonoBehaviour
    {
        private Rigidbody2D rig;
        private SpriteRenderer sr;//отвечает за то куда повёрнут наш персонаж

        private void Start()
        {
            rig = transform.root.GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
        }
        private void LateUpdate()
        {
            transform.up = Vector2.up;
            var xMotion = rig.velocity.x;
            if (xMotion > 0.01f) sr.flipX = false;
            else if (xMotion < -0.01f) sr.flipX = true;
        }
    }
}

