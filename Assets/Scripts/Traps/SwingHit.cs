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

    private int m_DamageAmount;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();;
        EventSystem = GameObject.Find("EventSystem");

        m_Player = AISU.m_ActivePlayer;
        PHS = m_Player.GetComponent<PlayerHealthSystem>();

        m_DamageAmount = EventSystem.GetComponent<TrapValues>().swingTrapDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D otherObject = collision.collider;

        if(otherObject.gameObject == m_Player)
        {
            PHS.TakeDamage(m_DamageAmount, gameObject.transform.position);
        }
    }
}
