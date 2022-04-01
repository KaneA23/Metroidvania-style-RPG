using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSpikesAttack : MonoBehaviour
{
    private AIRanged AIR;
    private AISetUp AISU;

    private GameObject m_Player;
    public GameObject m_EarthSpikesPrefab;
    private GameObject m_EarthSpikes;

    private Rigidbody2D m_PlayerBody;

    public LayerMask ignoreMask;

    private Vector3 m_SpikeSpawnPos;

    public float m_SpikeAttackSpeed;
    public float m_SpikeHeight;
    public float m_RaycastDistance;
    private float m_DistanceFromGround;

    [SerializeField] private bool m_Attacking;
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
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(m_Player.transform.position, -Vector2.up, Mathf.Infinity, ignoreMask);

        Debug.Log(hit.collider.gameObject.name);

        if (hit.collider != null)
        {
            m_DistanceFromGround = Mathf.Abs(hit.point.y - m_Player.transform.position.y);
        }

        m_SpikeSpawnPos = new Vector3(m_Player.transform.position.x, hit.collider.gameObject.transform.position.y - 0.2f, -1.0f);

        if(AIR.attacking)
        {
            m_Attacking = true;

            if (m_DistanceFromGround <= 0.08f)
            {
                if(!CR_RUNNING)
                {
                    StartCoroutine(SpikeAttack(m_SpikeHeight, m_SpikeAttackSpeed));
                }
            }
        }
        else
        {
            CR_RUNNING = false;
            m_Attacking = false;
        }
    }

    public IEnumerator SpikeAttack(float height, float timeToAppear)
    {
        CR_RUNNING = true;

        Vector3 spikePos;

        if(m_EarthSpikes == null)
        {
            m_EarthSpikes = Instantiate(m_EarthSpikesPrefab, m_SpikeSpawnPos, Quaternion.identity);

            m_EarthSpikes.GetComponent<AttackPlayer>().m_Enemy = gameObject;

            spikePos = m_EarthSpikes.transform.position;
            Vector3 spikePosNew = new Vector3(spikePos.x, spikePos.y + height, spikePos.z);

            ParticleSystem spikeRumble = m_EarthSpikes.GetComponent<ParticleSystem>();

            spikeRumble.Play();

            yield return new WaitForSeconds(1.5f);

            float elapsedTime = 0;

            Vector3 startPosition = m_EarthSpikes.transform.position;

            while(elapsedTime < timeToAppear)
            {
                if(m_EarthSpikes != null)
                {
                    m_EarthSpikes.transform.position = Vector3.Lerp(startPosition, spikePosNew, elapsedTime / timeToAppear);
                }
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            elapsedTime = 0;

            while(elapsedTime < timeToAppear)
            {
                if(m_EarthSpikes != null)
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
