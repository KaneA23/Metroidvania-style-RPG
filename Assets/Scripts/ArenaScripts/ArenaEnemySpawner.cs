using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemySpawner : MonoBehaviour
{
	[Header("Referenced Scripts")]
	private ArenaWaveManager AWM;

	[SerializeField] private GameObject enemyPrefab;

	public List<GameObject> enemySpawns;

	public GameObject previousSpawn;

	//public int wave;
	int round;

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

	// Start is called before the first frame update
	void Start()
	{
		//wave = 1;

		//SpawnEnemy();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			//CheckWave(wave);
			//for (int i = 0; i < spawnNumber; i++)
			//{
			//Invoke(nameof(SpawnEnemy), 0.5f);
			//}
			//wave++;
			spawnNumber = 0;
		}
	}

	//void SpawnEnemy()
	//{
	//	Debug.Log("Wave: " + wave.ToString());

	//	for (int i = 0; i < wave; i++)
	//	{
	//		GameObject randomSpawn = enemySpawns[Random.Range(0, enemySpawns.Count)];
	//		Vector2 spawnPoint = randomSpawn.transform.position;

	//		Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
	//	}
	//	wave++;
	//}

	public void SpawnEnemy()
	{
		GameObject randomSpawn = enemySpawns[Random.Range(0, enemySpawns.Count)];


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

	public void CheckWave(int a_waveNum)
	{
		round = a_waveNum;
		if (round > 10)
		{
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
}
