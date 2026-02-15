using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public abstract class LevelCondition : MonoBehaviour
    {
        public virtual bool IsCompleted {  get; }    
    }
}

