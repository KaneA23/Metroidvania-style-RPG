using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePush : MonoBehaviour
{
    public AISetUp AISU;
    private EnemyHealth EH;

    public GameObject m_Player;
    private Collider2D[] m_Enemies;

    public float m_PushBackRadius;
    public float m_PushBackForce;

    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        EH = GetComponent<EnemyHealth>();

        m_Player = AISU.m_ActivePlayer;
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D otherObject = collision.collider;

        if (otherObject.gameObject == m_Player)
        {
            m_Enemies = Physics2D.OverlapCircleAll(m_Player.transform.position, m_PushBackRadius);

            PushBack();
        }
    }

    void PushBack()
    {
        foreach (Collider2D collider in m_Enemies)
        {
            if (collider.gameObject.layer == 3)
            {
                if (collider.gameObject.transform.position.x < m_Player.transform.position.x)
                {
                    collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.1f, 0f) * m_PushBackForce, ForceMode2D.Impulse);
                }
                else if (collider.gameObject.transform.position.x > m_Player.transform.position.x)
                {
                    collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.1f, 0f) * m_PushBackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(m_Player.transform.position, m_PushBackRadius);
    }
}
