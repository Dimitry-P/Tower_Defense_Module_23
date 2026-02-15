using UnityEngine;
using SpaceShooter;


namespace TowerDefense
{
    public class TDLevelController : LevelController
    {
        private new void Start()
        {
            base.Start();
            TDPlayer.Instance.OnPlayerDead += () =>
            {
                StopLevelActivity();
                LevelResultController.Instance.Show(false);
            };
        }
        private void StopLevelActivity()
        {
            Debug.Log("Level Stopped");
        }
    }
}
