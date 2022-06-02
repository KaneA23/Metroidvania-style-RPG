using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Plays animated splashscreen when game begins.
/// Created by: Kane Adams
/// </summary>
public class IntroScript : MonoBehaviour
{
	public float introTime = 7f;

	// Start is called before the first frame update
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		StartCoroutine(WaitForIntro());
	}

	/// <summary>
	/// Loads MainMenu after animation
	/// </summary>
	/// <returns>Waits for animation to end</returns>
	IEnumerator WaitForIntro()
	{
		yield return new WaitForSeconds(introTime);

		SceneManager.LoadScene("Main Menu Scene");
	}
}
