using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAICurrent : MonoBehaviour
{
    public GameObject m_Player;
    public Transform m_PlayerTransform;

    public NavMeshAgent m_Agent;

    public LayerMask Ground, Player;

    private bool move;

    //Roaming
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    int m_Speed;

    public void Start()
    {
        m_Speed = 10;        
    }
    //thing
    private void Awake()
    {
        //GameStateManager.Instance.onGameStateChanged += OnGameStateChanged;
        //OnGameStateChanged(GameStateManager.Instance.CurrentGameState);

        m_Player = GameObject.Find("Player");
        m_PlayerTransform = m_Player.transform;
        m_Agent = GetComponent<NavMeshAgent>();       
    }

    private void OnDestroy()
    {
        //GameStateManager.Instance.onGameStateChanged -= OnGameStateChanged;
    }

    //private void OnGameStateChanged(GameState newGameState)
    //{
    //    switch (newGameState)
    //    {
    //        case GameState.Gameplay:
    //            move = true;
    //            break;
    //        case GameState.Paused:
    //            move = false;
    //            break;
    //    }
    //}

    void Patrolling()
    {
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            m_Agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void ChasePlayer()
    {
        m_Agent.SetDestination(m_PlayerTransform.position);
    }

    void AttackPlayer()
    {

    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
        {
            walkPointSet = true;
        }
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);

        if(move)
        {
            if (!playerInSightRange && !playerInAttackRange)
            {
                Patrolling();
            }
            else if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            else if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }
        }
        

        //Vector3 dir = (m_Player.transform.position - transform.position).normalized;

        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.MovePosition(rb.transform.position + dir * m_Speed * Time.deltaTime);
    }
}
