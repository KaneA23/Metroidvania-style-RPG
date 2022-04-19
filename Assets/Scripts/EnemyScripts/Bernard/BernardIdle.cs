using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BernardIdle : MonoBehaviour
{
    public BernardAI BAI;

    private Vector3 m_IdlePos;
    private Vector2 m_ReturnDir;

    private Rigidbody2D rb;

    public float m_ReturnSpeed;
    private float m_OrigSpeed;
    private float m_SlowDelayTime = 3f;

    private bool m_Returned;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        m_IdlePos = transform.position;

        m_OrigSpeed = GetComponent<BernardIdle>().m_ReturnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        m_ReturnDir = (m_IdlePos - transform.position).normalized;

        if (m_Returned)
        {
            m_SlowDelayTime--;
        }
        else
        {
            m_SlowDelayTime = 3f;
        }

        if (m_SlowDelayTime == 0f)
        {
            m_SlowDelayTime = 3f;
        }
    }

    void Idle_1()
    {
        if (transform.position != m_IdlePos)
        {
            rb.AddForce(m_ReturnDir * m_ReturnSpeed);
            m_Returned = false;
        }
        else if (transform.position == m_IdlePos)
        {
            m_Returned = true;
            rb.velocity = Vector2.zero;

            if (m_SlowDelayTime > 0f)
            {
                m_ReturnSpeed = 0f;
            }
            else
            {
                m_ReturnSpeed = m_OrigSpeed;
            }   
        }
    }

    void Idle_2()
    {

    }

    void Idle_3()
    {

    }
}
