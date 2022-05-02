using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads a previous character class from Player Prefs.
/// Created by: Kane Adams
/// </summary>
public class LoadInformation
{
	public static void LoadAllInformation()
	{
		GameInformation.PlayerLvl = PlayerPrefs.GetInt("PLAYERLEVEL");
		GameInformation.PlayerMaxHP = PlayerPrefs.GetInt("PLAYERMAXHP");
		GameInformation.PlayerMaxMP = PlayerPrefs.GetInt("PLAYERMAXMP");
		GameInformation.PlayerMPRegen = PlayerPrefs.GetInt("PLAYERMPREGEN");
		GameInformation.PlayerMaxStam = PlayerPrefs.GetInt("PLAYERSTAMINA");
		GameInformation.PlayerLightDmg = PlayerPrefs.GetInt("PLAYERLIGHTDAMAGE");
		GameInformation.PlayerHvyDmg = PlayerPrefs.GetInt("PLAYERHEAVYDAMAGE");
		GameInformation.PlayerWalkSpeed = PlayerPrefs.GetInt("PLAYERWALKSPEED");
		GameInformation.PlayerCrouchSpeed = PlayerPrefs.GetInt("PLAYERCROUCHSPEED");
		GameInformation.PlayerRunSpeed = PlayerPrefs.GetInt("PLAYERRUNSPEED");
		GameInformation.PlayerLightCooldown = PlayerPrefs.GetInt("PLAYERLIGHTCOOLDOWN");
		GameInformation.PlayerHvyCooldown = PlayerPrefs.GetInt("PLAYERHEAVYCOOLDOWN");

	}
}
