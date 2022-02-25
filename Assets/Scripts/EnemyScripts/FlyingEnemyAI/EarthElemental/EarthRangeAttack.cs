using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EarthRangeAttack : MonoBehaviour
{
    public AIRanged AIR;
    public AISetUp AISU;

    //public GameObject m_EarthChunkPrefab;
    //public GameObject m_EarthChunk;
    public GameObject m_EarthSpikesPrefab;
    public GameObject m_EarthSpikes;
    public GameObject m_Player;

    private Rigidbody2D m_PlayerBody;

    private Vector3 m_SpikeHeight;
    private Vector3 m_SpikeSpawnPos;

    public float m_GroundAttackSpeed;
    public float m_AttackInterval;
    private int m_Attacks = 0;

    private bool m_Attacking;
    private bool CR_RUNNING;
    private bool m_AttackComplete;

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
        m_Attacks++;

        CR_RUNNING = true;

        while (m_Attacking)
        {

            if (m_Player.transform.hasChanged)
            {
                position = new Vector3(m_Player.transform.position.x, -3.8599999f);
            }

            m_AttackComplete = false;
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

            m_AttackComplete = true;
        }
    }

    void AboveAttack()
    {

    }

}
