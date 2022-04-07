using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private AISetUp AISU;
    private PlayerHealthSystem PHS;

    private GameObject m_Player;
    public GameObject m_Cannon;

    private Collider2D m_CannonCollider;

    private Rigidbody2D m_PlayerBody;

    public int m_DamageAmount;

    private float m_Force;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        PHS = AISU.PHS;

        m_Player = AISU.m_ActivePlayer;
        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();

        m_CannonCollider = gameObject.GetComponent<Collider2D>();

        m_Force = GameObject.Find("EventSystem").GetComponent<TrapValues>().cannonForce;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D otherObject = collision.collider;

        if (otherObject.gameObject.CompareTag("CannonRight") ||
            otherObject.gameObject.CompareTag("CannonLeft") ||
            otherObject.gameObject.CompareTag("CannonAbove"))
        {
            Physics2D.IgnoreCollision(m_CannonCollider, otherObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == m_Player)
        {
            PHS.TakeDamage(m_DamageAmount, gameObject.transform.position, m_Force, true);
            if (collision.gameObject.transform.position.x > gameObject.transform.position.x)
            {
                m_PlayerBody.AddForce(new Vector2(1, 0));
            }
            else
            {
                m_PlayerBody.AddForce(new Vector2(-1, 0));
            }
        }
    }
}
