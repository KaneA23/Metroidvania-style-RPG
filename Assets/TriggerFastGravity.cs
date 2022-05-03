using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFastGravity : MonoBehaviour
{
    [Header("Referenced Scripts")]
    [SerializeField] private CameraFollowScript CFS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D a_other)
	{
		if (a_other.CompareTag("Player"))
		{
			a_other.GetComponent<Rigidbody2D>().gravityScale = 5f;
            CFS.cameraState = CameraState.CAM_BOSSBERNARD;
        }
	}
}
