using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Main_Menu_Script : MonoBehaviour
{
	[Header("Desiered load scene")]
	public int last_Save = 1;

	[Header("Toggles")]
	public Toggle resume;
	public Toggle new_Game;
	public Toggle load_Game;
	public Toggle settings;
	public Toggle exit;
	[Header("Sub-Menus")]

	public GameObject resume_Canvas;
	public GameObject new_Game_Canvas;
	public GameObject load_Game_Canvas;
	public GameObject settings_Canvas;
	public GameObject exit_Canvas;

	public Animator transition;

	public float transitionTime = 1f;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	public void Play_Game()
	{
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + last_Save);
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + last_Save));
	}

	public void Play_Arena()
	{
		StartCoroutine(LoadLevel(4));	// 4 is arena
	}

	public void Exit_Game()
	{
		Application.Quit();
	}

	public void Resume()
	{
		if (resume.isOn == true)
		{
			resume_Canvas.SetActive(true);
			new_Game_Canvas.SetActive(false);
			load_Game_Canvas.SetActive(false);
			settings_Canvas.SetActive(false);
			exit_Canvas.SetActive(false);
		}
	}

	public void New_Game()
	{
		if (new_Game.isOn == true)
		{
			resume_Canvas.SetActive(false);
			new_Game_Canvas.SetActive(true);
			load_Game_Canvas.SetActive(false);
			settings_Canvas.SetActive(false);
			exit_Canvas.SetActive(false);
		}
	}
	public void Load_Game()
	{
		if (load_Game.isOn == true)
		{
			resume_Canvas.SetActive(false);
			new_Game_Canvas.SetActive(false);
			load_Game_Canvas.SetActive(true);
			settings_Canvas.SetActive(false);
			exit_Canvas.SetActive(false);
		}
	}
	public void Settings()
	{
		if (settings.isOn == true)
		{
			resume_Canvas.SetActive(false);
			new_Game_Canvas.SetActive(false);
			load_Game_Canvas.SetActive(false);
			settings_Canvas.SetActive(true);
			exit_Canvas.SetActive(false);
		}
	}
	public void Exit()
	{
		if (exit.isOn == true)
		{
			resume_Canvas.SetActive(false);
			new_Game_Canvas.SetActive(false);
			load_Game_Canvas.SetActive(false);
			settings_Canvas.SetActive(false);
			exit_Canvas.SetActive(true);
		}
	}

	public void Close_All()
	{

		resume_Canvas.SetActive(false);
		new_Game_Canvas.SetActive(false);
		load_Game_Canvas.SetActive(false);
		settings_Canvas.SetActive(false);
		exit_Canvas.SetActive(false);

	}

	IEnumerator LoadLevel(int a_levelIndex)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(a_levelIndex);
	}

}
