using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    public AISetUp AISU;
    public PlayerHealthSystem PHS;

    public GameObject m_Player;
    public GameObject m_Enemy;

    public Rigidbody2D m_PlayerBody;
    public Collider2D m_ChunkCollider;

    public Vector2 m_AttackDirection;

    public ParticleSystem m_EarthExplosion;
    private ParticleSystem EarthBreak;

    public float m_AttackForce;
    public int m_DamageAmount;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.GetComponent<AISetUp>().m_ActivePlayer;

        PHS = m_Player.GetComponent<PlayerHealthSystem>();

        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Physics2D.IgnoreLayerCollision(3, 7);
        Physics2D.IgnoreLayerCollision(7, 7);

        if (m_Enemy == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D otherObject = collision.collider;

        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);

        if (gameObject.CompareTag("AirAttack"))
        {
            EarthBreak = Instantiate(m_EarthExplosion, transform.position, Quaternion.identity);

            //float particleDuration = EarthBreak.main.duration;
            EarthBreak.Play();
        }

        if (otherObject.gameObject == m_Player)
        {
            PHS.TakeDamage(m_DamageAmount, enemyPos, m_AttackForce, true);

            if (gameObject.CompareTag("GroundAttack"))
            {
                float heightForce = Random.Range(0.5f, 1f);

                if (m_Player.transform.position.x < transform.position.x)
                {
                    m_AttackDirection = new Vector2(-1f, heightForce) * m_AttackForce;
                }
                else if (m_Player.transform.position.x > transform.position.x)
                {
                    m_AttackDirection = new Vector2(1f, heightForce) * m_AttackForce;
                }
                else
                {
                    m_AttackDirection = new Vector2(0f, heightForce) * m_AttackForce;
                }

                m_PlayerBody.AddForce(m_AttackDirection, ForceMode2D.Impulse);
            }
            else if (gameObject.CompareTag("AirAttack"))
            {

                if (m_Player.transform.position.x < transform.position.x)
                {
                    m_AttackDirection = new Vector2(-1f, 0f) * m_AttackForce;
                }
                else if (m_Player.transform.position.x > transform.position.x)
                {
                    m_AttackDirection = new Vector2(1f, 0f) * m_AttackForce;
                }
                else if (m_Player.transform.position == transform.position)
                {
                    m_AttackDirection = new Vector2(0f, 0f);
                }

                m_PlayerBody.AddForce(m_AttackDirection, ForceMode2D.Impulse);

                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
