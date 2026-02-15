using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    //[RequireComponent(typeof(CircleCollider2D))]
    public class BossSpeedAura : MonoBehaviour
    {
        public float speedMultiplier = 5f;
        private CircleCollider2D _auraCollider;

        public void Init(CircleCollider2D auraCollider)
        {
            _auraCollider = auraCollider;
            if (_auraCollider == null) return;
            _auraCollider.isTrigger = true;
        }

        internal void Init(Collider2D auraCollider)
        {
            throw new NotImplementedException();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var ship = other.GetComponentInParent<SpaceShip>();
            if (ship == null) return;

            ship.FreezeImmune = true;

            ship.AddSpeedAura(speedMultiplier);
            Debug.Log("ENTER AURA");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var ship = other.GetComponentInParent<SpaceShip>();
            if (ship == null) return;

            ship.FreezeImmune = false;

            ship.RemoveSpeedAura(speedMultiplier);
            Debug.Log("EXIT AURA");
        }
    }
}