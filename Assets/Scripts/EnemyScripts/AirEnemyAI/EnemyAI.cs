using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wandering,
    Chasing,
    Attacking
}

public interface StateChange
{
    EnemyState GetId();
    void Enter(EnemyAgent agent);
    void Update(EnemyAgent agent);
    void Exit(EnemyAgent agent);
}
