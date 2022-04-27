using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Start is called before the first frame update
	void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (isSeen)
		{
			isSeen = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - yOffset), threshold, 0, playerLayer);
		}

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
		if (!isSeen)
		{
			//float fillF = Mathf.Round(healthFrontFillBar.fillAmount * 100) * 0.01f;
			//float fillB = Mathf.Round(healthBackHealthBar.fillAmount * 100) * 0.01f;
			//float playerTempX = Mathf.Round(playerTransform.position.x * 10) * 0.1f;
			//float playerTempY = Mathf.Round(playerTransform.position.y * 10) * 0.1f;
			//float camTempX = Mathf.Round(transform.position.x * 10) * 0.1f;
			//float camTempY = Mathf.Round((transform.position.y - yOffset) * 10) * 0.1f;

			float playerTempX = Mathf.Round(playerTransform.position.x);
			float playerTempY = Mathf.Round(playerTransform.position.y);
			float camTempX = Mathf.Round(transform.position.x);
			float camTempY = Mathf.Round((transform.position.y - yOffset));
			Debug.Log("player: " + playerTempX + ", " + playerTempY);
			Debug.Log("Camera: " + camTempX + ", " + camTempY);

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
}
