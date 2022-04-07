using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    private AISetUp AISU;
    private PlayerHealthSystem PHS;

    private GameObject m_Player;

    public int m_DamageAmount;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
        PHS = AISU.PHS;

        m_Player = AISU.m_ActivePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == m_Player)
        {
            PHS.TakeDamage(m_DamageAmount, gameObject.transform.position, 0f, false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
