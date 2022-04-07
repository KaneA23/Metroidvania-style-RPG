using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

public class AIRanged : MonoBehaviour
{
	public PlayerHealthSystem PHS;
	public EnemyHealth EH;
	public AISetUp AISU;
	public EnemyAnimationManager EAM;   // Controls enemy animations

	public GameObject m_Player;

	public Transform target;
	public float speed = 200f;
	public float nextWaypointDistance = 3f;
	public float m_AttackDistance = 5.5f;
	public float m_HitForce;

	public Transform enemyGraphics;

	private Vector3 m_TargetPos;
	[SerializeField] private Vector3 m_HoverPos;
	public Vector3 m_HoverDistance;
	private float m_Distance;

	public int m_DamageAmount;
	private int m_Timer = 0;

	Path path;
	int currentWaypoint = 0;
	bool reachedEndOfPath = false;

	public bool attacking = false;

	public bool isAlert;
	public bool isForget;
	public bool isAgro;

	Seeker seeker;
	Rigidbody2D rb;

	public float animDelay;

	private void Awake()
	{
		EAM = GetComponent<EnemyAnimationManager>();
	}

	// Start is called before the first frame update
	public void Start()
	{
		AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();
		PHS = AISU.PHS;

		m_Player = AISU.m_ActivePlayer;

		target = m_Player.transform;

		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		InvokeRepeating("UpdatePath", 0f, .5f);

		isAlert = false;
		isForget = false;
		isAgro = false;
		EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_CALMWALK);
	}

	void UpdatePath()
	{
		if (seeker.IsDone())
		{
			seeker.StartPath(rb.position, m_HoverPos, OnPathComplete);
		}
	}

	void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Collider2D otherCollider = collision.collider;
		Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);

		if (otherCollider.name == m_Player.tag)
		{
			PHS.TakeDamage(m_DamageAmount, enemyPos);
			//EH.TakeDamage(m_DamageAmount, enemyPos);

			if ((transform.position.x - otherCollider.transform.position.x) < 0)
			{
				Debug.Log("Left Hit");
				//rb.AddForce(new Vector2(-1f, 0f) * m_HitForce);
			}
			else if ((transform.position.x - otherCollider.transform.position.x) > 0)
			{
				Debug.Log("Right Hit");
				//rb.AddForce(new Vector2(1f, 0f) * m_HitForce);
			}
		}

		if (otherCollider.CompareTag("Enemy"))
		{
			Physics2D.IgnoreCollision(otherCollider, GetComponent<Collider2D>());
		}
	}

	void Update()
	{
		if (isAgro && !isAlert)
		{
			EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_AGROWALK);
		}
		else if (!isAgro && !isForget)
		{
			EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_CALMWALK);
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 velocity = rb.velocity;
		float a_speed = velocity.magnitude;

		Vector3 enemyPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		Vector3 playerPos = new Vector3(target.position.x, target.position.y, target.position.z);

		if (enemyPos.magnitude > playerPos.magnitude + 6)
		{
			if (rb.velocity.magnitude > 1)
			{
				rb.velocity = Vector3.zero;
			}
		}

		if (target.position.x < transform.position.x)
		{
			//m_TargetPos = target.position + new Vector3(3f, 5f);

			if (m_HoverDistance.y < 0f)
			{
				m_HoverDistance.y = System.Math.Abs(-m_HoverDistance.y);
			}

			m_TargetPos = target.position + m_HoverDistance;
		}
		else if (target.position.x > transform.position.x)
		{
			//m_TargetPos = target.position - new Vector3(3f, -5f);

			if (m_HoverDistance.y > 0f)
			{
				m_HoverDistance.y *= -1f;
			}

			m_TargetPos = target.position - m_HoverDistance;
		}

		m_HoverPos = m_TargetPos;
		m_Distance = Vector2.Distance(m_HoverPos, transform.position);

		if (path == null)
		{
			return;
		}

		if (currentWaypoint >= path.vectorPath.Count)
		{
			reachedEndOfPath = true;
			return;
		}
		else
		{
			reachedEndOfPath = false;
		}

		Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
		Vector2 force = direction * speed * Time.deltaTime;

		rb.AddForce(force);

		float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
		float finalDistance = Vector2.Distance(rb.position, path.vectorPath[path.vectorPath.Count - 1]);
		//Debug.Log(distance);

		if (distance < nextWaypointDistance)
		{
			currentWaypoint++;
		}

		//Debug.Log(a_speed);

		if (finalDistance < m_AttackDistance && !isAlert && !isAgro)
		{
			isAlert = true;
			EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_ALERT);
			animDelay = 1.12f;
			Invoke(nameof(CompleteAnim), animDelay);

		}
		else if (finalDistance > m_AttackDistance && !isForget && isAgro)
		{
			isForget = true;
			EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_FORGET);
			animDelay = 1.12f;
			Invoke(nameof(CompleteAnim), animDelay);

		}

		if (finalDistance < m_AttackDistance)
		{
			if (!isAlert && !isAgro)
			{
				isAlert = true;
				EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_ALERT);
				animDelay = 1.117f;
				Invoke(nameof(CompleteAnim), animDelay);
			}
		}
		else
		{
			if (isAlert && isAgro)
			{
				isAlert = true;
				EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_FORGET);
				animDelay = 1.117f;
				Invoke(nameof(CompleteAnim), animDelay);

			}
		}

		if (finalDistance < 1.6f)
		{
			rb.velocity = Vector2.zero;
			attacking = true;


			//Debug.Log("reached");
			//if(rb.position.x < target.position.x)
			//{
			//    //Debug.Log("reached");
			//    if (a_speed < 1f)
			//    {
			//        //Debug.Log("reached");
			//        rb.AddForce(-velocity, ForceMode2D.Force);
			//        //rb.AddForce(new Vector2(-10f, 0), ForceMode2D.Force);
			//    }
			//    //else if (rb.velocity.x > 1f)
			//    //{
			//    //    //Debug.Log("reached");
			//    //    rb.AddForce(new Vector2(-20f, 0), ForceMode2D.Force);
			//    //}
			//}
			//else if(rb.position.x > target.position.x)
			//{
			//    if (a_speed < 0.5f)
			//    {
			//        //Debug.Log("reached");
			//        rb.AddForce(velocity, ForceMode2D.Force);
			//        //rb.AddForce(new Vector2(5, 0), ForceMode2D.Force);
			//    }
			//    //else if (rb.velocity.x > 1f)
			//    //{
			//    //    //Debug.Log("reached");
			//    //    rb.AddForce(new Vector2(10, 0), ForceMode2D.Force);
			//    //}
			//}         
		}
		else
		{
			//if (attacking)
			//{
			//	EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_FORGET);
			//	animDelay = 1.12f;
			//	Invoke(nameof(CompleteAnim), animDelay);

			attacking = false;
			//}

		}

		if (rb.transform.position.x < target.position.x)
		{
			enemyGraphics.localScale = new Vector3(-1f, 1f, 1f);
		}
		else if (rb.transform.position.x > target.position.x)
		{
			enemyGraphics.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	void CompleteAnim()
	{
		Debug.Log("Completing animation");
		if (isAlert)
		{
			Debug.Log("Angery");
			//EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_AGROWALK);
			isAlert = false;
			isAgro = true;
		}
		else if (isForget)
		{
			Debug.Log("Chillin'");
			//EAM.ChangeAnimationState(AIAnimationState.EARTHELEMENTAL_CALMWALK);
			isForget = false;
			isAgro = false;
		}
	}
}
