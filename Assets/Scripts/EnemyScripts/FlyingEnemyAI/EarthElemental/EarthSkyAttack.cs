using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSkyAttack : MonoBehaviour
{
    private AIRanged AIR;
    private AISetUp AISU;

    private GameObject m_Player;
    public GameObject m_EarthChunkPrefab;
    private GameObject m_EarthChunk;

    private Rigidbody2D m_PlayerBody;
    private Rigidbody2D m_ChunkBody;

    private Vector3 m_ChunkSpawnPos;
    private Vector3 m_PlayerPos;
    private Vector3 m_ChunkHitDir;
    private Vector3 targetPos;

    public float m_AboveAttackForce;
    public float m_AttackInterval;
    public float m_HeightAbovePlayer;

    private bool CR_RUNNING;
    public bool m_Attacking;

    // Start is called before the first frame update
    void Start()
    {
        AIR = GetComponent<AIRanged>();
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;

        Debug.Log(m_Player);

        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + 0.5f);
        m_PlayerPos = m_Player.transform.position;

        if(AIR.attacking && AIR.isAgro)
        {
            m_Attacking = true;

            if (!CR_RUNNING)
            {
                StartCoroutine(AboveAttack(m_AboveAttackForce));
            }
        }
        else
        {
            CR_RUNNING = false;
            m_Attacking = false;
        }
    }

public IEnumerator AboveAttack(float force)
    {
        CR_RUNNING = true;

        float x = Random.Range(m_PlayerPos.x - 5f, m_PlayerPos.x + 5f); ;

        m_ChunkSpawnPos = new Vector3(x, m_PlayerPos.y + m_HeightAbovePlayer);

        m_EarthChunk = Instantiate(m_EarthChunkPrefab, m_ChunkSpawnPos, Quaternion.identity);

        m_EarthChunk.GetComponent<AttackPlayer>().m_Enemy = gameObject;

        m_ChunkBody = m_EarthChunk.GetComponent<Rigidbody2D>();

        m_ChunkHitDir = (targetPos - m_EarthChunk.transform.position).normalized;

        m_ChunkBody.AddForce(m_ChunkHitDir * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(m_AttackInterval);

        CR_RUNNING = false;
    }
}
