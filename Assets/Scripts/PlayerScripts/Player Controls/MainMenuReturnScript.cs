using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuReturnScript : MonoBehaviour
{
	public Animator transition;

	public float transitionTime = 1f;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			StartCoroutine(LoadLevel("Main Menu Scene"));
			//SceneManager.LoadScene("Main Menu Scene");
		}
	}

	IEnumerator LoadLevel(string a_sceneName)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(a_sceneName);
	}
}
