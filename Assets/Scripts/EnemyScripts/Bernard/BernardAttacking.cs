using System.Collections;
using UnityEngine;
using System;
public class BernardAttacking : MonoBehaviour
{
    [Header("Script References")]
    private BernardStates BS;
    public PlayerHealthSystem PHS;
    public EnemyHealth EH;
    public AISetUp AISU;
    public EnemyAnimationManager EAM;
    public BernardAnimationSystem BAS;
    [Space]
    private Vector3 m_TargetPos;
    private Vector2 m_TargetDir;
    Vector2 m_ChunkHitDir;
    [SerializeField] private Vector2 playerDir;

    private AnimationClip[] earthChunkAnimations;
    public Animator earthChunkAnimator;

    private Transform m_PlayerTransform;

    [SerializeField] private GameObject m_Player;
    GameObject m_EarthChunk;
    public GameObject m_Floor;
    private GameObject m_ChosenWall;
    public GameObject m_SpawnCheckPrefab;
    private GameObject m_SpawnCheck;

    public SpriteRenderer m_SpriteRenderer;

    [SerializeField] private Rigidbody2D rb;
    private Rigidbody2D m_PlayerBody;

    public CapsuleCollider2D m_BodyCollider;
    public PolygonCollider2D m_TailCollider;
    private BoxCollider2D m_BernardWallCollider;
    private Collider2D m_PlayerCollider;
    private Collider2D floorCollider;

    public GameObject m_EarthChunkPrefab;

    public LayerMask[] m_IgnoreMasks;
    public LayerMask m_PlayerMask;
    public LayerMask m_FloorMask;


    [SerializeField] private char directionChoice;
    public char playerDirection;
    public char playerDirNew;

    public int m_GroundAttackDamage;

    public float m_GroundAttackKnockback;
    public float m_Speed = 3;
    public float m_WallJumpForce;
    public float m_AttackDistance;
    public float HitForce;
    public float m_ProjectileForce;
    public float m_AboveAttackInterval;
    public float animDelay;
    public float wallDetectionRadius;
    public float m_WaitTime;
    public float m_HeightAbovePlayer;
    public float m_SpawnCheckRadius;
    private float m_Time;
    [SerializeField] float m_DistanceToPlayer;

    private bool wallHit;
    public bool m_MovingToTarget;
    public bool isAlert;
    public bool isForget;
    public bool isAgro;
    private bool flipReached = false;
    private bool oneTime = false;
    private bool flipped = false;
    public bool deathAnimFinished = false;

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
    private bool acceptableDistace = false;

    private int dirChoice = 0;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        BS = GetComponent<BernardStates>();

        floorCollider = m_Floor.GetComponent<Collider2D>();

        PHS = AISU.PHS;

        m_PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();

        rb = GetComponent<Rigidbody2D>();

        m_PlayerTransform = AISU.m_ActivePlayer.transform;
        m_Player = AISU.m_ActivePlayer;
        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();

        m_PlayerCollider = m_PlayerTransform.GetComponent<Collider2D>();
        m_BernardWallCollider = BS.bernardWallBody.GetComponent<BoxCollider2D>();

        thirdPhase = false;

