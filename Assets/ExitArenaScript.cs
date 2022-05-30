using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitArenaScript : MonoBehaviour
{
	[Header("Referenced Scripts")]
	private CameraFollowScript CFS;

	[Header("Unrequired UI")]
	[SerializeField] private GameObject bernardUI;

	private void Awake()
	{
		CFS = FindObjectOfType<CameraFollowScript>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter2D(Collider2D a_collision)
	{
		if (a_collision.CompareTag("Player"))
		{
			bernardUI.SetActive(false);

			CFS.cameraState = CameraState.CAM_FOLLOWING;
		}
	}
}
