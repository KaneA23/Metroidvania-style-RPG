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
    //thing
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == m_Player)
        {
            if(!m_Entered)
            {
                m_Entered = true;

                PHS.TakeDamage(TV.fireTrapDamage, gameObject.transform.position);

                switch(m_FireTrap.tag)
                {
                    case "TrapRight":
                        m_PlayerBody.AddForce(new Vector2(1, 0) * TV.fireTrapForce, ForceMode2D.Impulse);
                        break;
                    case "TrapLeft":
                        m_PlayerBody.AddForce(new Vector2(-1, 0) * TV.fireTrapForce, ForceMode2D.Impulse);
                        break;
                    case "TrapBelow":
                        m_PlayerBody.AddForce(new Vector2(0, 1) * TV.fireTrapForce, ForceMode2D.Impulse);
                        break;
                    case "TrapAbove":
                        m_PlayerBody.AddForce(new Vector2(0, -1) * TV.fireTrapForce, ForceMode2D.Impulse);
                        break;
                }
            }         
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == m_Player)
        {
            if (m_endTime <= 0.02f)
            {
                PHS.TakeDamage(TV.fireTrapDamage, gameObject.transform.position);

                switch (m_FireTrap.tag)
                {
                    case "TrapRight":
                        m_PlayerBody.AddForce(new Vector2(1, 0) * TV.fireTrapForce, ForceMode2D.Impulse);
                        break;
                    case "TrapLeft":
                        m_PlayerBody.AddForce(new Vector2(-1, 0) * TV.fireTrapForce, ForceMode2D.Impulse);
                        break;
                    case "TrapBelow":
                        m_PlayerBody.AddForce(new Vector2(0, 1) * TV.fireTrapForce, ForceMode2D.Impulse);
                        break;
                    case "TrapAbove":
                        m_PlayerBody.AddForce(new Vector2(0, -1) * TV.fireTrapForce, ForceMode2D.Impulse);
                        break;
                }
            }
        }
    }
}
