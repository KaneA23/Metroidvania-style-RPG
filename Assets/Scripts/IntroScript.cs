using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
	public float introTime = 7f;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(WaitForIntro());
	}

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator WaitForIntro()
	{
		yield return new WaitForSeconds(introTime);

		SceneManager.LoadScene("Main Menu Scene");
	}
}
