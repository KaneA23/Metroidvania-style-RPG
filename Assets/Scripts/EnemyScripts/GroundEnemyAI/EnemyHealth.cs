using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
	public BernardHealthUI BHUI;

	public float m_MaxHP = 100;
	public float m_CurrentHP;

	public float HitForce = 250;

	[SerializeField] private int startTime;
	private bool timerStart = false;

	Rigidbody2D rb;

	public Animator transition;
	public float transitionTime = 1f;

	//public Slider m_HealthBar;

	private SpriteRenderer m_SpriteRenderer;

	private void Awake()
	{
		//m_HealthBar = GetComponentInChildren<Slider>();
		m_SpriteRenderer = GetComponent<SpriteRenderer>();

		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		m_CurrentHP = m_MaxHP;

		//m_HealthBar.maxValue = m_MaxHP;
		//m_HealthBar.value = m_CurrentHP;
	}

	public void TakeDamage(float a_Damage, Vector2 a_PlayerPos/*, float a_KnockbackAmount*/)
	{
		m_CurrentHP -= a_Damage;
		//m_HealthBar.value = m_CurrentHP;

		if (m_CurrentHP <= 0)
		{
			Debug.Log("<color=Purple>Get Rekt Son</color>");

			if(gameObject.name == "Bernard")
            {
				Destroy(m_SpriteRenderer);

				GetComponent<BoxCollider2D>().enabled = false;
				GetComponent<PolygonCollider2D>().enabled = false;

				InvokeRepeating("Timer", 0, 1);
            }
            else
            {
				Invoke(nameof(KillEnemy), 0.5f);
			}			
		}

		if ((transform.position.x - a_PlayerPos.x) < 0)
		{
			Debug.Log("Left Hit");
			rb.AddForce(new Vector2(-1f, 0.5f) */* a_KnockbackAmount*/ HitForce);
		}
		else if ((transform.position.x - a_PlayerPos.x) > 0)
		{
			Debug.Log("Right Hit");
			rb.AddForce(new Vector2(1f, 0.5f) * /*a_KnockbackAmount*/HitForce);
		}

		BHUI.healthlerpTimer = 0;
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void Timer()
    {
        if (startTime > 0)
        {
            startTime--;
        }
        else
        {
			//KillEnemy();
			StartCoroutine(LoadLevel("Main Menu Scene"));
		}
    }

	void KillEnemy()
	{
		Destroy(gameObject);
	}

	IEnumerator LoadLevel(string a_sceneName)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(a_sceneName);
	}
}
