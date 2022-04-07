using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI : MonoBehaviour
{
    public PlayerHealthSystem PHS;
    public EnemyHealth EH;
    public AISetUp AISU;   

    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float m_HitForce;

    public Transform enemyGraphics;

    public int m_DamageAmount;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        PHS = AISU.PHS;

        target = AISU.m_ActivePlayer.transform;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D otherCollider = collision.collider;

        if(otherCollider.name == AISU.m_ActivePlayer.tag)
        {
            PHS.TakeDamage(m_DamageAmount, gameObject.transform.position, m_HitForce, true);
            //EH.TakeDamage(m_DamageAmount);

            if ((transform.position.x - otherCollider.transform.position.x) < 0)
            {
                Debug.Log("Left Hit");
                rb.AddForce(new Vector2(-1f, 0f) * m_HitForce);
            }
            else if ((transform.position.x - otherCollider.transform.position.x) > 0)
            {
                Debug.Log("Right Hit");
                rb.AddForce(new Vector2(1f, 0f) * m_HitForce);
            }
        }

        if (otherCollider.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(otherCollider, GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            enemyGraphics.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else if (force.x <= -0.01f)
        {
            enemyGraphics.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

    }
}
