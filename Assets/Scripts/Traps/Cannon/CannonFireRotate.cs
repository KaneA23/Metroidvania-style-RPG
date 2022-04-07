using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFireRotate : MonoBehaviour
{
    private AISetUp AISU;

    public GameObject m_CannonBallPrefab;
    private GameObject m_CannonBall;
    private GameObject m_Player;

    private Rigidbody2D m_BallBody;

    Vector3 m_PlayerPos;

    private float m_DistanceToPlayer;
    public float m_FireInterval;
    public float m_FireForce;
    public float m_HitForce;
    public float m_AggroDistance;

    private bool CR_RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        m_DistanceToPlayer = Vector2.Distance(gameObject.transform.position, m_Player.transform.position);
        m_PlayerPos = m_Player.transform.position;

        Vector3 cannonPos = transform.position;
        m_PlayerPos.x = m_PlayerPos.x - cannonPos.x;
        m_PlayerPos.y = m_PlayerPos.y - cannonPos.y;

        float angle = Mathf.Atan2(m_PlayerPos.y, m_PlayerPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (m_DistanceToPlayer < m_AggroDistance)
        {
            if (!CR_RUNNING)
            {
                StartCoroutine(Firing(m_FireInterval));
            }
        }
        else
        {
            CR_RUNNING = false;
        }

    }

    private IEnumerator Firing(float fireInterval)
    {
        CR_RUNNING = true;

        m_CannonBall = Instantiate(m_CannonBallPrefab, gameObject.transform.position, Quaternion.identity);
        m_CannonBall.GetComponent<CannonBall>().m_Cannon = gameObject;
        m_BallBody = m_CannonBall.GetComponent<Rigidbody2D>();
        m_BallBody.AddForce(m_PlayerPos.normalized * m_FireForce);

        yield return new WaitForSeconds(fireInterval);

        CR_RUNNING = false;
    }
}
