using UnityEngine;
using TMPro;

/// <summary>
/// On new games, player can choose their character class.
/// Created by: Kane Adams
/// </summary>
public class CreateNewCharacter : MonoBehaviour
{
	private BasePlayer newPlayer;

	[SerializeField] private TextMeshProUGUI className;
	[SerializeField] private TextMeshProUGUI classDesc;

	public bool isPaladinClass;
	public bool isArcaneWarriorClass;
	public bool isRogueClass;
	public bool isPeasantClass;

	//private string playerName;

	// Start is called before the first frame update
	void Start()
	{
		newPlayer = new BasePlayer();
	}

	/// <summary>
	/// Selects Paladin class waiting for confirmation
	/// </summary>
	/// <param name="a_isActive">Is currently selected?</param>
	public void TogglePaladin(bool a_isActive)
	{
		isPaladinClass = a_isActive;
		//Debug.Log("Paladin: " + isPaladinClass);

		if (isPaladinClass)
		{
			newPlayer.PlayerClass = new BasePaladinClass();
			className.text = newPlayer.PlayerClass.CharacterClassName;
			classDesc.text = newPlayer.PlayerClass.CharacterClassDescription;
		}
	}

	/// <summary>
	/// Selects Arcane Warrior class waiting for confirmation
	/// </summary>
	/// <param name="a_isActive">Is currently selected?</param>
	public void ToggleArcaneWarrior(bool a_isActive)
	{
		isArcaneWarriorClass = a_isActive;
		//Debug.Log("Arcane Warrior: " + isArcaneWarriorClass);

		if (isArcaneWarriorClass)
		{
			newPlayer.PlayerClass = new BaseArcaneWarriorClass();
			className.text = newPlayer.PlayerClass.CharacterClassName;
			classDesc.text = newPlayer.PlayerClass.CharacterClassDescription;
		}
	}

	/// <summary>
	/// Selects Rogue class waiting for confirmation
	/// </summary>
	/// <param name="a_isActive">Is currently selected?</param>
	public void ToggleRogue(bool a_isActive)
	{
		isRogueClass = a_isActive;
		//Debug.Log("Rogue: " + isRogueClass);

		if (isRogueClass)
		{
			newPlayer.PlayerClass = new BaseRogueClass();
			className.text = newPlayer.PlayerClass.CharacterClassName;
			classDesc.text = newPlayer.PlayerClass.CharacterClassDescription;
		}
	}

	/// <summary>
	/// Selects Peasant class waiting for confirmation
	/// </summary>
	/// <param name="a_isActive">Is currently selected?</param>
	public void TogglePeasant(bool a_isActive)
	{
		isPeasantClass = a_isActive;
		//Debug.Log("Peasant:" + isPeasantClass);

		if (isPeasantClass)
		{
			newPlayer.PlayerClass = new BasePeasantClass();
			className.text = newPlayer.PlayerClass.CharacterClassName;
			classDesc.text = newPlayer.PlayerClass.CharacterClassDescription;
		}
	}

	/// <summary>
	/// Chooses current class and alters Base Player Stats to be the selected class'
	/// </summary>
	public void ConfirmCharacterChoice()
	{
		newPlayer.PlayerLvl = 1;

		newPlayer.MaxHP = newPlayer.PlayerClass.MaximumHealthPoints;
		newPlayer.MaxMP = newPlayer.PlayerClass.MaximumMana;
		newPlayer.MaxMPRegen = newPlayer.PlayerClass.MaximumManaRegeneration;
		newPlayer.MaxStam = newPlayer.PlayerClass.MaximumStamina;
		newPlayer.LightDamage = newPlayer.PlayerClass.LightAttackDamage;
		newPlayer.HeavyDamage = newPlayer.PlayerClass.HeavyAttackDamage;
		newPlayer.WalkSpeed = newPlayer.PlayerClass.WalkSpeed;
		newPlayer.CrouchSpeed = newPlayer.PlayerClass.CrouchingSpeed;
		newPlayer.RunSpeed = newPlayer.PlayerClass.RunSpeed;
		newPlayer.LightAttackCooldown = newPlayer.PlayerClass.LightCooldown;
		newPlayer.HeavyAttackCooldown = newPlayer.PlayerClass.HeavyCooldown;

		//Debug.Log("Player Class: " + newPlayer.PlayerClass.CharacterClassName);
		//Debug.Log("Player Level: " + newPlayer.PlayerLvl);
		//Debug.Log("Player HP: " + newPlayer.PlayerClass.MaximumHealthPoints);
		//Debug.Log("Player MP: " + newPlayer.PlayerClass.MaximumMana);
		//Debug.Log("Player MP Regen: " + newPlayer.PlayerClass.MaximumManaRegeneration);
		//Debug.Log("Player Stam: " + newPlayer.PlayerClass.MaximumStamina);
		//Debug.Log("Player LightDmg: " + newPlayer.PlayerClass.LightAttackDamage);
		//Debug.Log("Player HvyDmg: " + newPlayer.PlayerClass.HeavyAttackDamage);
		//Debug.Log("Player walk: " + newPlayer.PlayerClass.WalkSpeed);
		//Debug.Log("Player crouch: " + newPlayer.PlayerClass.CrouchingSpeed);
		//Debug.Log("Player run: " + newPlayer.PlayerClass.RunSpeed);
		//Debug.Log("Player lightCool: " + newPlayer.PlayerClass.LightCooldown);
		//Debug.Log("Player hvycool: " + newPlayer.PlayerClass.HeavyCooldown);

		StoreNewPlayerInfo();

		//SaveInformation.SaveAllInformation();
		//GetComponent<Main_Menu_Script>().Play_Game();	// Plays next scene
	}

	/// <summary>
	/// Saves new stats to GameInformation to enter new scene as chosen class
	/// </summary>
	private void StoreNewPlayerInfo()
	{
		GameInformation.PlayerLvl = newPlayer.PlayerLvl;
		GameInformation.PlayerClass = newPlayer.PlayerClass;
		GameInformation.ClassName = newPlayer.PlayerClass.CharacterClassName;
		GameInformation.PlayerMaxHP = newPlayer.MaxHP;
		GameInformation.PlayerMaxMP = newPlayer.MaxMP;
		GameInformation.PlayerMPRegen = newPlayer.MaxMPRegen;
		GameInformation.PlayerMaxStam = newPlayer.MaxStam;
		GameInformation.PlayerLightDmg = newPlayer.LightDamage;
		GameInformation.PlayerHvyDmg = newPlayer.HeavyDamage;
		GameInformation.PlayerWalkSpeed = newPlayer.WalkSpeed;
		GameInformation.PlayerCrouchSpeed = newPlayer.CrouchSpeed;
		GameInformation.PlayerRunSpeed = newPlayer.RunSpeed;
		GameInformation.PlayerLightCooldown = newPlayer.LightAttackCooldown;
		GameInformation.PlayerHvyCooldown = newPlayer.HeavyAttackCooldown;

		//Debug.Log("Stored Class Name: " + GameInformation.ClassName);
		//Debug.Log("Stored Level: " + GameInformation.PlayerLvl);
	}
}
