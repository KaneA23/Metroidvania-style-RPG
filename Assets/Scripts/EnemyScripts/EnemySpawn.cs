using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject m_Enemy;

    public int m_EnemyNumber;

    public Vector2 spawnPos;

    public void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        float x;
        float y = -3.48f; //-2.68f

        for (int i = 0; i < m_EnemyNumber; i++)
        {
            x = Random.Range(-5.5f, -1.4f);
            spawnPos = new Vector2(x, y);

            Instantiate(m_Enemy, spawnPos, Quaternion.identity);
        }
    }
}
