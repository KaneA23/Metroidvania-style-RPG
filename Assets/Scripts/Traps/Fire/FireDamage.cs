using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    private AISetUp AISU;
    private PlayerHealthSystem PHS;
    private TrapValues TV;

    private GameObject m_Player;
    public GameObject m_FireTrap;

    private Rigidbody2D m_PlayerBody;

    private float m_endTime = 0;
    private float m_restartTime;

    private bool m_Entered = false;

    private void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        TV = GameObject.Find("EventSystem").GetComponent<TrapValues>();

        PHS = AISU.PHS;

        m_Player = AISU.m_ActivePlayer;
        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();

        m_endTime = TV.fireTrapDamageFrequency;
        m_restartTime = m_endTime;
    }

    private void Update()
    {
        m_endTime -= Time.deltaTime;

        if (m_endTime <= 0f)
        {
            m_endTime = m_restartTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == m_Player)
        {
            if(!m_Entered)
            {
                m_Entered = true;

                PHS.TakeDamage(TV.fireTrapDamage, gameObject.transform.position, TV.fireTrapForce, false);
            }         
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == m_Player)
        {
            m_Entered = false;
        }   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == m_Player)
        {
            if (m_endTime <= 0.02f)
            {
                Debug.Log("Here");

                PHS.TakeDamage(TV.fireTrapDamage, gameObject.transform.position, TV.fireTrapForce, false);
            }
        }
    }
}
