using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

public class AIRanged : MonoBehaviour
{
    [Header("Script References")]
    public PlayerHealthSystem PHS;
    public EnemyHealth EH;
    public AISetUp AISU;
    public EnemyAnimationManager EAM;

    [Header("Assigned Objects")]
    public GameObject m_Player;
    public GameObject m_DeadEarthElementalPrefab;
    private GameObject m_DeadEarthElemental;
    [Space]
    private Transform[] m_EarthElementalSpikes;
    public Transform target;
    public Transform enemyGraphics;
    [Space]
    public LayerMask m_GroundMask;

    [Header("Changable Values")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float m_AttackDistance;
    public float m_HitForce;
    public int m_DamageAmount;


    private Vector3 m_TargetPos;
    [SerializeField] private Vector3 m_HoverPos;
    public Vector3 m_HoverDistance;
    private float m_Distance;

    private int m_Timer = 0;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    [Header("Statuses")]
    public bool attacking = false;
    public bool isAlert;
    public bool isForget;
    public bool isAgro;
    public bool withinRange;
    private bool died = false;

    public float animDelay;

    Seeker seeker;

    private Rigidbody2D rb;
    private Rigidbody2D deadRb;

    private Collider2D coll;
    private Collider2D deadColl;

    private void Awake()
    {
        EAM = GetComponent<EnemyAnimationManager>();
    }

    // Start is called before the first frame update
    public void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        PHS = AISU.PHS;

        m_Player = AISU.m_ActivePlayer;

        target = m_Player.transform;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        coll = GetComponent<Collider2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);

        isAlert = false;
        isForget = false;
        isAgro = false;
        //EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_CALMWALK);

