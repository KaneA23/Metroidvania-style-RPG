using UnityEngine;

/// <summary>
/// Controls interactable doors that can be destroyed to allow exploration.
/// Created by: Kane Adams
/// </summary>
public class TrapDoorScript : MonoBehaviour
{
	[SerializeField] private GameObject[] enemies;
	[SerializeField] string enemyTag;

	public GameObject trapdoor;

	public GameObject enemyFloor;

	public GameObject visualCue;

	public GameObject monolith;

	[SerializeField] private ParticleSystem[] trapDoorParticles;
	[SerializeField] private ParticleSystem.EmissionModule[] emissionMod = new ParticleSystem.EmissionModule[4];

	public float checkRadius;
	public LayerMask playerMask;

	[SerializeField] private ParticleSystem[] trapExplosions;

	// Start is called before the first frame update
	void Start()
	{
		enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		//foreach (ParticleSystem particle in trapDoorParticles)
		//{
		//	em[] = particle.emission;
		//}
		monolith.SetActive(false);
		for (int i = 0; i < trapDoorParticles.Length; i++)
		{
			emissionMod[i] = trapDoorParticles[i].emission;
		}

	}

	// Update is called once per frame
	void Update()
	{
		enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		//if (enemies.Length != 0)
		//{
		//	Debug.Log("There are enemies about!");
		//}
		//else
		//{
		//	Debug.Log("Oh... nevermind!");
		//}

		if (enemies.Length == 0)
		{
			//foreach(ParticleSystem.EmissionModule em in emissionMod)
			//{
			//	em.enabled = true;
			//}

			Destroy(enemyFloor);

			monolith.SetActive(true);
			for (int i = 0; i < emissionMod.Length; i++)
			{
				emissionMod[i].enabled = true;
			}

			//emissionMod.enabled = true;

			// If player is in reach of monolith and interacts, trapdoor is opened
			if (Physics2D.OverlapCircle(gameObject.transform.position, checkRadius, playerMask))
			{
				visualCue.SetActive(true);
				if (Input.GetKeyDown(KeyCode.F))
				{
					//trapExplosion.Play();
					for (int i = 0; i < trapExplosions.Length; i++)
					{
						trapExplosions[i].Play();
					}

					for (int i = 0; i < emissionMod.Length; i++)
					{
						emissionMod[i].enabled = false;
					}

					monolith.SetActive(false);
					visualCue.SetActive(false);
					
					Invoke(nameof(RemoveTrapDoor), 1f);
				}
			}
			else
			{
				visualCue.SetActive(false);
			}
			//trapDoorParticle.Play();
			//trapdoor.SetActive(false);
		}
		else
		{
			//emissionMod.enabled = false;
			for (int i = 0; i < emissionMod.Length; i++)
			{
				emissionMod[i].enabled = false;
			}
			visualCue.SetActive(false);
		}
	}

	/// <summary>
	/// Opens up trap door for player
	/// </summary>
	void RemoveTrapDoor()
	{
		monolith.SetActive(false);
		visualCue.SetActive(false);
		trapdoor.SetActive(false);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, checkRadius);
	}
}
