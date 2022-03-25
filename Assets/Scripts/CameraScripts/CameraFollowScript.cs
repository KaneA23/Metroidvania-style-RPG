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

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void LateUpdate()
	{
        // Store camera's current position
		Vector3 temp = transform.position;
        temp.x = playerTransform.position.x;
        temp.y = playerTransform.position.y;
        temp.y += yOffset;
        transform.position = temp;
	}
}
