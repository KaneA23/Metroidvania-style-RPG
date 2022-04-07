using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SwingHit : MonoBehaviour
{
    [SerializeField] private AISetUp AISU;
    [SerializeField] private PlayerHealthSystem PHS;

    private GameObject m_Player;
    public GameObject EventSystem;

    private Rigidbody2D m_PlayerBody;

    private int m_DamageAmount;
    private float m_SwingTrapForce;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();;
        EventSystem = GameObject.Find("EventSystem");

        m_Player = AISU.m_ActivePlayer;
        m_PlayerBody = m_Player.GetComponent<Rigidbody2D>();
        PHS = m_Player.GetComponent<PlayerHealthSystem>();

        m_DamageAmount = EventSystem.GetComponent<TrapValues>().swingTrapDamage;
        m_SwingTrapForce = EventSystem.GetComponent<TrapValues>().swingTrapForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks to see if the swing trap has collided with the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D otherObject = collision.collider;

        if(otherObject.gameObject == m_Player)
        {
            PHS.TakeDamage(m_DamageAmount, gameObject.transform.position, m_SwingTrapForce, true);

            if (gameObject.transform.position.x > m_Player.transform.position.x)
            {
                m_PlayerBody.AddForce(new Vector2(-1, 0) * m_SwingTrapForce, ForceMode2D.Impulse);
            }
            else if (gameObject.transform.position.x < m_Player.transform.position.x)
            {
                m_PlayerBody.AddForce(new Vector2(1, 0) * m_SwingTrapForce, ForceMode2D.Impulse);
            }
        }
    }
}
