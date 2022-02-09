using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyConfig : ScriptableObject
{
    public float m_Speed;
    public float m_Range;
    public float m_MaxDistance;

    [Header("Wandering")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

}
