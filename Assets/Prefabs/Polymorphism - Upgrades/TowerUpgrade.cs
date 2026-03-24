using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDefense
{
    public abstract class TowerUpgrade : ScriptableObject
    {
        public abstract void Apply(Tower tower, int level);
    }
}

