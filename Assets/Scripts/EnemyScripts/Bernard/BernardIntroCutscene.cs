using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BernardIntroCutscene : MonoBehaviour
{
	[Header("Referenced Scripts")]
	[SerializeField] private CameraFollowScript CFS;

	[SerializeField] private Canvas uiCanvas;
	[SerializeField] private GameObject intro;

	[SerializeField] private GameObject mainCam;
	[SerializeField] private GameObject cutsceneCam;

	[SerializeField] Animator bernardAnim;
	[SerializeField] Animator uiAnim;

	float animTime;

	public bool isCutscene;

	// Start is called before the first frame update
	void Start()
	{
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

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D a_other)
	{
		if (a_other.CompareTag("Player"))
		{
			isCutscene = true;

			uiCanvas.enabled = false;
			cutsceneCam.SetActive(true);
			mainCam.SetActive(false);
			//introCanvas.enabled = true;
			intro.SetActive(true);
			//Invoke(nameof(PlayCutscene), 1f);
			PlayCutscene();
		}
	}

	void PlayCutscene()
	{
		bernardAnim.Play("Bernard_Cutscene");
		uiAnim.Play("Bernard_introUI");

		animTime = 5f;
		Invoke(nameof(EndCutscene), animTime);
	}

	void EndCutscene()
	{
		intro.SetActive(false);

		mainCam.SetActive(true);
		CFS.cameraState = CameraState.CAM_BOSSBERNARD;
		cutsceneCam.SetActive(false);
		uiCanvas.enabled = true;

		//introCanvas.enabled = false;

		isCutscene = false;
	}
}