        Physics2D.IgnoreLayerCollision(6, 17);
        Physics2D.IgnoreLayerCollision(10, 17);
        Physics2D.IgnoreLayerCollision(9, 17);
        Physics2D.IgnoreLayerCollision(7, 17);
        Physics2D.IgnoreLayerCollision(3, 17);
        Physics2D.IgnoreLayerCollision(16, 17);
        Physics2D.IgnoreLayerCollision(17, 12);
    }

    // Update is called once per frame
    void FixedUpdate()
    {     

        if (m_EarthChunk != null)
        {
            m_ChunkHitDir = (m_TargetPos - m_EarthChunk.transform.position).normalized;
        }

        float h = Input.GetAxisRaw("Horizontal");

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

        //EnemyFacing();
    }

    private void EnemyFacing()
    {
        if (thirdPhase && !secondPhase)
        {
            if (!flipped)
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
        // Prevents Bernard from attacking during his entrance
        if (FindObjectOfType<BernardIntroCutscene>().isCutscene)
		{
            return;
		}

        m_TargetPos = new Vector2(m_PlayerTransform.position.x, gameObject.transform.position.y);
        m_TargetDir = (m_TargetPos - transform.position).normalized;
        m_DistanceToPlayer = Vector2.Distance(transform.position, m_PlayerTransform.position);

        if (m_DistanceToPlayer < BS.m_AttackDistance)
        {
            rb.AddForce(m_TargetDir * m_Speed);
            m_MovingToTarget = true;
        }
        else
        {
            m_MovingToTarget = false;
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
    #region COLLISION_CHECKS
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_Time = 0;

        Collider2D otherObject = collision.collider;

        if (otherObject.CompareTag("BossWall"))
        {
            if (secondPhase)
            {
                m_ChosenWall = otherObject.gameObject;

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

        if (otherObject.gameObject == m_Player)
        {
            if (!secondPhase)
            {
                PHS.TakeDamage(m_GroundAttackDamage, gameObject.transform.position, m_GroundAttackKnockback, true);

                if (m_BodyCollider.bounds.Intersects(m_PlayerCollider.bounds) || m_TailCollider.bounds.Intersects(m_PlayerCollider.bounds))
                {
                    m_Player.layer = 12;
                }
            }

            if (PHS.isEnemyBack && !secondPhase)
            {
                if (Physics2D.IsTouching(m_BodyCollider, floorCollider) || Physics2D.IsTouching(m_TailCollider, floorCollider))
                {
                    m_PlayerBody.AddForce(new Vector2(0f, 1f).normalized * 20, ForceMode2D.Impulse);
                    rb.AddForce(new Vector2(0f, 1f).normalized * 6, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (m_Time > 1)
        {
            if(collision.collider.gameObject == m_Player)
            {
                m_Time += Time.deltaTime;

                if (PHS.isEnemyBack && !secondPhase)
                {
                    if (Physics2D.IsTouching(m_BodyCollider, floorCollider) || Physics2D.IsTouching(m_TailCollider, floorCollider))
                    {
                        m_PlayerBody.AddForce(new Vector2(0f, 1f).normalized * 20, ForceMode2D.Impulse);
                        rb.AddForce(new Vector2(0f, 1f).normalized * 6, ForceMode2D.Impulse);
                    }
                }
            }          
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.gameObject == m_Player)
        {
            m_Time = 0;
        }  
    }

    #endregion

    void Attacking_1()
    {
        firstPhase = true;
        EnemyFacing();
        GroundAttacking();
    }

    void Attacking_2()
    {
        firstPhase = false;
        secondPhase = true;

        m_TargetPos = m_Player.transform.position;

        if(!onWall)
        {
            Collider2D[] wallColliders = Physics2D.OverlapCircleAll(transform.position, wallDetectionRadius);

            if(!dirChosen)
            {
                ChooseDir(wallColliders);
            }          
            
            if(dirChosen)
            {
                gameObject.layer = 17;
                transform.Find("WallCollider").gameObject.layer = 17;
                MoveTowardsWall();
            }
            
            if(!oneTime)
            {
                EnemyFacingSecondPhase();
                oneTime = true;
            }

            for (int i = 0; i < wallColliders.Length; i++)
            {
                if (wallColliders[i].CompareTag("BossWall"))
                {
                    if (!jumpedUp)
                    {
                        if (/*wallDir.x > 0*/ directionChoice == 'L' && wallColliders[i].transform.Find("SideCheck").CompareTag("LeftWall"))
                        {
                            BAS.currentAnimName = "LizardJump";

                            m_BodyCollider.enabled = false;
                            m_TailCollider.enabled = false;

                            BS.bernardWallBody.SetActive(true);

                            rb.AddForce(new Vector2(-1f, 1.5f).normalized * m_WallJumpForce, ForceMode2D.Impulse);
                            jumpedUp = true;
                            Debug.Log("Jumped Left");
                        }
                        else if (/*wallDir.x < 0*/ directionChoice == 'R' && wallColliders[i].transform.Find("SideCheck").CompareTag("RightWall"))
                        {
                            BAS.currentAnimName = "LizardJump";

                            m_BodyCollider.enabled = false;
                            m_TailCollider.enabled = false;

                            BS.bernardWallBody.SetActive(true);

                            rb.AddForce(new Vector2(1f, 0.8f).normalized * m_WallJumpForce, ForceMode2D.Impulse);
                            jumpedUp = true;
                            Debug.Log("Jumped Right");
                        }    
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

    void ChooseDir(Collider2D[] wallColliders)
    {
        float distance = 0;

        for (int i = 0; i < wallColliders.Length; i++)
        {
            if(wallColliders[i].CompareTag("BossWall"))
            {
                distance = Vector2.Distance(transform.position, wallColliders[i].transform.position);
                if(wallColliders[i].transform.Find("SideCheck").CompareTag("LeftWall"))
                {
                    directionChoice = 'R';
                    dirChosen = true;
                }
                else if(wallColliders[i].transform.Find("SideCheck").CompareTag("RightWall"))
                {
                    directionChoice = 'L';
                    dirChosen = true;
                }
            }
            else
            {
                dirChoice = UnityEngine.Random.Range(0, 2);
                Debug.Log("Direction Choice: " + dirChoice);

                if (dirChoice >= 0 && dirChoice < 1)
                {
                    directionChoice = 'R';
                }
                else
                {
                    directionChoice = 'L';
                }

                dirChosen = true;
            }
        }

        //if (!dirChosen)
        //{
        //    dirChoice = UnityEngine.Random.Range(0, 2);
        //    Debug.Log("Direction Choice: " + dirChoice);
        //    dirChosen = true;
        //}
        //else
        //{
        //    gameObject.layer = 17;
        //    transform.Find("WallCollider").gameObject.layer = 17;
        //}
    }

    void MoveTowardsWall()
    {
        //if (dirChoice >= 0 && dirChoice < 1)
        //{
        //    directionChoice = 'R';

        //    rb.AddForce(new Vector2(1f, 0f).normalized * m_Speed);
        //}
        //else
        //{
        //    directionChoice = 'L';

        //    rb.AddForce(new Vector2(-1f, 0f).normalized * m_Speed);
        //}

        if (directionChoice == 'R')
        {
            rb.AddForce(new Vector2(1f, 0f).normalized * m_Speed);
        }
        else if(directionChoice == 'L')
        {
            rb.AddForce(new Vector2(-1f, 0f).normalized * m_Speed);
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

        EnemyFacing();

        if (!jumpedDown)
        {
            m_ChosenWall.layer = 16;
            gameObject.layer = 17;

            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            switch (directionChoice)
            {
                case 'L':
                    rb.AddForce(new Vector2(1, 0).normalized * 0.05f, ForceMode2D.Impulse);
                    break;
                case 'R':
                    rb.AddForce(new Vector2(-1, 0).normalized * 0.05f, ForceMode2D.Impulse);
                    break;
            }



            if (!m_BodyCollider.bounds.Intersects(m_ChosenWall.GetComponent<Collider2D>().bounds) ||
                !m_TailCollider.bounds.Intersects(m_ChosenWall.GetComponent<Collider2D>().bounds))
            {
                if (!Physics2D.IsTouching(m_TailCollider, m_ChosenWall.GetComponent<Collider2D>()) ||
                    !Physics2D.IsTouching(m_BodyCollider, m_ChosenWall.GetComponent<Collider2D>()))
                {
                    if ((!m_BodyCollider.IsTouchingLayers(m_FloorMask) ||
                         !m_TailCollider.IsTouchingLayers(m_FloorMask) ||
                         !m_BernardWallCollider.IsTouchingLayers(m_FloorMask)))
                    {
                        if (Physics2D.IsTouching(m_TailCollider, floorCollider) ||
                            Physics2D.IsTouching(m_BodyCollider, floorCollider))
                        {
                            gameObject.layer = 3;
                            jumpedDown = true;
                        }
                    }                              
                }
                else
                {
                    jumpedDown = false;
                }
            }
        }

            Collider2D m_WallCollider = m_ChosenWall.GetComponent<Collider2D>();

            if ((m_BodyCollider.bounds.Intersects(m_WallCollider.bounds) || m_TailCollider.bounds.Intersects(m_WallCollider.bounds)))
            {
                gameObject.layer = 17;
            }

            if (!CR_RUNNING && m_EarthChunk == null)
            {
                StartCoroutine(AboveAttack(m_ProjectileForce));
            }

            GroundAttacking();
        
    }

    void Die()
    {
        if (Physics2D.IsTouching(m_TailCollider, floorCollider) ||
            Physics2D.IsTouching(m_BodyCollider, floorCollider))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            m_TailCollider.enabled = false;
            m_BodyCollider.enabled = false;    
        }

        if(BAS.animator.GetCurrentAnimatorStateInfo(0).length > BAS.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            EH.animFinished = true;
        }
        else
        {
            EH.animFinished = false;
        }
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
