using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRangeAttack : MonoBehaviour
{
    public AIRanged AIR;
    public AISetUp AISU;

    public LayerMask ignoreMask;

    public GameObject m_EarthChunkPrefab;
    public GameObject m_EarthChunk;
    public GameObject m_EarthSpikesPrefab;
    public GameObject m_EarthSpikes;
    public GameObject m_Player;
    //public GameObject m_Floor;

    private Rigidbody2D m_PlayerBody;
    private Rigidbody2D m_ChunkBody;

    private Vector3 m_SpikeSpawnPos;
    private Vector3 m_ChunkHitDir;
    private Vector3 m_ChunkSpawnPos;
    private Vector3 m_PlayerPos;
    private Vector3 m_FlooorPos;

    public ParticleSystem m_SpikeRumble;

    public Animator earthChunkAnimator;
    private AnimationClip[] earthChunkAnimations;

    public float m_GroundAttackSpeed;
    public float m_AboveAttackForce;
    public float m_AttackInterval;
    public float m_FloorDistance;
    public float m_SpikeHeight;
    public float m_RaycastDistance;
    private float m_DistanceFromGround;
    public float m_HeightAbovePlayer;
    public float m_WaitTime;

    int m_spikeCount = 0;

    [SerializeField] private bool m_Attacking;
    [SerializeField] private bool CR_RUNNING;
    private bool m_AttackFinished = false;

    void Start()
    {
        AIR = GetComponent<AIRanged>();
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;

        Debug.Log(m_Player);

        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();

        //m_FlooorPos = m_Floor.transform.position;
    }

    void Update()
    {
        m_PlayerPos = m_Player.transform.position;

        //RaycastHit hit;

        //if(Physics.Linecast(m_PlayerPos, m_FlooorPos, out hit))
        //{
        //    m_FloorDistance = hit.distance;
        //}

        RaycastHit2D hit = Physics2D.Raycast(m_Player.transform.position, -Vector2.up, Mathf.Infinity, ignoreMask);

        Debug.Log(hit.collider.gameObject.name);

        if (hit.collider != null)
        {
            m_DistanceFromGround = Mathf.Abs(hit.point.y - m_Player.transform.position.y);
        }

        m_SpikeSpawnPos = new Vector3(m_Player.transform.position.x, hit.collider.gameObject.transform.position.y - 0.2f, -1.0f);

        if (AIR.attacking)
        {
            m_Attacking = true;

            if (m_DistanceFromGround > 0.08f)
            {
                if (!CR_RUNNING)
                {
                    StartCoroutine(AboveAttack(m_AboveAttackForce));
                }
            }
            else if (m_DistanceFromGround <= 0.08f)
            {
                if (!CR_RUNNING)
                {
                    StartCoroutine(GroundAttack(m_SpikeHeight, m_GroundAttackSpeed));
                }
            }
        }
        else
        {
            CR_RUNNING = false;
            m_Attacking = false;
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

    public IEnumerator AboveAttack(float force)
    {
        CR_RUNNING = true;

        float x = Random.Range(m_PlayerPos.x - 5f, m_PlayerPos.x + 5f); ;

        m_ChunkSpawnPos = new Vector3(x, m_PlayerPos.y + 8f);

        m_EarthChunk = Instantiate(m_EarthChunkPrefab, m_ChunkSpawnPos, Quaternion.identity);

        m_EarthChunk.GetComponent<AttackPlayer>().m_Enemy = gameObject;

        m_ChunkBody = m_EarthChunk.GetComponent<Rigidbody2D>();

        m_ChunkHitDir = (m_Player.transform.position - m_EarthChunk.transform.position).normalized;

        m_ChunkBody.AddForce(m_ChunkHitDir * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);

        CR_RUNNING = false;
    }
}
