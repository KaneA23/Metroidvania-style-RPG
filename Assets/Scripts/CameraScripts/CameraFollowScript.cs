using UnityEngine;

/// <summary>
/// Different camera states that change what camera focuses on.
/// </summary>
public enum CameraState
{
	CAM_FOLLOWING,  // default camera
	CAM_BOSSBERNARD,
	CAM_ARENA,
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

	float followCamSize = 7.5f;

	public CameraState cameraState;

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

		// Debugs
		//if (Input.GetKeyDown(KeyCode.Alpha1))
		//{
		//	cameraState = CameraState.CAM_FOLLOWING;
		//}
		//if (Input.GetKeyDown(KeyCode.Alpha2))
		//{
		//	cameraState = CameraState.CAM_BOSSBERNARD;
		//}
		//if (Input.GetKeyDown(KeyCode.Alpha3))
		//{
		//	cameraState = CameraState.CAM_ARENA;
		//}

		CheckCameraState();
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
		Vector3 smoothedPos;

		switch (cameraState)
		{
			case CameraState.CAM_FOLLOWING:
				isFollowingPlayer = true;
				smoothZoom = Mathf.Lerp(cam.orthographicSize, followCamSize, 1f * Time.fixedDeltaTime);
				cam.orthographicSize = smoothZoom;

				break;

			case CameraState.CAM_BOSSBERNARD:
				isFollowingPlayer = false;

				smoothZoom = Mathf.Lerp(cam.orthographicSize, 11.3f, 2f * Time.deltaTime);
				cam.orthographicSize = smoothZoom;

				smoothedPos = Vector3.Lerp(transform.position, new Vector3(1.6f, -6.4f, -10f), 1f * Time.fixedDeltaTime);
				transform.position = smoothedPos;

				break;

			case CameraState.CAM_ARENA:
				isFollowingPlayer = false;
				smoothZoom = Mathf.Lerp(cam.orthographicSize, 14.5f, 2f * Time.deltaTime);
				cam.orthographicSize = smoothZoom;

				smoothedPos = Vector3.Lerp(transform.position, new Vector3(3f, -2.5f, -10f), 1f * Time.fixedDeltaTime);
				transform.position = smoothedPos;

				break;

			default:
				isFollowingPlayer = true;
				cam.orthographicSize = followCamSize;

				break;
		}
	}
}
