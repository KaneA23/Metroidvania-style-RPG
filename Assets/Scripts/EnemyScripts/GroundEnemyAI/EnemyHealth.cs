using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
	public float m_MaxHP = 100;
	public float m_CurrentHP;

	public float HitForce = 250;

	Rigidbody2D rb;

	//public Slider m_HealthBar;

	private SpriteRenderer m_SpriteRenderer;

	private void Awake()
	{
		//m_HealthBar = GetComponentInChildren<Slider>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();

		rb = GetComponent<Rigidbody2D>();
	}

	// Start is called before the first frame update
	void Start()
	{
		m_CurrentHP = m_MaxHP;

		//m_HealthBar.maxValue = m_MaxHP;
		//m_HealthBar.value = m_CurrentHP;
	}

	public void TakeDamage(float a_Damage, Vector2 a_PlayerPos)
	{
		m_CurrentHP -= a_Damage;
		//m_HealthBar.value = m_CurrentHP;

		if (m_CurrentHP <= 0)
		{
			Debug.Log("<color=Purple>Get Rekt Son</color>");
			Destroy(m_SpriteRenderer);
			Invoke(nameof(KillEnemy), 0.5f);
		}

		if ((transform.position.x - a_PlayerPos.x) < 0)
		{
			Debug.Log("Left Hit");
			rb.AddForce(new Vector2(-1f, 0.5f) * HitForce);
		}
		else if ((transform.position.x - a_PlayerPos.x) > 0)
		{
			Debug.Log("Right Hit");
			rb.AddForce(new Vector2(1f, 0.5f) * HitForce);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	void KillEnemy()
	{
		Destroy(gameObject);
	}
}
