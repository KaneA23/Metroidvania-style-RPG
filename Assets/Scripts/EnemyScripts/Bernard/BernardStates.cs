using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BernardStates : MonoBehaviour
{
    private AISetUp AISU;
    public BernardAttacking BA;
    public BernardIdle BI;

    private GameObject m_Player;

    private float m_DistanceToPlayer;
    public float m_AttackDistance;
    public float m_Health;
    private float m_MaxHealth;
    private float m_HealthPercentage;
    [SerializeField] private int m_HealthPercentageRounded;

    private int state;

    [SerializeField] private bool m_Attacking;

    public enum states
    {
        Idle,
        Attacking,
    }

    public string[] IdleStateTypes = { "Idle_1", "Idle_2", "Idle_3" };
    public string[] AttackStateTypes = { "Attacking_1", "Attacking_2", "Attacking_3" };

    private void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;

        m_MaxHealth = GetComponent<EnemyHealth>().m_MaxHP;
        m_Health = m_MaxHealth;
    }

    private void Update()
    {
        m_DistanceToPlayer = Vector2.Distance(gameObject.transform.position, m_Player.transform.position);
        m_Health = GetComponent<EnemyHealth>().m_CurrentHP;

        if (m_DistanceToPlayer < m_AttackDistance)
        {
            state = (int)states.Attacking;
        }
        else
        {
            state = (int)states.Idle;
        }

        m_HealthPercentage = (m_MaxHealth * 33.33f) / 100;
        m_HealthPercentageRounded = (int)Math.Round(m_HealthPercentage, 0);

        switch (state)
        {
            case 0:
                m_Attacking = false;

                if (m_Health > 0 && m_Health < m_HealthPercentageRounded)
                {
                    BI.Invoke(IdleStateTypes[2], 0f);
                }
                else if (m_Health >= m_HealthPercentageRounded && m_Health < m_HealthPercentageRounded * 2)
                {
                    BI.Invoke(IdleStateTypes[1], 0f);
                }
                else if (m_Health >= m_HealthPercentageRounded * 2 && m_Health <= m_MaxHealth)
                {
                    BI.Invoke(IdleStateTypes[0], 0f);
                }
                
                break;
            case 1:
                m_Attacking = true;

                if (m_Health > 0 && m_Health < m_HealthPercentageRounded)
                {
                    BA.Invoke(AttackStateTypes[2], 0f);
                }
                else if (m_Health >= m_HealthPercentageRounded && m_Health < m_HealthPercentageRounded * 2)
                {
                    BA.Invoke(AttackStateTypes[1], 0f);
                }
                else if (m_Health >= m_HealthPercentageRounded * 2 && m_Health <= m_MaxHealth)
                {
                    BA.Invoke(AttackStateTypes[0], 0f);
                }

                break;
        }
    }
}
