using UnityEngine;

/// <summary>
/// Controls when new scene Boss intro is played.
/// Created by: Kane Adams
/// </summary>
public class BernardIntroCutscene : MonoBehaviour
{
	[Header("Referenced Scripts")]
	[SerializeField] private CameraFollowScript CFS;

	[SerializeField] private Canvas uiCanvas;
	[SerializeField] private GameObject intro;

	[SerializeField] private GameObject mainCam;
	[SerializeField] private GameObject cutsceneCam;

	[SerializeField] private GameObject bernardUI;

	[SerializeField] Animator bernardAnim;
	[SerializeField] Animator uiAnim;
	[SerializeField] Animator bgAnim;

	float animTime;

	public bool isCutscene;

	// Start is called before the first frame update
	void Start()
	{
		bernardUI.SetActive(false);
		gameObject.GetComponent<Collider2D>().enabled = true;
		isCutscene = false;

		CFS.cameraState = CameraState.CAM_FOLLOWING;
		//introCanvas.enabled = false;
		intro.SetActive(false);
		uiCanvas.enabled = true;

		mainCam.SetActive(true);
		cutsceneCam.SetActive(false);

		bernardAnim.StopPlayback();
		uiAnim.StopPlayback();
	}

	/// <summary>
	/// If player enters arena trigger, intro plays
	/// </summary>
	/// <param name="a_other">Checks if player entered trigger</param>
	private void OnTriggerEnter2D(Collider2D a_other)
	{
		if (a_other.CompareTag("Player"))
		{
			a_other.GetComponent<Rigidbody2D>().gravityScale = 1f;

			//introCanvas.enabled = true;
			isCutscene = true;
			//Invoke(nameof(PlayCutscene), 1f);
			PlayCutscene();
		}
	}

	/// <summary>
	/// Hides UI and plays Intro cutscene
	/// </summary>
	void PlayCutscene()
	{
		FindObjectOfType<AudioManager>().StopAudio("SkyTheme");
		FindObjectOfType<AudioManager>().PlayAudio("BernardIntro");
		uiCanvas.enabled = false;
		cutsceneCam.SetActive(true);
		mainCam.SetActive(false);

		intro.SetActive(true);
		bernardAnim.Play("Bernard_Cutscene");
		uiAnim.Play("Bernard_introUI");
		bgAnim.Play("Cutscene_BG");

		animTime = 4f;
		Invoke(nameof(EndCutscene), animTime);
	}

	/// <summary>
	/// Turns back on UI for player control
	/// </summary>
	void EndCutscene()
	{
		intro.SetActive(false);

		mainCam.SetActive(true);
		//CFS.cameraState = CameraState.CAM_BOSSBERNARD;
		cutsceneCam.SetActive(false);
		uiCanvas.enabled = true;

		//introCanvas.enabled = false;

		FindObjectOfType<AudioManager>().PlayAudio("SkyTheme");
		isCutscene = false;

		bernardUI.SetActive(true);

		gameObject.GetComponent<Collider2D>().enabled = false;
	}
}
