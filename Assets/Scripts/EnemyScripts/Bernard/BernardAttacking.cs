using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BernardAttacking : MonoBehaviour
{
    private BernardStates BS;
    public PlayerHealthSystem PHS;
    public EnemyHealth EH;
    public AISetUp AISU;
    public EnemyAnimationManager EAM;

    private Vector3 m_TargetPos;
    private Vector2 m_TargetDir;
    private Vector3 m_MovementDirection;
    private Vector3 m_NewDestination;
    private Vector3 m_CurrentPos;
    private Vector3 m_OrigPos;

    private Transform m_PlayerTransform;

    private GameObject m_Player;

    private SpriteRenderer m_SpriteRenderer;

    private Rigidbody2D rb;

    private Collider2D m_BossCollider;
    private Collider2D m_PlayerCollider;

    public float m_Speed;
    public float m_WallJumpForce;
    public float m_AttackDistance;
    public float HitForce;
    float minDistance = Mathf.Infinity;
    public float animDelay;

    public bool m_MovingToTarget;
    public bool isAlert;
    public bool isForget;
    public bool isAgro;
    private bool dirChosen = false;
    private bool onWall = false;
    private bool jumped = false;

    private int dirChoice = 0;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        BS = GetComponent<BernardStates>();

        PHS = AISU.PHS;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();

        m_OrigPos = transform.position;

        rb = GetComponent<Rigidbody2D>();

        m_PlayerTransform = AISU.m_ActivePlayer.transform;
        m_Player = AISU.m_ActivePlayer;

        m_BossCollider = GetComponent<Collider2D>();
        m_PlayerCollider = m_PlayerTransform.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isAgro && !isAlert)                                                ]
        //{                                                                      ]
        //    EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_ATTACK);   ]
        //}                                                                      ]--- Kane Animation stuff. Uncomment and change as needed.
        //else if (!isAgro && !isForget)                                         ]
        //{                                                                      ]
        //    EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_WALK);     ]
        //}                                                                      ]

        Physics2D.IgnoreLayerCollision(6, 12);
    }

    private void EnemyFacing()
    {
        //if (collisionCount == 0)
        //{

        if (m_TargetDir.x > 0)
        {
            m_SpriteRenderer.flipX = false;
        }

        if (m_TargetDir.x < 0)
        {
            m_SpriteRenderer.flipX = true;
        }

        //m_OrigPos = transform.position.x;
        //}
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D otherObject = collision.collider;

        if(otherObject.CompareTag("BossWall"))
        {
            onWall = true;
            gameObject.layer = 3;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (otherObject.gameObject.layer == 10)
        {
            jumped = false;
        }
        else if (otherObject.gameObject == m_Player)
        {
            
        }
        else
        {
            onWall = false;
        }
    }

    void Attacking_1()
    {
        m_CurrentPos = transform.position;

        m_TargetPos = new Vector3(m_PlayerTransform.position.x, transform.position.y, m_PlayerTransform.position.z);
        m_TargetDir = (m_TargetPos - transform.position).normalized;

        EnemyFacing();

        if (m_MovementDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(m_MovementDirection.y, m_MovementDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (Vector2.Distance(transform.position, m_TargetPos) < m_AttackDistance)
        {
            rb.AddForce(m_TargetDir * m_Speed);
            
            m_MovingToTarget = true;

            //if (!isAlert && !isAgro)                                              ]
            //{                                                                     ]
            //    isAlert = true;                                                   ]
            //    EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_ALERT);   ]--- Kane Animation stuff. Uncomment and change as needed.
            //    animDelay = 0.383f;                                               ]
            //    Invoke(nameof(CompleteAnim), animDelay);                          ]
            //}                                                                     ]
        }
        else
        {
            m_MovingToTarget = false;

            //if (!isForget && isAgro)                                               ]
            //{                                                                      ]
            //    isForget = true;                                                   ]
            //    EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_FORGET);   ]--- Kane Animation stuff. Uncomment and change as needed.
            //    animDelay = 0.383f;                                                ]
            //    Invoke(nameof(CompleteAnim), animDelay);                           ]
            //}                                                                      ]
        }
    }

    void Attacking_2()
    {
        System.Random rng = new System.Random();

        if(!onWall)
        {
            gameObject.layer = 12;

            if (!dirChosen)
            {
                dirChoice = rng.Next(1, 2);
                dirChosen = true;
            }

            if (dirChoice == 1)
            {
                rb.AddForce(new Vector2(transform.position.x + 0.1f, transform.position.y).normalized * m_Speed);
            }
            else if (dirChoice == 2)
            {
                rb.AddForce(new Vector2(transform.position.x - 0.1f, transform.position.y).normalized * m_Speed);
            }

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 7f);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].CompareTag("BossWall"))
                {
                    Vector2 wallDir = (transform.position - hitColliders[i].gameObject.transform.position).normalized;
                    if(!jumped)
                    {
                        rb.AddForce(new Vector2(wallDir.x, wallDir.y + 2f).normalized * m_WallJumpForce, ForceMode2D.Impulse);
                        jumped = true;
                    }                                     
                }
            }
        }        
    }

    void Attacking_3()
    {
      
    }
}
