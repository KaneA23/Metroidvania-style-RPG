using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInformation : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

    public static int PlayerLvl { get; set; }
    public static BasePlayerClass PlayerClass { get; set; }
    public static int PlayerMaxHP { get; set; }
    public static int PlayerMaxMP { get; set; }
    public static float PlayerMPRegen { get; set; }
    public static int PlayerMaxStam { get; set; }
    public static float PlayerLightDmg { get; set; }
    public static float PlayerHvyDmg { get; set; }
    public static float PlayerWalkSpeed { get; set; }
    public static float PlayerCrouchSpeed { get; set; }
    public static float PlayerRunSpeed { get; set; }
    public static float PlayerLightCooldown { get; set; }
    public static float PlayerHvyCooldown { get; set; }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
