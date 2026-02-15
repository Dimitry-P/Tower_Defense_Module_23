using SpaceShooter;
using System.Collections;
using TowerDefense;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Towers.std
{
    public class VariousTowerMechanicsBossTower : VariousMech
    {
        [Header("Boss Tower Settings")]
        public int baseDamage;          // базовый урон обычному врагу
        public int bossDamageMultiplier = 5; // урон для босса

        [Header("Visual Effect")]
        public float scaleMultiplier = 1.5f;
        public float scaleDuration = 20f;
        private float m_Radius;

        public override void TryApplyDamage(Destructible destructible)
        {
            if (destructible == null) return;
            //if (destructible.IsPoisoned == true) return;
            int damageToApply = baseDamage;
           
            destructible.ApplyDamage(damageToApply, this);
            StartCoroutine(ScaleEnemyTemporary(destructible));
            //destructible.IsPoisoned = true;
            //destructible.PoisonSource = this;
        }

        private IEnumerator ScaleEnemyTemporary(Destructible destructible)
        {
            if (destructible == null) yield break;

            var sprite = destructible.GetComponentInChildren<SpriteRenderer>();
            if (sprite == null) yield break;

            Transform visual = sprite.transform;

            Vector3 originalScale = visual.localScale;
            visual.localScale = originalScale * scaleMultiplier;

            yield return new WaitForSeconds(scaleDuration);

            if (visual != null)
                visual.localScale = originalScale;
        }

        public override void OnEnemyKilled()
        {
            base.OnEnemyKilled();
        }

        public override void UseSpecificMechanic(TurretProperties turretProperties)
        {
            baseDamage = turretProperties.Damage;
            m_Radius = tower.Radius;
        }
    }
}
