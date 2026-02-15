using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerDefense;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Towers.std
{
    public class VariousTowerMechanicsSlowDownTower : VariousMech
    {
        private int baseDamage;

        public float duration = 5f;     // на 2 секунды

        public override void TryApplyDamage(Destructible destructible)
        {
            Debug.Log("Enemy goes slower");
            if (destructible == null) return;
            
            var ship = destructible.GetComponent<SpaceShip>();
            //float initialSpeed = ship.MaxLinearVelocity;
            //ship.MaxLinearVelocity *= 0.1f;
            //StartCoroutine(RemoveAfterTime(ship, duration, destructible, initialSpeed));
            //destructible.IsPoisoned = true;
            if (ship == null) return;

            ship.ApplySlow(0.3f, duration);

            destructible.IsPoisoned = true;
            destructible.ApplyDamage(baseDamage, this);
        }

        //private IEnumerator SlowCoroutine(SpaceShip ship, float duration)
        //{
        //    ship.SetSpeedMultiplier(0.3f, 5f);

        //    yield return new WaitForSeconds(duration);

        //    //if (ship != null)
        //    //    ship.SetSpeedMultiplier(1f);
        //}


        //private IEnumerator SlowCoroutine(SpaceShip ship, float duration, Destructible destructible)
        //{
        //    float originalSpeed = ship.MaxLinearVelocity;
        //    ship.MaxLinearVelocity *= 0.1f;

        //    yield return new WaitForSeconds(duration);

        //    if (ship != null)
        //        ship.MaxLinearVelocity = originalSpeed;

        //    if (destructible != null)
        //        destructible.IsPoisoned = false;
        //}

        //private IEnumerator RemoveAfterTime(SpaceShip ship, float duration, Destructible destructible, float initialSpeed)
        //{
        //    yield return new WaitForSeconds(duration);
        //    if (ship != null) ship.MaxLinearVelocity = initialSpeed;
        //    destructible.IsPoisoned = false;
        //}

        public override void UseSpecificMechanic(TurretProperties turretProperties)
        {
            baseDamage = turretProperties.Damage;
        }
    }
}
