using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireAttacks : MonoBehaviour
{
    public EnemyPathfindingNew EPN;

    public GameObject m_Fire;

    private Rigidbody2D rb;

    private Vector3 m_InitPos;
    private Vector2 m_PosToSpawn;

    private float m_DistanceTravelled;
    private float m_FireLife;

    private bool CR_RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        m_InitPos = transform.position;
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdateInitPos", 0f, 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics2D.IgnoreLayerCollision(3, 8);
        //Physics2D.IgnoreLayerCollision(6, 8);
        Physics2D.IgnoreLayerCollision(8, 8);

        if (rb.velocity.x > 0f)
        {
            m_PosToSpawn = new Vector2(transform.position.x - 0.2f, -3.926f);
        }
        else if (rb.velocity.x < 0f)
        {
            m_PosToSpawn = new Vector2(transform.position.x + 0.2f, -3.926f);
        }

        m_DistanceTravelled = Vector3.Distance(transform.position, m_PosToSpawn);

        //if (m_DistanceTravelled >= 0.7f && m_DistanceTravelled <= 0.9)
        //{
        //    if (rb.velocity.x > 0f)
        //    {
        //        Instantiate(m_Fire, new Vector2(transform.position.x - 0.2f, -3.926f), Quaternion.identity);
        //    }
        //    else if (rb.velocity.x < 0f)
        //    {
        //        Instantiate(m_Fire, new Vector2(transform.position.x + 0.2f, -3.926f), Quaternion.identity);
        //    }
        //}
    }
    //thing
    private void UpdateInitPos()
    {
        Vector3 m_CurrentPos = transform.position;

        if(m_CurrentPos.x >= m_CurrentPos.x + m_DistanceTravelled || transform.position.x >= m_CurrentPos.x + -m_DistanceTravelled)
        {
            Instantiate(m_Fire, transform.position, Quaternion.identity);
        }
        //m_InitPos = transform.position;
    }
}
