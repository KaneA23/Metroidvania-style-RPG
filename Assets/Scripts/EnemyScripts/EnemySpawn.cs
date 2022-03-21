using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject m_GroundEnemy;
    public GameObject m_FlyingEnemy;

    public int m_GroundEnemyNumber;
    public int m_FlyingEnemyNumber;

    public Vector2 spawnPos;

    public void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        float x;
        float y = -3.48f; //-2.68f

		for (int i = 0; i < m_GroundEnemyNumber; i++)
		{
			x = Random.Range(-5.5f, -1.4f);
			spawnPos = new Vector2(x, y);

			Instantiate(m_GroundEnemy, spawnPos, Quaternion.identity);
		}

		for (int i = 0; i < m_FlyingEnemyNumber; i++)
        {
            x = Random.Range(-8.51f, 8.51f);
            y = Random.Range(2.49f, 4.5f);
            spawnPos = new Vector2(x, y);

            Instantiate(m_FlyingEnemy, spawnPos, Quaternion.identity);
        }
    }
}
