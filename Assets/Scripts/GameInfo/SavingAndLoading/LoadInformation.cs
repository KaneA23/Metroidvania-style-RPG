using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Loads a previous character class from Player Prefs.
/// Created by: Kane Adams
/// </summary>
public static class LoadInformation
{
	public static GameInformation LoadAllInformation()
	{
		//GameInformation.PlayerLvl = PlayerPrefs.GetInt("PLAYERLEVEL");
		//GameInformation.PlayerMaxHP = PlayerPrefs.GetInt("PLAYERMAXHP");
		//GameInformation.PlayerMaxMP = PlayerPrefs.GetInt("PLAYERMAXMP");
		//GameInformation.PlayerMPRegen = PlayerPrefs.GetInt("PLAYERMPREGEN");
		//GameInformation.PlayerMaxStam = PlayerPrefs.GetInt("PLAYERSTAMINA");
		//GameInformation.PlayerLightDmg = PlayerPrefs.GetInt("PLAYERLIGHTDAMAGE");
		//GameInformation.PlayerHvyDmg = PlayerPrefs.GetInt("PLAYERHEAVYDAMAGE");
		//GameInformation.PlayerWalkSpeed = PlayerPrefs.GetInt("PLAYERWALKSPEED");
		//GameInformation.PlayerCrouchSpeed = PlayerPrefs.GetInt("PLAYERCROUCHSPEED");
		//GameInformation.PlayerRunSpeed = PlayerPrefs.GetInt("PLAYERRUNSPEED");
		//GameInformation.PlayerLightCooldown = PlayerPrefs.GetInt("PLAYERLIGHTCOOLDOWN");
		//GameInformation.PlayerHvyCooldown = PlayerPrefs.GetInt("PLAYERHEAVYCOOLDOWN");


		string filePath = Application.persistentDataPath + "/player.dat";

		if (File.Exists(filePath))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fStream = new FileStream(filePath, FileMode.Open);

			GameInformation gameData = formatter.Deserialize(fStream) as GameInformation;
			fStream.Close();

			return gameData;
		}
		else
		{
			Debug.LogError("Save file not found in " + filePath);
			return null;
		}

	}
}
