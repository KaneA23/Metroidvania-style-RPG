using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFire : MonoBehaviour
{
    private AISetUp AISU;
    private PlayerHealthSystem PHS;

    private GameObject m_Player;

    private Vector2 m_AttackDirection;

    private float m_TimeInitialized;
    private float m_ElapsedTime;
    public float m_FireLife;
    public float m_AttackForce;

    public int m_DamageAmount;


    void Awake()
    {
        m_TimeInitialized = Time.time;
    }

    private void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;

        PHS = m_Player.GetComponent<PlayerHealthSystem>();
    }

    void FixedUpdate()
    {
        m_ElapsedTime = Time.time - m_TimeInitialized;

        if (m_ElapsedTime > m_FireLife)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);

        if (collision.gameObject == m_Player)
        {
            Debug.Log("Ouchie");

            PHS.TakeDamage(m_DamageAmount, enemyPos, m_AttackForce, true);

            float heightForce = Random.Range(0.5f, 1f);

            if (m_Player.transform.position.x < transform.position.x)
            {
                m_AttackDirection = new Vector2(-1f, heightForce) * m_AttackForce;
            }
            else if (m_Player.transform.position.x > transform.position.x)
            {
                m_AttackDirection = new Vector2(1f, heightForce) * m_AttackForce;
            }
            else
            {
                m_AttackDirection = new Vector2(0f, heightForce) * m_AttackForce;
            }
        }
    }
}
