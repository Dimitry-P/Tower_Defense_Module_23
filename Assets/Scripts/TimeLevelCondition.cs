using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

public class TimeLevelCondition : MonoBehaviour, ILevelCondition
{
    [SerializeField] private float timeLimit;
    private void Start()
    {
        timeLimit += Time.time;
    }
    public bool IsCompleted => Time.time > timeLimit;
}
