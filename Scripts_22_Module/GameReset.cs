using SpaceShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public static class GameReset
    {
        public static void ResetStatics()
        {
            TDPlayer.Reset();
            // добавишь сюда другие менеджеры
        }
    }
}
