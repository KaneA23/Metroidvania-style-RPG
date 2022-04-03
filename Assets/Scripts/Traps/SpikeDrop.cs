using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDrop : MonoBehaviour
{
    private AISetUp AISU;

    private GameObject m_Player;
    public GameObject m_SpikesPrefab;
    private GameObject m_Spikes;

    private Vector2 m_DropPos;

    private bool m_TimerStart = false;

    private float m_Timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;

        m_DropPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_TimerStart)
        {
            m_Timer++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == m_Player)
        {
            Drop();
        }
    }

    private void Drop()
    {
        m_Spikes = Instantiate(m_SpikesPrefab, m_DropPos, Quaternion.identity);
        m_TimerStart = true;
    }
}
