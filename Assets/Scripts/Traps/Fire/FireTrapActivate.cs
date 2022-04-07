using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapActivate : MonoBehaviour
{
    private AISetUp AISU;
    private TrapValues TV;

    public GameObject fireTrap;
    private GameObject m_Player;
    public GameObject m_DamageZonePrefab;
    private GameObject m_DamageZone;

    private ParticleSystem fire;
    private ParticleSystem.Particle[] m_fireParticles;

    private Vector3 m_scaleDirection;

    public float m_endTime;
    private float m_restartTime;
    private float m_Speed = 0.08f;
    private bool m_activated = false;

    // Start is called before the first frame update
    void Start()
    {
        AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

        m_Player = AISU.m_ActivePlayer;

        m_restartTime = m_endTime;

        fire = fireTrap.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        //fire.GetParticles(m_fireParticles);

        if (m_activated == true)
        {
            DamageExtend();
            m_endTime -= Time.deltaTime;
            
        }

        if (m_endTime <= 0f)
        {
            m_activated = false;
            fire.Stop();
            Destroy(m_DamageZone);
            m_endTime = m_restartTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == m_Player)
        {
            fire.Play();
            m_activated = true;
        }
    }

    private void DamageExtend()
    {

        //m_fireParticles[1].position.x

        if (m_DamageZone == null)
        {
            m_DamageZone = Instantiate(m_DamageZonePrefab, fireTrap.transform.position, Quaternion.identity);
            m_DamageZone.GetComponent<FireDamage>().m_FireTrap = fireTrap;
        }

        switch (fireTrap.tag)
        {
            case "TrapLeft":
                m_scaleDirection = new Vector3(fireTrap.transform.localPosition.x - 1f, 0f, 0f).normalized;
                m_DamageZone.transform.position += m_scaleDirection * m_Speed / 2;
                m_DamageZone.transform.localScale += m_scaleDirection * m_Speed;
                break;
            case "TrapRight":
                m_scaleDirection = new Vector3(fireTrap.transform.localPosition.x + 1f, 0f, 0f).normalized;
                m_DamageZone.transform.position += m_scaleDirection * m_Speed / 2;
                m_DamageZone.transform.localScale += m_scaleDirection * m_Speed;
                break;
            case "TrapAbove":
                m_scaleDirection = new Vector3(0f, fireTrap.transform.localPosition.y - 1f, 0f).normalized;
                m_DamageZone.transform.position -= m_scaleDirection * m_Speed / 2;
                m_DamageZone.transform.localScale -= m_scaleDirection * m_Speed;
                break;
        }

        
    }
}
