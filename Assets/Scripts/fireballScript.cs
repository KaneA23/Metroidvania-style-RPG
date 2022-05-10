using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballScript : MonoBehaviour
{
	BasePlayerClass BPC;

	GameObject eventSystem;

	public float speed = -20f;
	Rigidbody2D rb;

	[Header("Enemy values")]
	public LayerMask enemyLayers;   // items the player can attack

	ProjectileAnimationSystem PAS;

	bool isMidair;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		eventSystem = GameObject.Find("EventSystem");
		BPC = eventSystem.GetComponent<BasePlayerClass>();

		PAS = GetComponent<ProjectileAnimationSystem>();

		rb.velocity = transform.right * speed;

		PAS.ChangeAnimationState(ProjectileAnimationState.FIREBALL_SHOT);
		isMidair = false;
		Debug.Log("Feur Frei!");
		Invoke(nameof(CompleteAnim), 0.18f);
		//isMidair = true;
	}

	// Update is called once per frame
	void Update()
	{
		//if (GetComponent<CircleCollider2D>().bounds.Intersects())
		//Fireball();
		if (isMidair)
		{
			Debug.Log("Free Flying");
			PAS.ChangeAnimationState(ProjectileAnimationState.FIREBALL_MIDAIR);
		}
	}

	void Fireball()
	{
		Collider2D hitEnemy = Physics2D.OverlapCircle(gameObject.transform.position, 0.75f, enemyLayers);
		Debug.Log(hitEnemy.gameObject.name);

		if (hitEnemy != null)
		{
			Debug.Log("Enemy Hit");
			hitEnemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(/*BPC.rangeAtkDamage*/25, gameObject.transform.position/*, BPC.lightKnockbackDist*/);

			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D a_collision)
	{
		//Destroy(gameObject);
		if (a_collision.gameObject.CompareTag("Player") || a_collision.gameObject.CompareTag("Projectile"))
		{
			return;
		}

		Debug.Log(a_collision.tag);

		if (a_collision.gameObject.CompareTag("Enemy"))
		{
			Debug.Log("HitEnemy");
			a_collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(/*BPC.rangeAtkDamage*/25, gameObject.transform.position/*, BPC.lightKnockbackDist*/);

		}

		//gameObject.SetActive(false);

		//Destroy(this.gameObject);
		rb.velocity = Vector3.zero;
		PAS.ChangeAnimationState(ProjectileAnimationState.FIREBALL_HIT);
		isMidair = false;
		Invoke(nameof(CompleteAnim), 0.35f);
		Debug.Log("is fireball destroyed?");
	}

	private void OnDrawGizmosSelected()
	{
		// Need to manually add Gizmo ranges to work, can't reference other script
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(gameObject.transform.position, 0.75f);
	}

	void CompleteAnim()
	{
		if (PAS.currentAnimState == "Fireball_Shot")
		{
			isMidair = true;
		}
		else if (PAS.currentAnimState == "Fireball_Hit")
		{
			Debug.Log("no witnesses!");
			Destroy(gameObject);
		}
	}
}
