using UnityEngine;

/// <summary>
/// Controls how fast player falls into arena.
/// Created by: Kane Adams
/// </summary>
public class TriggerFastGravity : MonoBehaviour
{
    [Header("Referenced Scripts")]
    [SerializeField] private CameraFollowScript CFS;

	/// <summary>
	/// Speeds up player's fall amd changes camera
	/// </summary>
	/// <param name="a_other">Checks if triggered by player</param>
	private void OnTriggerEnter2D(Collider2D a_other)
	{
		if (a_other.CompareTag("Player"))
		{
			a_other.GetComponent<Rigidbody2D>().gravityScale = 5f;
            CFS.cameraState = CameraState.CAM_BOSSBERNARD;
        }
	}
}
