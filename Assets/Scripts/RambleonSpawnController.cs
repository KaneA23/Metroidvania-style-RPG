using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RambleonSpawnController : MonoBehaviour
{
	public GameObject[] rambleons;

	public int rambleonCounter;

	// Start is called before the first frame update
	void Start()
	{
		for (int i = 0; i < rambleons.Length; i++)
		{
			rambleons[i].SetActive(false);
		}

		rambleonCounter = 0;
	}

	// Update is called once per frame
	void Update()
	{
		switch (rambleonCounter)
		{
			case 0:
				rambleons[0].SetActive(true);
				break;

			case 1:
				rambleons[1].SetActive(true);
				break;

			case 2:
				rambleons[2].SetActive(true);
				break;

			case 3:
				rambleons[3].SetActive(true);
				break;

			case 4:
				rambleons[4].SetActive(true);
				break;

			default:
				break;
		}
	}
}
