using System.Collections;
using UnityEngine;
using System;
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
    private Vector2 wallDir;
    Vector2 m_ChunkHitDir;
    [SerializeField] private Vector2 playerDir;

    private AnimationClip[] earthChunkAnimations;
    public Animator earthChunkAnimator;

    private Transform m_PlayerTransform;

    private GameObject m_Player;
    GameObject m_EarthChunk;
    public GameObject m_Floor;
    private GameObject m_ChosenWall;
    [SerializeField] private GameObject m_TargetWall;
    public GameObject m_SpawnCheckPrefab;
    private GameObject m_SpawnCheck;

    public SpriteRenderer m_SpriteRenderer;

    private Rigidbody2D rb;
    private Rigidbody2D m_PlayerBody;

    private Collider2D m_BossCollider;
    public BoxCollider2D m_BodyCollider;
    public PolygonCollider2D m_TailCollider;
    private Collider2D m_PlayerCollider;
    private Collider2D floorCollider;

    public GameObject m_EarthChunkPrefab;

    public LayerMask[] m_IgnoreMasks;
    public LayerMask m_PlayerMask;

    private char wallSide;
    [SerializeField] private char directionChoice;
    public char playerDirection;
    public char playerDirNew;

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
    public float wallDetectionRadius;
    public float m_WaitTime;
    public float m_HeightAbovePlayer;
    public float m_SpawnCheckRadius;

    private bool wallHit;
    public bool m_MovingToTarget;
    public bool isAlert;
    public bool isForget;
    public bool isAgro;
    private bool flipReached = false;
    private bool oneTime = false;
    private bool flipped = false;

    [SerializeField] private bool m_Attacking;
    [SerializeField] private bool dirChosen = false;
    [SerializeField] public bool onWall = false;
    [SerializeField] public bool jumpedUp = false;
    private bool jumpedDown = false;
    [SerializeField] private bool firstPhase = false;
    [SerializeField] private bool secondPhase = false;
    [SerializeField] private bool thirdPhase = false;
    private bool CR_RUNNING = false;
    [SerializeField] private bool isFacingRight = false;
    [SerializeField] private bool isFacingLeft = true;

    private int dirChoice = 0;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        BS = GetComponent<BernardStates>();

        floorCollider = m_Floor.GetComponent<Collider2D>();

        PHS = AISU.PHS;

        m_PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();

        m_OrigPos = transform.position;

        rb = GetComponent<Rigidbody2D>();

        m_PlayerTransform = AISU.m_ActivePlayer.transform;
        m_Player = AISU.m_ActivePlayer;
        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();

        m_BossCollider = GetComponent<Collider2D>();
        m_PlayerCollider = m_PlayerTransform.GetComponent<Collider2D>();

        thirdPhase = false;

        Physics2D.IgnoreLayerCollision(6, 17);
        Physics2D.IgnoreLayerCollision(10, 17);
        Physics2D.IgnoreLayerCollision(9, 17);
        Physics2D.IgnoreLayerCollision(7, 17);
        Physics2D.IgnoreLayerCollision(3, 17);
        Physics2D.IgnoreLayerCollision(16, 17);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_MovementDirection = rb.velocity.normalized;

        if (m_EarthChunk != null)
        {
            m_ChunkHitDir = (m_TargetPos - m_EarthChunk.transform.position).normalized;
        }

        float h = Input.GetAxisRaw("Horizontal");

        //if (isAgro && !isAlert)                                                ]
        //{                                                                      ]
        //    EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_ATTACK);   ]
        //}                                                                      ]--- Kane Animation stuff. Uncomment and change as needed.
        //else if (!isAgro && !isForget)                                         ]
        //{                                                                      ]
        //    EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_WALK);     ]
        //}                                                                      ]

        if(m_BodyCollider.bounds.Intersects(m_PlayerCollider.bounds) || m_TailCollider.bounds.Intersects(m_PlayerCollider.bounds))
        {
            m_Player.layer = 12;
        }

        if (transform.position.x < m_Player.transform.position.x)
        {
            playerDirection = 'R';
        }
        else if (transform.position.x > m_Player.transform.position.x)
        {
            playerDirection = 'L';
        }

        if (onWall)
        {
            transform.Find("WallCollider").GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            transform.Find("WallCollider").GetComponent<BoxCollider2D>().isTrigger = false;
        }

        EnemyFacing();
    }

    private void EnemyFacing()
    {
        if (thirdPhase && !secondPhase)
        {
            if(!flipped)
            {
                if (directionChoice == 'L')
                {
                    isFacingRight = false;
                }
                else if (directionChoice == 'R')
                {
                    isFacingRight = true;
                }

                flipped = true;
            }

            //if (!flipped && (playerDirection == 'R' && !isFacingRight) || (playerDirection == 'L' && isFacingRight))
            //{
            //    transform.Rotate(new Vector2(0, 180));
            //    isFacingRight = !isFacingRight;
            //    flipped = true;
            //}

            if ((playerDirection == 'R' && !isFacingRight) || (playerDirection == 'L' && isFacingRight))
            {
                transform.Rotate(new Vector2(0, 180));
                isFacingRight = !isFacingRight;
            }
        }       

        if (firstPhase && !secondPhase)
        {
            if ((playerDirection == 'R' && !isFacingRight) || (playerDirection == 'L' && isFacingRight))
            {
                transform.Rotate(new Vector2(0, 180));
                isFacingRight = !isFacingRight;
            }
        }
    }

    void EnemyFacingSecondPhase()
    {
        Debug.Log("RAN");

        if (secondPhase)
        {
            //Debug.Log("player direction x: " + playerDir.x);

            if (directionChoice == 'R' && playerDirection == 'L')
            {
                if (!flipReached)
                {
                    Debug.Log("FLIP");
                    transform.Rotate(new Vector2(0, 180));
                    flipReached = true;
                }
            }
            else if (directionChoice == 'L' && playerDirection == 'R')
            {
                if (!flipReached)
                {
                    Debug.Log("FLIP");
                    transform.Rotate(new Vector2(0, 180));
                    flipReached = true;
                }
            }
            else
            {
                Debug.Log("Narp");
            }
        }
    }

    private void GroundAttacking()
    {
        m_CurrentPos = transform.position;

        m_TargetPos = new Vector3(m_PlayerTransform.position.x, transform.position.y, m_PlayerTransform.position.z);
        m_TargetDir = (m_TargetPos - transform.position).normalized;

        //if (m_MovementDirection != Vector3.zero)
        //{
        //    float angle = Mathf.Atan2(m_MovementDirection.y, m_MovementDirection.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
        //}

        if (Vector2.Distance(transform.position, m_TargetPos) < BS.m_AttackDistance)
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

        if (otherObject.CompareTag("BossWall"))
        {
            if (secondPhase)
            {
                if (!Physics2D.IsTouching(GetComponent<Collider2D>(), floorCollider))
                {
                    m_ChosenWall = otherObject.gameObject;

                    onWall = true;
                    gameObject.layer = 3;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
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

        if ((transform.position.x - otherObject.transform.position.x) < 0)
        {
            wallSide = 'L';
        }
        else if ((transform.position.x - otherObject.transform.position.x) > 0)
        {
            wallSide = 'R';
        }

        if (otherObject.gameObject == m_Player)
        {

            if (!secondPhase)
            {
                PHS.TakeDamage(m_GroundAttackDamage, gameObject.transform.position, m_GroundAttackKnockback, true);
            }

            if (PHS.isEnemyBack && !secondPhase)
            {
                if (Physics2D.IsTouching(GetComponent<Collider2D>(), floorCollider))
                {
                    m_PlayerBody.AddForce(new Vector2(0f, 1f).normalized * 20, ForceMode2D.Impulse);
                    rb.AddForce(new Vector2(0f, 1f).normalized * 6, ForceMode2D.Impulse);
                }
            }
        }
    }
    //thing
    void Attacking_1()
    {
        firstPhase = true;
        GroundAttacking();
    }

    void Attacking_2()
    {
        firstPhase = false;
        secondPhase = true;

        m_TargetPos = m_Player.transform.position;

        if (!onWall)
        {
            transform.Find("WallCollider").gameObject.layer = 17;

            if (!dirChosen)
            {
                dirChoice = UnityEngine.Random.Range(0, 2);
                Debug.Log("Direction Choice: " + dirChoice);
                dirChosen = true;
            }
            else
            {
                gameObject.layer = 17;
            }

            if (dirChoice >= 0 && dirChoice < 1)
            {
                directionChoice = 'R';

                rb.AddForce(new Vector2(1f, 0f).normalized * m_Speed);
            }
            else
            {
                directionChoice = 'L';

                rb.AddForce(new Vector2(-1f, 0f).normalized * m_Speed);
            }

            if (!oneTime)
            {
                EnemyFacingSecondPhase();
                oneTime = true;
            }

            Collider2D[] wallColliders = Physics2D.OverlapCircleAll(transform.position, wallDetectionRadius);

            for (int i = 0; i < wallColliders.Length; i++)
            {
                if (wallColliders[i].CompareTag("BossWall"))
                {

                    m_TargetWall = wallColliders[i].gameObject;
                    wallDir = (transform.position - m_TargetWall.transform.position).normalized;

                    if (!jumpedUp)
                    {
                        if (/*wallDir.x > 0*/ directionChoice == 'L' && m_TargetWall.transform.Find("SideCheck").CompareTag("LeftWall"))
                        {
                            BAS.currentAnimName = "LizardJump";
                            rb.AddForce(new Vector2(-1f, 2f).normalized * m_WallJumpForce, ForceMode2D.Impulse);                            
                        }
                        else if (/*wallDir.x < 0*/ directionChoice == 'R' && m_TargetWall.transform.Find("SideCheck").CompareTag("RightWall"))
                        {
                            BAS.currentAnimName = "LizardJump";
                            rb.AddForce(new Vector2(1f, 2f).normalized * m_WallJumpForce, ForceMode2D.Impulse);
                        }
                        jumpedUp = true;
                    }
                }
            }
        }
        else
        {
            transform.Find("WallCollider").gameObject.layer = 3;

            if (!CR_RUNNING && m_EarthChunk == null)
            {
                StartCoroutine(AboveAttack(m_ProjectileForce));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, wallDetectionRadius);
    }

    public IEnumerator AboveAttack(float force)
    {
        CR_RUNNING = true;

        float x = UnityEngine.Random.Range(m_TargetPos.x - 5f, m_TargetPos.x + 5f); ;

        Vector2 m_ChunkSpawnPos = new Vector2(x, m_TargetPos.y + m_HeightAbovePlayer);

        m_SpawnCheck = Instantiate(m_SpawnCheckPrefab, m_ChunkSpawnPos, Quaternion.identity);

        Collider2D wallCollider = Physics2D.OverlapCircle(m_SpawnCheck.transform.position, 0.6f);

        if (wallCollider != null)
        {
            Destroy(m_SpawnCheck);
            CR_RUNNING = false;
            yield break;
        }

        Destroy(m_SpawnCheck);

        m_EarthChunk = Instantiate(m_EarthChunkPrefab, m_ChunkSpawnPos, Quaternion.identity);

        m_EarthChunk.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        earthChunkAnimator = m_EarthChunk.GetComponent<Animator>();

        earthChunkAnimations = earthChunkAnimator.runtimeAnimatorController.animationClips;

        GetAnimClip();

        m_EarthChunk.GetComponent<AttackPlayer>().m_Enemy = gameObject;

        Rigidbody2D m_ChunkBody = m_EarthChunk.GetComponent<Rigidbody2D>();

        yield return new WaitForSeconds(m_WaitTime);

        m_EarthChunk.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        m_EarthChunk.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        m_ChunkBody.AddForce(m_ChunkHitDir * force, ForceMode2D.Impulse);

        CR_RUNNING = false;
    }

    void Attacking_3()
    {
        secondPhase = false;
        thirdPhase = true;

        if (!jumpedDown)
        {
            m_ChosenWall.layer = 16;
            gameObject.layer = 17;

            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            switch (wallSide)
            {
                case 'L':
                    rb.AddForce(new Vector2(1, 0).normalized * 0.1f, ForceMode2D.Impulse);
                    break;
                case 'R':
                    rb.AddForce(new Vector2(-1, 0).normalized * 0.1f, ForceMode2D.Impulse);
                    break;
            }
        }

        if (!jumpedDown)
        {
            if (Physics2D.IsTouching(GetComponent<Collider2D>(), floorCollider))
            {
                if (!m_BossCollider.bounds.Intersects(m_ChosenWall.GetComponent<Collider2D>().bounds))
                {
                    if (!Physics2D.IsTouching(GetComponent<Collider2D>(), m_ChosenWall.GetComponent<Collider2D>()))
                    {
                        gameObject.layer = 3;

                        jumpedDown = true;
                    }
                    else
                    {
                        jumpedDown = false;
                    }
                }
            }
        }

        Collider2D m_WallCollider = m_ChosenWall.GetComponent<Collider2D>();

        if (m_BodyCollider.bounds.Intersects(m_WallCollider.bounds) || m_TailCollider.bounds.Intersects(m_WallCollider.bounds))
        {
            gameObject.layer = 17;
        }

        if (!CR_RUNNING && m_EarthChunk == null)
        {
            StartCoroutine(AboveAttack(m_ProjectileForce));
        }

        GroundAttacking();
    }

    void GetAnimClip()
    {
        foreach (AnimationClip clip in earthChunkAnimations)
        {
            switch (clip.name)
            {
                case "Earth_Chunk_Instantiate":
                    m_WaitTime = clip.length;
                    break;
            }
        }
    }
}
