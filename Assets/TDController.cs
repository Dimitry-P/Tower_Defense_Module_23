using UnityEngine;
using SpaceShooter;


namespace TowerDefense
{
    public class TDController : AIController
    {
        private Path m_path;
        private int pathIndex;
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
                Destroy(gameObject);
            }
        }
    }
}
