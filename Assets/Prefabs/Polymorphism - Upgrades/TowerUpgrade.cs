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
        public virtual void ApplyPlayer(int level) { }
        public virtual void Egypt_Tower_Apply(Tower tower, int level) { }
    }
}

