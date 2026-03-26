using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public abstract class TowerUpgrade : ScriptableObject
    {
        public virtual void Apply(Tower tower, int level) { }
        public virtual void ApplyPlayer(Player player, int level) { }
    }
}

