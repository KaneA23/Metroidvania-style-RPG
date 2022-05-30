using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves Player class selection between scenes.
/// Created by: Kane Adams
/// </summary>
[System.Serializable]
public class GameInformation : MonoBehaviour/*, IDataPersistence*/
{
	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	public static int PlayerLvl { get; set; }
	public static BasePlayerClass PlayerClass { get; set; }
	public static string PlayerName { get; set; }
	public static string ClassName { get; set; }
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

	public static float checkpointPosX { get; set; }
	public static float checkpointPosY { get; set; }


	public GameInformation()
	{

	}

	//public void LoadData(GameData gameData)
	//{
	//	//throw new System.NotImplementedException();
	//}
	//
	//public void SaveData(ref GameData gameData)
	//{
	//	//throw new System.NotImplementedException();
	//}
}
