using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float m_MaxHP = 100;
    public float m_CurrentHP;

    public Slider m_HealthBar;

    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_HealthBar = GetComponentInChildren<Slider>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentHP = m_MaxHP;

        m_HealthBar.maxValue = m_MaxHP;
        m_HealthBar.value = m_CurrentHP; 
    }

    public void TakeDamage(float a_Damage)
    {
        m_CurrentHP -= a_Damage;
        m_HealthBar.value = m_CurrentHP;

        if(m_CurrentHP <= 0)
        {
            Debug.Log("<color=Purple>Get Rekt Son</color>");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
