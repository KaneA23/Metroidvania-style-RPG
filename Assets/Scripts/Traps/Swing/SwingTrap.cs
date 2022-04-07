using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTrap : MonoBehaviour
{
    private AISetUp AISU;
    private PlayerHealthSystem PHS;

    private GameObject m_Player;
    public GameObject m_SwingObjectPrefab;
    private GameObject m_SwingObject;
    private GameObject m_DamageObject;

    private bool m_Dropped = false;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        PHS = AISU.PHS;

        m_Player = AISU.m_ActivePlayer;
    }

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
        m_SwingObject = Instantiate(m_SwingObjectPrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 5f), Quaternion.identity);
        m_SwingObject.transform.GetChild(1).gameObject.GetComponent<HingeJoint2D>().connectedBody = m_SwingObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>();
        m_DamageObject = m_SwingObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        m_SwingObject.transform.GetChild(0).GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        m_SwingObject.transform.GetChild(0).GetComponent<Rigidbody2D>().mass = 5;
        m_Dropped = true;
    }
}
