using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AirRangeAttack : MonoBehaviour
{
    AIRanged AIR;

    private GameObject m_Player;
    public GameObject m_AirProjectilePrefab;
    private GameObject m_AirProjectile;

    private Vector3 m_FirePosition;

    public float m_ProjectileSpeed = 5f;
    public float m_FireInterval = 0.5f;

    private bool m_Attacking;
    private bool CR_RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        AIR = GetComponent<AIRanged>();
        m_Player = GameObject.Find("AI_Setup").GetComponent<AISetUp>().m_ActivePlayer;
    }

    private void FixedUpdate()
    {
        Vector2 fireDirection = (m_Player.transform.position - m_FirePosition).normalized;

        if (m_Player.transform.position.x < transform.position.x)
        {
            m_FirePosition = new Vector3(transform.position.x - 1f, transform.position.y);
        }
        else if (m_Player.transform.position.x > transform.position.x)
        {
            m_FirePosition = new Vector3(transform.position.x + 1f, transform.position.y);
        }

        if (AIR.attacking)
        {
            m_Attacking = true;

            if (!CR_RUNNING)
            {
                StartCoroutine(Fire(fireDirection));
            }
        }
        else
        {
            CR_RUNNING = false;
            m_Attacking = false;
        }
    }
    //thing
    public IEnumerator Fire(Vector2 fireDirection)
    {
        CR_RUNNING = true;

        while (m_Attacking)
        {           
            m_AirProjectile = Instantiate(m_AirProjectilePrefab, m_FirePosition, Quaternion.identity);

            if (m_AirProjectile != null)
            {
                m_AirProjectile.GetComponent<Rigidbody2D>().AddForce(fireDirection * m_ProjectileSpeed, ForceMode2D.Impulse);
            }

            Destroy(m_AirProjectile);

            yield return new WaitForSeconds(m_FireInterval);   
        }       
    }
}
