using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    public AISetUp AISU;
    public PlayerHealthSystem PHS;

    public GameObject m_Player;

    public Rigidbody2D m_PlayerBody;

    public float m_AttackForce;
    public int m_DamageAmount;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.GetComponent<AISetUp>().m_ActivePlayer;

        PHS = m_Player.GetComponent<PlayerHealthSystem>();

        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D otherObject = collision.collider;

        if (otherObject.gameObject == m_Player)
        {

            PHS.TakeDamage(m_DamageAmount);

            float heightForce = Random.Range(0.5f, 1f);

            if (m_Player.transform.position.x < transform.position.x)
            {
                m_PlayerBody.AddForce(new Vector2(-1f, heightForce) * m_AttackForce, ForceMode2D.Impulse);
            }
            else if (m_Player.transform.position.x > transform.position.x)
            {
                m_PlayerBody.AddForce(new Vector2(1f, heightForce) * m_AttackForce, ForceMode2D.Impulse);
            }
            else
            {
                m_PlayerBody.AddForce(new Vector2(0f, heightForce) * m_AttackForce, ForceMode2D.Impulse);
            }

        }
    }
}
