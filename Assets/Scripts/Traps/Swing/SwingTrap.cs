using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTrap : MonoBehaviour
{
    private AISetUp AISU;
    private PlayerHealthSystem PHS;

    private GameObject m_Player;
    public GameObject m_SwingObject;
    private GameObject m_DamageObject;

    private bool m_Dropped = false;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        PHS = AISU.PHS;

        m_Player = AISU.m_ActivePlayer;

        m_DamageObject = m_SwingObject.GetComponentInChildren<Collider2D>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == m_Player)
        {
            if (!m_Dropped)
            {
                Drop();
            }
        }
    }

    private void Drop()
    {
        m_SwingObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        m_SwingObject.GetComponent<Rigidbody2D>().mass = 5;
        m_Dropped = true;
    }
}
