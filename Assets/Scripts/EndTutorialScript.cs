using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls when the final Rambleon appears after defeating the tutorial enemy.
/// Created by: Kane Adams
/// </summary>
public class EndTutorialScript : MonoBehaviour
{
    [SerializeField] private GameObject rambleon;

	// Start is called before the first frame update
	void Start()
    {
        rambleon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<EnemyHealth>().m_CurrentHP <= 0)
		{
            rambleon.SetActive(true);
		}
    }
}