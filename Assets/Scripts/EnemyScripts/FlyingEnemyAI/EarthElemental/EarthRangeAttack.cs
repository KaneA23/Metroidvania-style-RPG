using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRangeAttack : MonoBehaviour
{
    public AIRanged AIR;
    public AISetUp AISU;

    public GameObject m_EarthChunkPrefab;
    public GameObject m_EarthChunk;
    public GameObject m_EarthSpikesPrefab;
    public GameObject m_EarthSpikes;
    public GameObject m_Player;
    public GameObject m_Floor;

    private Rigidbody2D m_PlayerBody;
    private Rigidbody2D m_ChunkBody;

    private Vector3 m_SpikeHeight;
    private Vector3 m_SpikeSpawnPos;
    private Vector3 m_ChunkHitDir;
    private Vector3 m_ChunkSpawnPos;
    private Vector3 m_PlayerPos;
    private Vector3 m_FlooorPos;

    public ParticleSystem m_SpikeRumble;

    public float m_GroundAttackSpeed;
    public float m_AboveAttackForce;
    public float m_AttackInterval;
    public float m_FloorDistance;

    int m_spikeCount = 0;

    private bool m_Attacking;
    private bool CR_RUNNING;
    private bool m_AttackFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        AIR = GetComponent<AIRanged>();
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;
        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();

        m_FlooorPos = m_Floor.transform.position;
    }

    void FixedUpdate()
    {
        m_SpikeHeight = new Vector3(m_Player.transform.position.x, -3.8599999f);
        m_SpikeSpawnPos = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y - 0.5f, -1.0f);

        m_PlayerPos = m_Player.transform.position;

        //RaycastHit hit;

        //if(Physics.Linecast(m_PlayerPos, m_FlooorPos, out hit))
        //{
        //    m_FloorDistance = hit.distance;
        //}
        
        if (AIR.attacking)
        {
            m_Attacking = true;

            Debug.Log("Player y Pos: " + m_PlayerPos.y);

            if (m_PlayerPos.y > -3f)
            {
                if (!CR_RUNNING)
                {
                    StartCoroutine(AboveAttack(m_AboveAttackForce));
                }
            }
            else if (m_PlayerPos.y < -3f)
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

    public IEnumerator GroundAttack(Vector3 position, float timeToAppear)
    {
        CR_RUNNING = true;

        if (GameObject.FindGameObjectsWithTag("GroundAttack") != null)
        {
            GameObject[] spikes = GameObject.FindGameObjectsWithTag("GroundAttack");
            foreach (GameObject spike in spikes)
            {
                Destroy(spike);
            }
        }

        while (m_Attacking)
        {
            //if (m_AttackFinished && !m_Attacking)
            //{
            //    break;
            //}

            Debug.Log("Spike Count: " + m_spikeCount);

            if (m_EarthSpikes == null)
            {
                m_EarthSpikes = Instantiate(m_EarthSpikesPrefab, m_SpikeSpawnPos, Quaternion.identity);
                position = new Vector3(m_PlayerPos.x, m_PlayerPos.y);
                m_spikeCount = 1;
            }

            if (m_EarthSpikes != null || m_Player.transform.hasChanged)
            {
                m_AttackFinished = false;

                position = new Vector3(m_PlayerPos.x, m_PlayerPos.y);

                ParticleSystem spikeRumble = Instantiate(m_SpikeRumble, m_Player.transform.position, Quaternion.identity);
                spikeRumble.Play();
                float particleDuration = spikeRumble.duration + spikeRumble.startLifetime;
                Destroy(spikeRumble, particleDuration);
                yield return new WaitForSeconds(particleDuration - 2);

                float elapsedTime = 0;

                //Debug.Log("Spike Count: " + m_spikeCount);


                Vector3 startPosition = m_EarthSpikes.transform.position;

                while (elapsedTime < timeToAppear)
                {
                    if (m_EarthSpikes != null)
                    {
                        m_EarthSpikes.transform.position = Vector3.Lerp(startPosition, position, elapsedTime / timeToAppear);
                    }
                    elapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                elapsedTime = 0;

                while (elapsedTime < timeToAppear)
                {
                    if (m_EarthSpikes != null)
                    {
                        m_EarthSpikes.transform.position = Vector3.Lerp(position, startPosition, elapsedTime / timeToAppear);
                    }
                    elapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                Destroy(m_EarthSpikes);
                m_spikeCount = 0;
                m_AttackFinished = true;
            }
        }
    }

    public IEnumerator AboveAttack(float force)
    {
        CR_RUNNING = true;

        while (m_Attacking)
        {
            float x = Random.Range(m_PlayerPos.x - 5f, m_PlayerPos.x + 5f); ;

            m_ChunkSpawnPos = new Vector3(x, m_PlayerPos.y + 5.5f);

            m_EarthChunk = Instantiate(m_EarthChunkPrefab, m_ChunkSpawnPos, Quaternion.identity);

            m_ChunkBody = m_EarthChunk.GetComponent<Rigidbody2D>();

            m_ChunkHitDir = (m_Player.transform.position - m_EarthChunk.transform.position).normalized;

            m_ChunkBody.AddForce(m_ChunkHitDir * force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(2f);
        }
    }
}
