using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSkyAttack : MonoBehaviour
{
    private AIRanged AIR;
    private AISetUp AISU;
    public AttackPlayer AP;
    public EnemyHealth EH;

    public Animator earthChunkAnimator;
    private AnimationClip[] earthChunkAnimations;

    private GameObject m_Player;
    public GameObject m_EarthSpikesPrefab;
    private GameObject m_EarthSpikes;
    public GameObject m_EarthChunkPrefab;
    private GameObject m_EarthChunk;
    public GameObject m_SpawnCheckPrefab;
    private GameObject m_SpawnCheck;

    private Rigidbody2D m_PlayerBody;
    private Rigidbody2D m_ChunkBody;

    private Vector3 m_ChunkSpawnPos;
    private Vector3 m_PlayerPos;
    private Vector3 m_ChunkHitDir;
    private Vector3 targetPos;
    private Vector3 m_SpikeSpawnPos;

    public LayerMask ignoreMask;

    public float m_AboveAttackForce;
    public float m_AttackInterval;
    public float m_HeightAbovePlayer;
    public float m_WaitTime;
    private float m_DistanceFromGround;
    public float m_SpikeHeight;
    public float m_GroundAttackSpeed;

    private bool CR_RUNNING;
    public bool m_Attacking;

    // Start is called before the first frame update
    void Start()
    {
        AIR = GetComponent<AIRanged>();
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        EH = GetComponent<EnemyHealth>();

        m_Player = AISU.m_ActivePlayer;

        //Debug.Log(m_Player);

        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();    
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

    // Update is called once per frame
    void Update()
    {
        targetPos = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + 0.5f);
        m_PlayerPos = m_Player.transform.position;

        if (AIR.attacking && AIR.isAgro && !EH.isDead)
        {
            m_Attacking = true;

            if (!CR_RUNNING && m_EarthChunk == null)
            {
                StartCoroutine(AboveAttack(m_AboveAttackForce));
            }
        }
        else
        {
            CR_RUNNING = false;
            m_Attacking = false;
        }

        if (m_EarthChunk != null)
        {
            m_ChunkHitDir = (targetPos - m_EarthChunk.transform.position).normalized;
        }

        //RaycastHit2D hit = Physics2D.Raycast(m_Player.transform.position, -Vector2.up, Mathf.Infinity, ignoreMask);

        //Debug.Log(hit.collider.gameObject.name);

        //if (hit.collider != null)
        //{
        //    m_DistanceFromGround = Mathf.Abs(hit.point.y - m_Player.transform.position.y);
        //}

        //m_SpikeSpawnPos = new Vector3(m_Player.transform.position.x, hit.collider.gameObject.transform.position.y - 0.2f, -1.0f);

        //if (AIR.attacking && AIR.isAgro)
        //{
        //    m_Attacking = true;

        //    //if (m_DistanceFromGround > 0.08f)
        //    //{
        //        if (!CR_RUNNING && m_EarthChunk == null)
        //        {
        //            StartCoroutine(AboveAttack(m_AboveAttackForce));
        //        }
        //    //}
        //    //else if (m_DistanceFromGround <= 0.08f)
        //    //{
        //    //    if (!CR_RUNNING)
        //    //    {
        //    //        StartCoroutine(GroundAttack(m_SpikeHeight, m_GroundAttackSpeed));
        //    //    }
        //    //}
        //}
        //else
        //{
        //    CR_RUNNING = false;
        //    m_Attacking = false;
        //}
    }

    public IEnumerator AboveAttack(float force)
    {
        CR_RUNNING = true;

        float x = Random.Range(m_PlayerPos.x - 5f, m_PlayerPos.x + 5f); ;

        m_ChunkSpawnPos = new Vector3(x, m_PlayerPos.y + m_HeightAbovePlayer);

        m_SpawnCheck = Instantiate(m_SpawnCheckPrefab, m_ChunkSpawnPos, Quaternion.identity);

        Collider2D wallCollider = Physics2D.OverlapCircle(m_SpawnCheck.transform.position, 0.6f);

        if(wallCollider != null)
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

        m_ChunkBody = m_EarthChunk.GetComponent<Rigidbody2D>();

        yield return new WaitForSeconds(m_WaitTime);

        m_EarthChunk.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        m_EarthChunk.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        m_ChunkBody.AddForce(m_ChunkHitDir * force, ForceMode2D.Impulse);

        //yield return new WaitForSeconds(m_AttackInterval);

        CR_RUNNING = false;
    }

    public IEnumerator GroundAttack(float height, float timeToAppear)
    {
        CR_RUNNING = true;

        Vector3 spikePos;

        //if (GameObject.FindGameObjectsWithTag("GroundAttack") != null)
        //{
        //    GameObject[] spikes = GameObject.FindGameObjectsWithTag("GroundAttack");
        //    foreach (GameObject spike in spikes)
        //    {
        //        Destroy(spike);
        //    }
        //}


        //if (m_AttackFinished && !m_Attacking)
        //{
        //    break;
        //}

        if (m_EarthSpikes == null)
        {
            m_EarthSpikes = Instantiate(m_EarthSpikesPrefab, m_SpikeSpawnPos, Quaternion.identity);

            m_EarthSpikes.GetComponent<AttackPlayer>().m_Enemy = gameObject;

            //if (m_EarthSpikes != null || m_Player.transform.hasChanged)
            //{
            //    m_AttackFinished = false;


            spikePos = m_EarthSpikes.transform.position;
            Vector3 spikePosNew = new Vector3(spikePos.x, spikePos.y + height, spikePos.z);

            //ParticleSystem spikeRumble = Instantiate(m_SpikeRumble, m_SpikeSpawnPos, Quaternion.identity);
            ParticleSystem spikeRumble = m_EarthSpikes.GetComponent<ParticleSystem>();

            spikeRumble.Play();
            //float particleDuration = spikeRumble.main.duration;
            //Destroy(spikeRumble, particleDuration);

            yield return new WaitForSeconds(1.5f);

            float elapsedTime = 0;

            //Debug.Log("Spike Count: " + m_spikeCount);

            Vector3 startPosition = m_EarthSpikes.transform.position;

            while (elapsedTime < timeToAppear)
            {
                if (m_EarthSpikes != null)
                {
                    m_EarthSpikes.transform.position = Vector3.Lerp(startPosition, spikePosNew, elapsedTime / timeToAppear);
                }
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            elapsedTime = 0;

            while (elapsedTime < timeToAppear)
            {
                if (m_EarthSpikes != null)
                {
                    m_EarthSpikes.transform.position = Vector3.Lerp(spikePosNew, startPosition, elapsedTime / timeToAppear);
                }
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Destroy(m_EarthSpikes);

        }
        CR_RUNNING = false;

        yield return new WaitForSeconds(1.5f);
    }
}
