using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindingNew : MonoBehaviour
{
	public PlayerHealthSystem PHS;
	public EnemyHealth EH;
	public AISetUp AISU;
	public EnemyAnimationManager EAM;

	public float m_Speed;
	public float m_AttackDistance;
	public float HitForce;
	float minDistance = Mathf.Infinity;

	public bool m_MovingToTarget;

	[SerializeField] private Transform m_Player;

	public GameObject[] m_Enemies;
	public GameObject[] m_Players;
	//private GameObject m_ActivePlayer;

	private Vector3 m_TargetPos;
	private Vector2 m_TargetDir;
	private Vector3 m_MovementDirection;
	private Vector3 m_NewDestination;
	private Vector3 m_CurrentPos;
	private Vector3 m_Velocity;

	private float m_OrigPos;

	public int m_DamageAmount;

	private char m_TravelDirection;

	private SpriteRenderer m_SpriteRenderer;

	private Rigidbody2D rb;

	public bool isAlert;
	public bool isForget;
	public bool isAgro;
	private bool isFacingRight;

	public float animDelay;

	private void Awake()
	{
		EAM = GetComponent<EnemyAnimationManager>();
	}

	private void Start()
	{
		AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

		PHS = AISU.PHS;

		m_SpriteRenderer = GetComponent<SpriteRenderer>();

		m_Player = GameObject.Find("Player").GetComponent<Transform>();

		m_OrigPos = transform.position.x;

		rb = GetComponent<Rigidbody2D>();

		m_Player = AISU.m_ActivePlayer.transform;

		EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_IDLE);

		isAlert = false;
		isForget = false;
		isAgro = false;
	}

	//Transform GetClosestEnemy(Transform[] Enemies)
	//{
	//	Transform tMin = null;

	//	foreach (Transform t in Enemies)
	//	{
	//		float distance = Vector3.Distance(t.position, transform.position);
	//		if (distance < minDistance)
	//		{
	//			tMin = t;
	//			minDistance = distance;
	//		}
	//	}
	//	return tMin;
	//}

	private void EnemyFacing()
	{
		if(gameObject != null)
        {
			if (m_TargetDir.x > 0)
			{
				m_SpriteRenderer.flipX = false;
			}

			if (m_TargetDir.x < 0)
			{
				m_SpriteRenderer.flipX = true;
			}
		}
	}

	#region __CHECK_COLLISIONS__

	//[SerializeField] private int collisionCount = 0;

	//public bool NotColliding
	//{
	//	get { return collisionCount == 0; }
	//}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Collider2D otherCollider = collision.collider;

		//if (otherCollider.name != "Floor" || otherCollider.tag != "Enemy")
		//{
		//	//Debug.Log(collider.name);
		//	collisionCount++;
		//}

		if (otherCollider.name == AISU.m_ActivePlayer.tag)
		{
			PHS.TakeDamage(m_DamageAmount, gameObject.transform.position, HitForce, true);
			//EH.TakeDamage(m_DamageAmount);

			Debug.Log("collision");

			//if ((transform.position.x - otherCollider.transform.position.x) < 0)
			//{
			//	Debug.Log("Left Hit");
			//	rb.AddForce(new Vector2(-1f, 0.5f) * HitForce);
			//}
			//else if ((transform.position.x - otherCollider.transform.position.x) > 0)
			//{
			//	Debug.Log("Right Hit");
			//	rb.AddForce(new Vector2(1f, 0.5f) * HitForce);
			//}
		}

		if (otherCollider.CompareTag("Enemy"))
		{
			Physics2D.IgnoreCollision(otherCollider, GetComponent<Collider2D>());
		}

		if (otherCollider.CompareTag("GroundAttack"))
		{
			Physics2D.IgnoreCollision(otherCollider, GetComponent<Collider2D>());
		}

	}

	//private void OnCollisionExit2D(Collision2D collision)
	//{
	//	collisionCount--;
	//}

	#endregion __CHECK_COLLISIONS_END__



	//private void GetPlayers()
	//{
	//    m_Players = GameObject.FindGameObjectsWithTag("Player");

	//    foreach(GameObject p in m_Players)
	//    {
	//        if(p.activeSelf)
	//        {
	//            PHS = p.GetComponent<PlayerHealthSystem>();
	//            m_ActivePlayer = p;
	//        }
	//    }
	//}

	private void Update()
	{
		//GetPlayers();

		m_Velocity = rb.velocity.normalized;

		if(m_SpriteRenderer.flipX == true)
		{
			isFacingRight = false;
		}
		else if (m_SpriteRenderer.flipX == false)
		{
			isFacingRight = true;
		}

		if (isAgro && !isAlert)
		{
			EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_ATTACK);
		}
		else if (!isAgro && !isForget)
		{
			EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_WALK);
		}

		m_CurrentPos = transform.position;

		m_TargetPos = new Vector2(m_Player.position.x, gameObject.transform.position.y);
		m_TargetDir = (m_TargetPos - transform.position).normalized;

		EnemyFacing();

		//if (m_MovementDirection != Vector3.zero)
		//{
		//	float angle = Mathf.Atan2(m_MovementDirection.y, m_MovementDirection.x) * Mathf.Rad2Deg;
		//	transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		//}

		if (Vector2.Distance(transform.position, m_TargetPos) > m_AttackDistance /*|| Vector2.Distance(transform.position, m_ClosestEnemyPos) > m_AttackDistance*/)
		{
			rb.AddForce(m_TargetDir * m_Speed);
			m_MovingToTarget = true;
			//transform.position = Vector2.MoveTowards(transform.position, m_TargetPos, m_Speed * Time.deltaTime);

			if (!isAlert && !isAgro)
			{
				isAlert = true;
				EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_ALERT);
				animDelay = 0.383f;
				Invoke(nameof(CompleteAnim), animDelay);
			}
		}
		else
		{
			m_MovingToTarget = false;

			if (!isForget && isAgro)
			{
				isForget = true;
				EAM.ChangeAnimationState(AIAnimationState.FIREELEMENTAL_FORGET);
				animDelay = 0.383f;
				Invoke(nameof(CompleteAnim), animDelay);
			}
		}
	}

	void CompleteAnim()
	{
		if (isAlert)
		{
			isAlert = false;
			isAgro = true;
		}
		else if (isForget)
		{
			isForget = false;
			isAgro = false;
		}
	}
}
