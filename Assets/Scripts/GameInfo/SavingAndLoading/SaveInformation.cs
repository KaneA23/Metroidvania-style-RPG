using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves character in Player Prefs.
/// Created by: Kane Adams
/// </summary>
public class SaveInformation
{
	public static void SaveAllInformation()
	{
		PlayerPrefs.SetInt("PLAYERLEVEL", GameInformation.PlayerLvl);
		PlayerPrefs.SetInt("PLAYERMAXHP", GameInformation.PlayerMaxHP);
		PlayerPrefs.SetInt("PLAYERMAXMP", GameInformation.PlayerMaxMP);
		PlayerPrefs.SetFloat("PLAYERMPREGEN", GameInformation.PlayerMPRegen);
		PlayerPrefs.SetInt("PLAYERSTAMINA", GameInformation.PlayerMaxStam);
		PlayerPrefs.SetFloat("PLAYERLIGHTDAMAGE", GameInformation.PlayerLightDmg);
		PlayerPrefs.SetFloat("PLAYERHEAVYDAMAGE", GameInformation.PlayerHvyDmg);
		PlayerPrefs.SetFloat("PLAYERWALKSPEED", GameInformation.PlayerWalkSpeed);
		PlayerPrefs.SetFloat("PLAYERCROUCHSPEED", GameInformation.PlayerCrouchSpeed);
		PlayerPrefs.SetFloat("PLAYERRUNSPEED", GameInformation.PlayerRunSpeed);
		PlayerPrefs.SetFloat("PLAYERLIGHTCOOLDOWN", GameInformation.PlayerLightCooldown);
		PlayerPrefs.SetFloat("PLAYERHEAVYCOOLDOWN", GameInformation.PlayerHvyCooldown);

		Debug.Log("Saved all information");
	}
}
