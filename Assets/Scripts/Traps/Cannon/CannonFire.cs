using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFire : MonoBehaviour
{
    private AISetUp AISU;

    public GameObject m_CannonBallPrefab;
    private GameObject m_CannonBall;
    private GameObject m_Player;

    private Rigidbody2D m_BallBody;

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

        if (m_DistanceToPlayer < m_AggroDistance)
        {
            if(!CR_RUNNING)
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

        switch (gameObject.name)
        {
            case "CannonRight":
                m_BallBody.AddForce(new Vector2(1f, 0f) * m_FireForce);
                break;
            case "CannonLeft":
                m_BallBody.AddForce(new Vector2(-1f, 0f) * m_FireForce);
                break;
            case "CannonAbove":
                m_BallBody.AddForce(new Vector2(0f, -1f) * m_FireForce);
                break;
        }

        yield return new WaitForSeconds(fireInterval);

        CR_RUNNING = false;
    }
}
