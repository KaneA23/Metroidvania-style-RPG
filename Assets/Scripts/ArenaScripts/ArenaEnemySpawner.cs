using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls how many enemies spawn each round.
/// Created by: Kane Adams
/// </summary>
public class ArenaEnemySpawner : MonoBehaviour
{
	[Header("Referenced Scripts")]
	private ArenaWaveManager AWM;

	[Header("Enemy Spawns")]
	[SerializeField] private GameObject enemyPrefab;

	public List<GameObject> enemySpawns;

	public GameObject previousSpawn;	// prevents 2 enemies spawning on top of eachother

	//public int wave;
	int round;

	[Header("Enemies per wave")]
	public int wave1Count;
	public int wave2Count;
	public int wave3Count;
	public int wave4Count;
	public int wave5Count;
	public int wave6Count;
	public int wave7Count;
	public int wave8Count;
	public int wave9Count;
	public int wave10Count;

	public int spawnNumber;

	private void Awake()
	{
		AWM = GetComponent<ArenaWaveManager>();
	}

	private void Start()
	{
		enemyPrefab.GetComponent<EnemyHealth>().m_MaxHP = 200;

		// Earth Elemental attack strength
		if (enemyPrefab.GetComponent<AIRanged>() != null)
		{
			enemyPrefab.GetComponent<AIRanged>().m_DamageAmount = 5; 
		}

		// Fire Elemental attack strength and speed
		if (enemyPrefab.GetComponent<EnemyPathfindingNew>() != null)
		{
			enemyPrefab.GetComponent<EnemyPathfindingNew>().m_DamageAmount = 1;
			enemyPrefab.GetComponent<EnemyPathfindingNew>().m_Speed = 5;
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Debug
		//if (Input.GetKeyDown(KeyCode.T))
		//{
		//	spawnNumber = 0;
		//}
	}

	/// <summary>
	/// Selects a random spawn point and adds enemy to that point
	/// </summary>
	public void SpawnEnemy()
	{
		GameObject randomSpawn = enemySpawns[Random.Range(0, enemySpawns.Count)];

		// if enemy just spawned at point, wait before spawning again
		if (randomSpawn == previousSpawn)
		{
			Invoke(nameof(SpawnEnemy), 0.1f);
		}
		else
		{
			Vector2 spawnPoint = new Vector2(randomSpawn.transform.position.x + Random.Range(-5, 5), randomSpawn.transform.position.y);

			Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
			previousSpawn = randomSpawn;
		}
	}

	/// <summary>
	/// Changes number of enemies dependent on wave
	/// </summary>
	/// <param name="a_waveNum">Next wave</param>
	public void CheckWave(int a_waveNum)
	{
		round = a_waveNum;
		
		// Aster 10 rounds, number of enemies loop but are stronger
		if (round > 10)
		{
			IncreaseEnemyStats();
			CheckWave(round - 10);
		}
		else
		{
			switch (round)
			{
				case 1:
					spawnNumber = wave1Count;
					break;

				case 2:
					spawnNumber = wave2Count;
					break;

				case 3:
					spawnNumber = wave3Count;
					break;

				case 4:
					spawnNumber = wave4Count;
					break;

				case 5:
					spawnNumber = wave5Count;
					break;

				case 6:
					spawnNumber = wave6Count;
					break;

				case 7:
					spawnNumber = wave7Count;
					break;

				case 8:
					spawnNumber = wave8Count;
					break;

				case 9:
					spawnNumber = wave9Count;
					break;

				case 10:
					spawnNumber = wave10Count;
					break;

				default:
					spawnNumber += round;
					break;
			}
		}
	}

	/// <summary>
	/// Enemies health and attack strength increases to add difficulty
	/// </summary>
	void IncreaseEnemyStats()
	{
		enemyPrefab.GetComponent<EnemyHealth>().m_MaxHP += 10;

		// Earth Elemental attack strength
		if (enemyPrefab.GetComponent<AIRanged>() != null)
		{
			enemyPrefab.GetComponent<AIRanged>().m_DamageAmount++;
		}

		// Fire Elemental attack strength and speed
		if (enemyPrefab.GetComponent<EnemyPathfindingNew>() != null)
		{
			enemyPrefab.GetComponent<EnemyPathfindingNew>().m_DamageAmount++;
			enemyPrefab.GetComponent<EnemyPathfindingNew>().m_Speed += 0.001f;
		}
	}
}