        Physics2D.IgnoreLayerCollision(6, 19);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, m_HoverPos, OnPathComplete);
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
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);

        if (otherCollider.name == m_Player.tag)
        {
            PHS.TakeDamage(m_DamageAmount, enemyPos, m_HitForce, true);
            //EH.TakeDamage(m_DamageAmount, enemyPos);

            if ((transform.position.x - otherCollider.transform.position.x) < 0)
            {
                Debug.Log("Left Hit");
                //rb.AddForce(new Vector2(-1f, 0f) * m_HitForce);
            }
            else if ((transform.position.x - otherCollider.transform.position.x) > 0)
            {
                Debug.Log("Right Hit");
                //rb.AddForce(new Vector2(1f, 0f) * m_HitForce);
            }
        }

        if (otherCollider.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(otherCollider, GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAgro && !isAlert && !EH.isDead)
        {
            EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_AGROWALK);
        }
        else if (!isAgro && !isForget && !EH.isDead)
        {
            EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_CALMWALK);
        }

        if (EH.isDead && !died)
        {
            EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_DYING);

            if (EH.animFinished)
            {
                coll.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                transform.Find("GFX").GetComponent<SpriteRenderer>().enabled = false;

                m_DeadEarthElemental = Instantiate(m_DeadEarthElementalPrefab, transform.position, Quaternion.identity);
                deadColl = m_DeadEarthElemental.GetComponent<Collider2D>();
                deadRb = m_DeadEarthElemental.GetComponent<Rigidbody2D>();

                m_EarthElementalSpikes = m_DeadEarthElemental.GetComponentsInChildren<Transform>();

                m_DeadEarthElemental.transform.DetachChildren();

                died = true;
            }
        }

        if (m_DeadEarthElemental != null)
        {
            if (Physics2D.IsTouchingLayers(deadColl, m_GroundMask))
            {
                deadRb.constraints = RigidbodyConstraints2D.FreezeAll;
                deadColl.enabled = false;
            }

            foreach (Transform spike in m_EarthElementalSpikes)
            {
                Collider2D spikeColl = spike.GetComponent<Collider2D>();
                Rigidbody2D spikeBody = spike.GetComponent<Rigidbody2D>();

                if (Physics2D.IsTouchingLayers(spikeColl, m_GroundMask))
                {
                    spikeBody.constraints = RigidbodyConstraints2D.FreezeAll;
                    spikeColl.enabled = false;
                }
            }
        }

    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        float a_speed = velocity.magnitude;

        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPos = new Vector2(target.position.x, target.position.y);

        float distanceToPlayer = Vector2.Distance(gameObject.transform.position, m_Player.transform.position);

        if (distanceToPlayer <= m_AttackDistance)
        {
            withinRange = true;
        }

        if (enemyPos.magnitude > playerPos.magnitude + 6)
        {
            if (rb.velocity.magnitude > 1)
            {
                rb.velocity = Vector2.zero;
            }
        }

        if (target.position.x < transform.position.x)
        {
            //m_TargetPos = target.position + new Vector3(3f, 5f);

            if (m_HoverDistance.y < 0f)
            {
                m_HoverDistance.y = System.Math.Abs(-m_HoverDistance.y);
            }

            m_TargetPos = target.position + m_HoverDistance;
        }
        else if (target.position.x > transform.position.x)
        {
            //m_TargetPos = target.position - new Vector3(3f, -5f);

            if (m_HoverDistance.y > 0f)
            {
                m_HoverDistance.y *= -1f;
            }

            m_TargetPos = target.position - m_HoverDistance;
        }

        m_HoverPos = m_TargetPos;
        m_Distance = Vector2.Distance(m_HoverPos, transform.position);

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

        if (withinRange && !EH.isDead)
        {
            rb.AddForce(force, ForceMode2D.Force);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        float finalDistance = Vector2.Distance(rb.position, path.vectorPath[path.vectorPath.Count - 1]);
        //Debug.Log(distance);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Debug.Log(a_speed);

        if (distanceToPlayer < m_AttackDistance && !isAlert && !isAgro)
        {
            isAlert = true;
            EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_ALERT);
            animDelay = 1.117f;
            Invoke(nameof(CompleteAnim), animDelay);

        }
        else if (distanceToPlayer > m_AttackDistance && !isForget && isAgro)
        {
            isForget = true;
            EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_FORGET);
            animDelay = 1.117f;
            Invoke(nameof(CompleteAnim), animDelay);

        }

        if (finalDistance < 1.6f)
        {
            rb.velocity = Vector2.zero;

            attacking = true;
            //Debug.Log("reached");
            //if(rb.position.x < target.position.x)
            //{
            //    //Debug.Log("reached");
            //    if (a_speed < 1f)
            //    {
            //        //Debug.Log("reached");
            //        rb.AddForce(-velocity, ForceMode2D.Force);
            //        //rb.AddForce(new Vector2(-10f, 0), ForceMode2D.Force);
            //    }
            //    //else if (rb.velocity.x > 1f)
            //    //{
            //    //    //Debug.Log("reached");
            //    //    rb.AddForce(new Vector2(-20f, 0), ForceMode2D.Force);
            //    //}
            //}
            //else if(rb.position.x > target.position.x)
            //{
            //    if (a_speed < 0.5f)
            //    {
            //        //Debug.Log("reached");
            //        rb.AddForce(velocity, ForceMode2D.Force);
            //        //rb.AddForce(new Vector2(5, 0), ForceMode2D.Force);
            //    }
            //    //else if (rb.velocity.x > 1f)
            //    //{
            //    //    //Debug.Log("reached");
            //    //    rb.AddForce(new Vector2(10, 0), ForceMode2D.Force);
            //    //}
            //}         
        }
        else
        {
            attacking = false;
        }

        if (rb.transform.position.x < target.position.x)
        {
            enemyGraphics.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.transform.position.x > target.position.x)
        {
            enemyGraphics.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void CompleteAnim()
    {
        if (isAlert)
        {
            isAlert = false;
            isAgro = true;
        }
        else if (isForget)
        {
            isForget = false;
            isAgro = false;
        }
    }
}
