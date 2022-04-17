using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BernardStates : MonoBehaviour
{
    private AISetUp AISU;
    public BernardAttacking BA;
    public BernardIdle BI;

    private GameObject m_Player;

    private float m_DistanceToPlayer;
    private int state;

    public enum states
    {
        Idle,
        Attacking,
    }

    public static string[] stateTypes = { "Idle_1", "Idle_2", "Idle_3", "Attacking_1", "Attacking_2", "Attacking_3"};

    private void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;
    }

    private void Update()
    {
        m_DistanceToPlayer = Vector2.Distance(gameObject.transform.position, m_Player.transform.position);

        if (m_DistanceToPlayer < 5f)
        {
            state = (int)states.Attacking;
        }
        else
        {
            state = (int)states.Idle;
        }

        switch (state)
        {
            case 0:
                Debug.Log("Idle");
                break;
            case 1:
                Debug.Log("Attacking");
                break;
        }
    }
}
