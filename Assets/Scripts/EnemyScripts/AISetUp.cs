using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISetUp : MonoBehaviour
{
    public PlayerHealthSystem PHS;

    public GameObject[] m_Players;
    public GameObject m_ActivePlayer;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        GetPlayers();
    }

    private void GetPlayers()
    {
        m_Players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in m_Players)
        {
            if (p.active == true)
            {
                PHS = p.GetComponent<PlayerHealthSystem>();
                m_ActivePlayer = p;
            }
        }
    }
}
