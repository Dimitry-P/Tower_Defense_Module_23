using SpaceShooter;
using TowerDefense;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Towers.std
{
    public class VariousTowerMechanicsAoETower : VariousMech
    {
        private int baseDamage;

        private int enemyLayerMask;
        private float m_Radius;

        private void Awake()
        {
            enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        }

        public override void TryApplyDamage(Destructible destructible)
        {
            if (destructible.IsBoss == true) return;
            //Поиск всех врагов в радиусе от точки попадания
            Debug.Log("Enemy layer mask: " + enemyLayerMask);
            Collider2D[] enemies = Physics2D.OverlapCircleAll(
    destructible.transform.position,
    m_Radius,
    enemyLayerMask
);
            
            foreach (Collider2D col in enemies)
            {
                Destructible destr = col.GetComponentInParent<Destructible>();
                if (destr != null)
                {
                    Vector2 targetVector = destr.transform.position - transform.position;
                    if (targetVector.magnitude > m_Radius)continue;
                    destr.ApplyDamage(baseDamage, this);
                }
            }
        }
        public override void UseSpecificMechanic(TurretProperties turretProperties)
        {
            baseDamage = turretProperties.Damage;
            m_Radius = tower.Radius;
        }
    }
}
