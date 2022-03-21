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

    private Rigidbody2D m_PlayerBody;
    private Rigidbody2D m_ChunkBody;

    private Vector3 m_SpikeHeight;
    private Vector3 m_SpikeSpawnPos;
    private Vector3 m_ChunkHitDir;
    private Vector3 m_ChunkSpawnPos;

    public float m_GroundAttackSpeed;
    public float m_AboveAttackForce;
    public float m_AttackInterval;

    private bool m_Attacking;
    private bool CR_RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        AIR = GetComponent<AIRanged>();
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;
        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_SpikeHeight = new Vector3(m_Player.transform.position.x, -3.8599999f);
        m_SpikeSpawnPos = new Vector3(m_Player.transform.position.x, -4.6f);

        if (AIR.attacking)
        {
            m_Attacking = true;

            if (m_Player.transform.position.y > -2.467551)
            {
                if(!CR_RUNNING)
                { 
                    StartCoroutine(AboveAttack(m_AboveAttackForce));
                }                
            }
            else if (m_Player.transform.position.y < -2.467551)
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

        while (m_Attacking)
        {
            if (m_Player.transform.hasChanged)
            {
                position = new Vector3(m_Player.transform.position.x, -3.8599999f);
            }

            m_EarthSpikes = Instantiate(m_EarthSpikesPrefab, m_SpikeSpawnPos, Quaternion.identity);

            float elapsedTime = 0;
            Vector3 startPosition = m_EarthSpikes.transform.position;

            while (elapsedTime < timeToAppear)
            {
                m_EarthSpikes.transform.position = Vector3.Lerp(startPosition, position, elapsedTime / timeToAppear);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            elapsedTime = 0;

            while (elapsedTime < timeToAppear)
            {
                m_EarthSpikes.transform.position = Vector3.Lerp(position, startPosition, elapsedTime / timeToAppear);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            Destroy(m_EarthSpikes);
        }
    }

    public IEnumerator AboveAttack(float force)
    {
        CR_RUNNING = true;

        while(m_Attacking)
        {
            float x = Random.Range(-8.66f, 8.66f);;

            m_ChunkSpawnPos = new Vector3(x, 5.6f);

            m_EarthChunk = Instantiate(m_EarthChunkPrefab, m_ChunkSpawnPos, Quaternion.identity);

            m_ChunkBody = m_EarthChunk.GetComponent<Rigidbody2D>();

            m_ChunkHitDir = (m_Player.transform.position - m_EarthChunk.transform.position).normalized;

            m_ChunkBody.AddForce(m_ChunkHitDir * force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(2f);
        }
    }
}
