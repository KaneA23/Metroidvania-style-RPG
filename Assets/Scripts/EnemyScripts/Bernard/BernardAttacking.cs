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
    public BernardAnimationSystem BAS;

    private Vector3 m_TargetPos;
    private Vector3 m_PlayerPos;
    private Vector2 m_TargetDir;
    private Vector3 m_MovementDirection;
    private Vector3 m_NewDestination;
    private Vector3 m_CurrentPos;
    private Vector3 m_OrigPos;

    private Transform m_PlayerTransform;

    private GameObject m_Player;
    public GameObject m_Floor;

    private SpriteRenderer m_SpriteRenderer;

    private Rigidbody2D rb;
    private Rigidbody2D m_PlayerBody;

    private Collider2D m_BossCollider;
    private Collider2D m_PlayerCollider;

    public GameObject m_EarthChunkPrefab;

    private char wallSide;

    public int m_GroundAttackDamage;

    public float m_GroundAttackKnockback;
    public float m_Speed;
    public float m_WallJumpForce;
    public float m_AttackDistance;
    public float HitForce;
    public float m_ProjectileForce;
    public float m_AboveAttackInterval;
    float minDistance = Mathf.Infinity;
    public float animDelay;

    public bool m_MovingToTarget;
    public bool isAlert;
    public bool isForget;
    public bool isAgro;
    [SerializeField] private bool m_Attacking;
    private bool dirChosen = false;
    private bool onWall = false;
    private bool jumpedUp = false;
    private bool jumpedDown = false;
    private bool thirdPhase = false;
    private bool CR_RUNNING = false;

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
        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();

        m_BossCollider = GetComponent<Collider2D>();
        m_PlayerCollider = m_PlayerTransform.GetComponent<Collider2D>();

        thirdPhase = false;
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

        if (m_TargetDir.x < 0)
        {
            m_SpriteRenderer.flipX = false;
        }

        if (m_TargetDir.x > 0)
        {
            m_SpriteRenderer.flipX = true;
        }

        //m_OrigPos = transform.position.x;
        //}
    }

    private void GroundAttacking()
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
        Collider2D floorCollider = m_Floor.GetComponent<Collider2D>();

        float BernardWidth = m_BossCollider.bounds.size.x;
        float BernardHeight = m_BossCollider.bounds.size.y;

        if (!thirdPhase)
        {
            if (otherObject.CompareTag("BossWall"))
            {
                if (!Physics2D.IsTouching(GetComponent<Collider2D>(), floorCollider))
                {
                    onWall = true;
                    gameObject.layer = 3;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
            else if (otherObject.gameObject.layer == 10)
            {
                jumpedUp = false;
            }           
            else
            {
                onWall = false;
            }
        }
        else
        {
            if ((transform.position.x - otherObject.transform.position.x) < 0)
            {
                wallSide = 'L';
            }
            else if ((transform.position.x - otherObject.transform.position.x) > 0)
            {
                wallSide = 'R';
            }    
        }

        if (otherObject.gameObject == m_Player)
        {
            PHS.TakeDamage(m_GroundAttackDamage, gameObject.transform.position, m_GroundAttackKnockback, true);

            if(PHS.isEnemyBack)
            {
                if(Physics2D.IsTouching(GetComponent<Collider2D>(), floorCollider))
                {
                    m_PlayerBody.AddForce(new Vector2(0f, 1f).normalized * 20, ForceMode2D.Impulse);
                    rb.AddForce(new Vector2(0f, 1f).normalized * 6, ForceMode2D.Impulse);
                }                             
            }
        }
    }

    void Attacking_1()
    {
        GroundAttacking();
    }

    void Attacking_2()
    {
        System.Random rng = new System.Random();
        m_TargetPos = m_Player.transform.position;       

        if (!onWall)
        {
            gameObject.layer = 12;

            if (!dirChosen)
            {
                dirChoice = rng.Next(2);
                Debug.Log("Direction Choice: " + dirChoice);
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

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 25f);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].CompareTag("BossWall"))
                {
                    Vector2 wallDir = (transform.position - hitColliders[i].gameObject.transform.position).normalized;
                    if (!jumpedUp)
                    {
                        BAS.currentAnimName = "LizardJump";
                        rb.AddForce(new Vector2(wallDir.x, 1f).normalized * m_WallJumpForce, ForceMode2D.Impulse);
                        jumpedUp = true;
                    }
                }
            }
        }
        else
        {
            if (!CR_RUNNING)
            {
                StartCoroutine(AboveAttack(m_ProjectileForce));
            }
        }
    }

    public IEnumerator AboveAttack(float force)
    {
        CR_RUNNING = true;

        float x = UnityEngine.Random.Range(m_TargetPos.x - 5f, m_TargetPos.x + 5f); ;

        Vector2 m_ChunkSpawnPos = new Vector2(x, m_TargetPos.y + 8f);

        GameObject m_EarthChunk = Instantiate(m_EarthChunkPrefab, m_ChunkSpawnPos, Quaternion.identity);

        m_EarthChunk.GetComponent<AttackPlayer>().m_Enemy = gameObject;

        Rigidbody2D m_ChunkBody = m_EarthChunk.GetComponent<Rigidbody2D>();

        Vector2 m_ChunkHitDir = (m_TargetPos - m_EarthChunk.transform.position).normalized;

        m_ChunkBody.AddForce(m_ChunkHitDir * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(m_AboveAttackInterval);

        CR_RUNNING = false;
    }

    void Attacking_3()
    {
        if (!jumpedDown)
        {
            if (!thirdPhase)
            {
                thirdPhase = true;
                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            switch (wallSide)
            {
                case 'L':
                    rb.AddForce(new Vector2(1, 0).normalized * 100, ForceMode2D.Impulse);
                    break;
                case 'R':
                    rb.AddForce(new Vector2(-1, 0).normalized * 100, ForceMode2D.Impulse);
                    break;
            }

            jumpedDown = true;
        }

        if (!CR_RUNNING)
        {
            StartCoroutine(AboveAttack(m_ProjectileForce));
        }

        GroundAttacking();

    }
}
