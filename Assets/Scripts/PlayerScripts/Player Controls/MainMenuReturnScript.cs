using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls player returning to main menu.
/// Created by: Kane Adams
/// </summary>
public class MainMenuReturnScript : MonoBehaviour
{
	public Animator transition;

	public float transitionTime = 1f;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			StartCoroutine(LoadLevel("Main Menu Scene"));
		}
	}

	/// <summary>
	/// Starts fade out transition
	/// </summary>
	/// <param name="a_sceneName">New scene to be entered</param>
	/// <returns>Waits 1 second before changing scene</returns>
	IEnumerator LoadLevel(string a_sceneName)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(a_sceneName);
	}
}
