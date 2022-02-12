using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWanderState : StateChange
{
    public EnemyConfig config;

    public GameObject m_Player;
    public GameObject m_Enemy;

    public Transform m_PlayerTransform;

    public NavMeshAgent m_Agent;

    public LayerMask Ground, Player;

    private bool move;

    Vector2 m_TargetPos;

    public void Enter(EnemyAgent agent)
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Enemy = GameObject.FindGameObjectWithTag("Enemy");

        m_Agent = m_Player.GetComponent<NavMeshAgent>();

        if(!config.walkPointSet)
        {
            SearchWalkPoint();
        }

        if(config.walkPointSet)
        {
            m_Agent.SetDestination(config.walkPoint);
        }

        Vector2 distanceToWalkPoint = m_Enemy.transform.position - config.walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            config.walkPointSet = false;
        }
    }

    public void Exit(EnemyAgent agent)
    {
        
    }

    public EnemyState GetId()
    {
        return EnemyState.Wandering;
    }

    public void Update(EnemyAgent agent)
    {
        config.playerInAttackRange = Physics.CheckSphere(m_Enemy.transform.position, config.attackRange, Player);
        config.playerInSightRange = Physics.CheckSphere(m_Enemy.transform.position, config.sightRange, Player);

        if(move)
        {
            if(!config.playerInSightRange && !config.playerInAttackRange)
            {
                
            }
        }
    }

    void SearchWalkPoint()
    {

    }
}
