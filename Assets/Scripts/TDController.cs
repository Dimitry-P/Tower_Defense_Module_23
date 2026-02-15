using SpaceShooter;
using UnityEngine;
using UnityEngine.Events;


namespace TowerDefense
{
    public class TDController : AIController
    {
        private Path m_path;
        private int pathIndex;
        [SerializeField] private UnityEvent TakeDamage;

        public void SetPath(Path newPath)
        {
            m_path = newPath;
            pathIndex = 0;
            SetPatrolBehaviour(m_path[pathIndex]);
        }
        protected override void GetNewPoint()
        {
            pathIndex += 1;
            if(m_path.Length > pathIndex)
            {
                SetPatrolBehaviour(m_path[pathIndex]);
            }
            else
            {
                TakeDamage?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
