using UnityEngine;

/// <summary>
/// Changes camera and UI to not be in fight
/// Created by: Kane Adams
/// </summary>
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

	/// <summary>
	/// If interacts with player, 
	/// camera follows player and removes boss UI
	/// </summary>
	/// <param name="a_collision">Checks if collides with player</param>
	private void OnTriggerEnter2D(Collider2D a_collision)
	{
		if (a_collision.CompareTag("Player"))
		{
			bernardUI.SetActive(false);

			//CFS.cameraState = CameraState.CAM_FOLLOWING;
		}
	}
}
