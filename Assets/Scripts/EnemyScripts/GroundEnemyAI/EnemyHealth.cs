using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
	public BernardHealthUI BHUI;
	public BernardAttacking BA;

	public float m_MaxHP = 100;
	public float m_CurrentHP;

	public float HitForce = 250;

	[SerializeField] private int startTime;
	private bool timerStart = false;
	public bool isDead = false;
	public bool isDeadOnce = false;
	public bool animFinished = false;

	Rigidbody2D rb;

	public Animator transition;
	public float transitionTime = 1f;

	//public Slider m_HealthBar;

	private SpriteRenderer m_SpriteRenderer;

	public int xp = 25;

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

		//if (m_CurrentHP <= 0)
		//{
		//	Debug.Log("<color=Purple>Get Rekt Son</color>");
		//	isDead = true;

		//          if (gameObject.name == "Bernard")
		//          {
		//		//Destroy(m_SpriteRenderer);

		//		//InvokeRepeating("Timer", 0, 1);

		//		if(BA.deathAnimFinished)
		//              {
		//			Invoke(nameof(KillEnemy), 0.5f);
		//              }
		//          }
		//          else
		//          {
		//              if (animFinished)
		//              {
		//			Invoke(nameof(KillEnemy), 0.5f);
		//		}           							
		//	}			
		//}

		if (!isDead)
		{
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
		}

		BHUI.healthlerpTimer = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (m_CurrentHP <= 0 && !isDeadOnce)
		{
			Debug.Log("<color=Purple>Get Rekt Son</color>");
			isDead = true;
			isDeadOnce = true;

			gameObject.tag = "EnemyCorpse";
			if (gameObject.GetComponent<SpriteRenderer>() != null)
			{
				gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
			}
			else
			{
				gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
			}

			GameObject.Find("ExperienceBar").GetComponent<LevelSystem>().GainExperience(xp);

			if (gameObject.name == "Bernard")
			{
				FindObjectOfType<CameraFollowScript>().cameraState = CameraState.CAM_FOLLOWING;
				//Destroy(m_SpriteRenderer);

				//InvokeRepeating("Timer", 0, 1);
			}
			else
			{
				//if (animFinished)
				//{
				//    Invoke(nameof(KillEnemy), 0.5f);
				//}
			}
		}
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
