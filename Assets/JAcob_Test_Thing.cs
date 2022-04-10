using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAcob_Test_Thing : MonoBehaviour
{
    private Vector2 teleportLocation;

    public Transform playerCheck;
    public LayerMask playerMask;
    private readonly float checkRadius = 3;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        teleportLocation = new Vector2(-18.5f, 13f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(playerCheck.position, checkRadius, playerMask))
		{
            player.transform.position = teleportLocation;
		}
    }
}
