using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Different camera states that change what camera focuses on.
/// </summary>
public enum CameraState
{
	CAM_FOLLOWING,  // default camera
					//CAM_BERNARDINTRO,
	CAM_BOSSBERNARD,
}

/// <summary>
/// Controls the camera position, dependent on player position.
/// Created by: Kane Adams
/// </summary>
public class CameraFollowScript : MonoBehaviour
{
	private Transform playerTransform;

	public float yOffset;
	[Range(0, 10)]
	public float smoothness;

	public LayerMask playerLayer;

	public Vector2 threshold;

	public bool isSeen;

	float followCamSize = 5f;

	public CameraState cameraState;

	//gameObject.GetComponent<Camera>().orthographicSize
	Camera cam;

	[SerializeField] private bool isFollowingPlayer;

	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	// Start is called before the first frame update
	void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		cameraState = CameraState.CAM_FOLLOWING;
		CheckCameraState();
	}

	// Update is called once per frame
	void Update()
	{
		if (isSeen && isFollowingPlayer)
		{
			isSeen = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - yOffset), threshold, 0, playerLayer);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			cameraState = CameraState.CAM_FOLLOWING;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			cameraState = CameraState.CAM_BOSSBERNARD;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			cameraState = CameraState.CAM_BOSSBERNARD;
		}
		CheckCameraState();

		//if (playerTransform.position.x != threshold.x || playerTransform.position.y != threshold.y)
		//{
		//	isSeen = false;
		//}
		//else
		//{
		//	isSeen = true;
		//}
	}

	private void LateUpdate()
	{
		if (!isSeen && isFollowingPlayer)
		{
			float playerTempX = Mathf.Round(playerTransform.position.x);
			float playerTempY = Mathf.Round(playerTransform.position.y);
			float camTempX = Mathf.Round(transform.position.x);
			float camTempY = Mathf.Round((transform.position.y - yOffset));
			//Debug.Log("player: " + playerTempX + ", " + playerTempY);
			//Debug.Log("Camera: " + camTempX + ", " + camTempY);

			if (playerTempX != camTempX || playerTempY != camTempY)
			{
				//isSeen = false;
				// Store camera's current position
				Vector3 temp = transform.position;
				temp.x = playerTransform.position.x;
				temp.y = playerTransform.position.y;
				temp.y += yOffset;

				Vector3 smoothedPos = Vector3.Lerp(transform.position, temp, smoothness * Time.fixedDeltaTime);
				transform.position = smoothedPos;
			}
			else
			{
				isSeen = true;
			}
		}
	}

	/// <summary>
	/// Allows to see attack ranges in editor
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		// Need to manually add Gizmo ranges to work, can't reference other script
		Gizmos.color = Color.grey;
		Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - yOffset), threshold);
	}

	/// <summary>
	/// Changes what camera focuses on, 
	/// by default will follow player
	/// </summary>
	void CheckCameraState()
	{
		float smoothZoom;

		switch (cameraState)
		{
			case CameraState.CAM_FOLLOWING:
				isFollowingPlayer = true;
				//cam.orthographicSize = followCamSize;
				smoothZoom = Mathf.Lerp(cam.orthographicSize, followCamSize, 1f * Time.deltaTime);
				cam.orthographicSize = smoothZoom;
				break;

			case CameraState.CAM_BOSSBERNARD:
				isFollowingPlayer = false;
				//gameObject.GetComponent<Camera>().orthographicSize = 11.3f;
				smoothZoom = Mathf.Lerp(cam.orthographicSize, 11.3f, 1f * Time.deltaTime);
				cam.orthographicSize = smoothZoom;

				//gameObject.transform.position = ;
				Vector3 smoothedPos = Vector3.Lerp(transform.position, new Vector3(8.5f, -30f, -10f), 0.2f * Time.fixedDeltaTime);
				transform.position = smoothedPos;
				//transform.position = new Vector3(8.5f, -30f, -10f);
				break;

			default:
				isFollowingPlayer = true;
				cam.orthographicSize = followCamSize;
				break;
		}
	}
}
