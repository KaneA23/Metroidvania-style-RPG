using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyScript : MonoBehaviour
{
    public int maxHP = 100;
    public float currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float a_damage)
	{
        currentHP -= a_damage;
        Debug.Log(gameObject.name + ": " + currentHP);

        if (currentHP <= 0)
		{
            Die();
		}
	}

    void Die()
	{
        Debug.Log("<color=Purple>DEATH TO THE ENEMY!</color>");
        gameObject.SetActive(false);
	}
}
