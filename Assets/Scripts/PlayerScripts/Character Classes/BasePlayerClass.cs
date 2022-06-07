using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains all the stats needed for player.
/// Created by: Kane Adams
/// </summary>
[System.Serializable]
public class BasePlayerClass : MonoBehaviour/*, IDataPersistence*/
{
	[Header("Class details")]
	private string characterClassName;
	private string characterClassDes;

	[Header("Leveling System")]
	public int maxLvl = 10;
	private int currentLvl;

	[Space(2)]
	public int maxXP;       // max XP for current level (dependent on level)
	public int currentXP;

	[Space(2)]
	public int skillPoints;

	// Health and regen speed
	#region Constitution Stats

	[Header("Constitution Stats")]
	public int constitutionLvl;
	public int maxConstitutionLvl;

	[Space(5)]
	public int maxHP = 1000;               // maximum overall health
	public int minHP = 100;               // the lowest the healthbar size can go
	public float currentHP;
	public int currentMaxHP = 100;    // maximum health player can reach at current levels

	[Space(2)]
	public float regenRateHP;
	public float maxRegenHP;    // Max health to naturally regen

	#endregion

	// Attack damage and jump heights
	#region Strength Stats

	[Header("Strength Stats")]
	public int strengthLvl;
	public int maxStrenthLvl;

	#region attacks
	[Space(5)]
	public bool hasLightAtk;
	public float lightAtkDamage = 35f;
	public float lightAtkMultiplier;
	public float lightKnockbackDist = 500;

	[Space(5)]
	public bool hasHeavyAtk;
	public float heavyAtkDamage = 60f;
	public float heavyAtkMultiplier;
	public float heavyKnockbackDist = 1000;

	[Space(5)]
	// Post COMX
	public float rangeAtkDamage = 25f;        //post COMX
	public float rangeAtkMultiplier;    //post COMX
	public float rangeKnockbackDist;    //post COMX
	#endregion

	#region jump power
	[Space(5)]
	public bool hasJump;
	public float jumpForce = 25f;
	public int jumpCost = 1;

	[Space(2)]
	public bool hasDoubleJump;
	public float doubleJumpHeight;

	[Space(2)]
	public bool hasWallJump;
	public float wallJumpForce = 50f;
	#endregion

	[Space(5)]
	public int inventorySize;

	#endregion

	// Speed, knockback taken, stamina
	#region Agility Stats

	[Header("Agility Stats")]
	public int agilityLvl;
	public int maxAgilityLvl;

	[Space(5)]
	public int maxStam;                 // max overall stamina
	public int minStam;                 // the lowest the stamina bar can go
	public float currentStam;
	public int currentMaxStam = 100;    // maximum stamina player can reach at current levels
	public float regenRateStam = 150;

	[Space(5)]
	public bool hasWalk;
	public float walkSpeed = 10f;

	[Space(2)]
	public bool hasCrouch;
	public float crouchSpeed = 5f;
	public float speedMultiplier;

	[Space(2)]
	public bool hasRun;
	public float runSpeed = 15f;
	public float runSpeedMultiplier;
	public float runCost = 1;

	[Space(5)]
	// Speeds determined by animation?
	public float lightAtkSpeed = 0.35f;
	public float heavyAtkSpeed = 0.517f;
	public float rangeAtkSpeed; //post COMX

	[Space(2)]
	public int lightAtkCost = 10;
	public int heavyAtkCost = 15;

	[Space(5)]
	public float lightAtkCooldown = 0.5f;
	public float heavyAtkCooldown = 1f;
	public float rangeAtkCooldown = 0.5f;  //post COMX

	[Space(5)]
	public float knockbackTaken = 500f;

	#endregion

	// Magic, dash
	#region Wisdom Stats

	[Header("Wisdom Stats")]
	public int wisdomLvl;
	public int maxWisdomLvl;

	[Space(5)]
	public int maxMP;               // maximum overall mana
	public int minMP;               // the lowest the Manabar can go
	public float currentMP;
	public int currentMaxMP = 100;  // maximum mana player can reach at current levels
	public float regenRateMP = 50;
	public float maxRegenMP;   // Third of max   

	[Space(5)]
	public bool hasDash;
	public float dashDist = 50f;
	public float dashCooldown = 1f;
	public int dashCost = 30;

	public int fireballCost = 15;

	[Space(5)]
	public float lightAtkRange = 1.5f;
	public float heavyAtkRange = 2f;
	public float rangeAtkRange; //post COMX

	[Space(5)]
	public float uiViewDist = 10f;   // distance where an enemy's UI is visible

	#endregion

	// Shop cost, rambleon talk speed
	#region Charisma Stats

	[Header("Charisma Stats")]
	public int charismaLvl;
	public int maxCharismaLvl;

	#endregion

	// Drop rate, enemy spawns
	#region Luck Stats

	[Header("Luck Stats")]
	public int luckLvl;
	public int maxLuckLvl;

	#endregion

	#region Getter Setters

	public int Level
	{
		get
		{
			return currentLvl;
		}

		set
		{
			currentLvl = value;
		}
	}

	public string CharacterClassName
	{
		get
		{
			return characterClassName;
		}

		set
		{
			characterClassName = value;
		}
	}

	public string CharacterClassDescription
	{
		get
		{
			return characterClassDes;
		}

		set
		{
			characterClassDes = value;
		}
	}

	public int MaximumHealthPoints
	{
		get
		{
			return currentMaxHP;
		}

		set
		{
			currentMaxHP = value;
		}
	}

	public int MaximumMana
	{
		get
		{
			return currentMaxMP;
		}

		set
		{
			currentMaxMP = value;
		}
	}

	public float MaximumManaRegeneration
	{
		get
		{
			return maxRegenMP;
		}

		set
		{
			maxRegenMP = value;
		}
	}

	public int MaximumStamina
	{
		get
		{
			return currentMaxStam;
		}

		set
		{
			currentMaxStam = value;
		}
	}

	public float LightAttackDamage
	{
		get
		{
			return lightAtkDamage;
		}

		set
		{
			lightAtkDamage = value;
		}
	}

	public float HeavyAttackDamage
	{
		get
		{
			return heavyAtkDamage;
		}

		set
		{
			heavyAtkDamage = value;
		}
	}

	public float WalkSpeed
	{
		get
		{
			return walkSpeed;
		}

		set
		{
			walkSpeed = value;
		}
	}

	public float RunSpeed
	{
		get
		{
			return runSpeed;
		}

		set
		{
			runSpeed = value;
		}
	}

	public float CrouchingSpeed
	{
		get
		{
			return crouchSpeed;
		}

		set
		{
			crouchSpeed = value;
		}
	}

	public float LightCooldown
	{
		get
		{
			return lightAtkCooldown;
		}

		set
		{
			lightAtkCooldown = value;
		}
	}

	public float HeavyCooldown
	{
		get
		{
			return heavyAtkCooldown;
		}

		set
		{
			heavyAtkCooldown = value;
		}
	}

	//public float LightAttackSpeed
	//{
	//	get
	//	{
	//		return lightAtkSpeed;
	//	}

	//	set
	//	{
	//		lightAtkSpeed = value;
	//	}
	//}

	//public float HeavyAttackSpeed
	//{
	//	get
	//	{
	//		return heavyAtkSpeed;
	//	}

	//	set
	//	{
	//		heavyAtkSpeed = value;
	//	}
	//}

	//public int 

	//public int ConstitutionLevel
	//{
	//	get
	//	{
	//		return constitutionLvl;
	//	}

	//	set
	//	{
	//		constitutionLvl = value;
	//	}
	//}

	//public int StrengthLevel
	//{
	//	get
	//	{
	//		return strengthLvl;
	//	}

	//	set
	//	{
	//		strengthLvl = value;
	//	}
	//}

	//public int AgilityLevel
	//{
	//	get
	//	{
	//		return agilityLvl;
	//	}

	//	set
	//	{
	//		agilityLvl = value;
	//	}
	//}

	//public int WisdomLevel
	//{
	//	get
	//	{
	//		return wisdomLvl;
	//	}

	//	set
	//	{
	//		wisdomLvl = value;
	//	}
	//}

	#endregion

	private void Awake()
	{
		// Comment out if want to test mechanics without class (uses default values)

		//characterClassName = GameInformation.PlayerClass.ToString();

		characterClassName = GameInformation.ClassName;
		Level = GameInformation.PlayerLvl;
		MaximumHealthPoints = GameInformation.PlayerMaxHP;
		MaximumMana = GameInformation.PlayerMaxMP;
		MaximumManaRegeneration = GameInformation.PlayerMPRegen;
		MaximumStamina = GameInformation.PlayerMaxStam;
		LightAttackDamage = GameInformation.PlayerLightDmg;
		HeavyAttackDamage = GameInformation.PlayerHvyDmg;
		WalkSpeed = GameInformation.PlayerWalkSpeed;
		CrouchingSpeed = GameInformation.PlayerCrouchSpeed;
		RunSpeed = GameInformation.PlayerRunSpeed;
		LightCooldown = GameInformation.PlayerLightCooldown;
		HeavyCooldown = GameInformation.PlayerHvyCooldown;


		//Debug.Log("Player Class: " + characterClassName);
		//Debug.Log("Player Level: " + Level);
		//Debug.Log("Player HP: " + MaximumHealthPoints);
		//Debug.Log("Player MP: " + MaximumMana);
		//Debug.Log("Player MP Regen: " + MaximumManaRegeneration);
		//Debug.Log("Player Stam: " + MaximumStamina);
		//Debug.Log("Player LightDmg: " + LightAttackDamage);
		//Debug.Log("Player HvyDmg: " + HeavyAttackDamage);
		//Debug.Log("Player walk: " + WalkSpeed);
		//Debug.Log("Player crouch: " + CrouchingSpeed);
		//Debug.Log("Player run: " + RunSpeed);
		//Debug.Log("Player lightCool: " + LightCooldown);
		//Debug.Log("Player hvycool: " + HeavyCooldown);
	}


	private void Update()
	{
		// Comment out if want to test mechanics without dialogue

		if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			hasWalk = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("hasWalk")).value;
			hasRun = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("hasRun")).value;
			hasDash = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("hasDash")).value;
			hasJump = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("hasJump")).value;
			hasDoubleJump = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("hasDoubleJump")).value;
			hasLightAtk = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("hasLightAtk")).value;
			hasHeavyAtk = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("hasHeavyAtk")).value;
		}
		else
		{
			hasWalk = true;
			hasRun = true;
			hasDash = true;
			hasJump = true;
			hasDoubleJump = true;
			hasLightAtk = true;
			hasHeavyAtk = true;
		}

		// Debugs
		//if (Input.GetKeyDown(KeyCode.Y))
		//{
		//	Debug.Log("Level: " + Level);
		//}
		//if (Input.GetKeyDown(KeyCode.H))
		//{
		//	Level++;
		//}
	}

	/// <summary>
	/// Updates stats so player levels up.
	/// Future plan is to use skill points to level up. Will need to create skill tree UI
	/// </summary>
	public void UpdateStats()
	{
		// Constitution
		MaximumHealthPoints += 10;
		if (MaximumHealthPoints > maxHP)
		{
			MaximumHealthPoints = maxHP;
		}
		currentHP = MaximumHealthPoints;
		maxRegenHP += 10;


		// Strength
		LightAttackDamage++;
		lightKnockbackDist += 10;

		HeavyAttackDamage++;
		heavyKnockbackDist += 10;

		rangeAtkDamage++;

		jumpForce++;
		if (jumpForce > 50)
		{
			jumpForce = 50;
		}


		// Agility
		MaximumStamina += 10;
		if (MaximumStamina > maxHP)
		{
			MaximumStamina = maxHP;
		}
		currentStam = MaximumStamina;

		LightCooldown -= 0.01f;
		HeavyCooldown -= 0.01f;
		rangeAtkCooldown -= 0.01f;


		// Wisdom
		MaximumMana += 10;
		if (MaximumMana > maxHP)
		{
			MaximumMana = maxHP;
		}
		currentMP = MaximumMana;
		MaximumManaRegeneration += 10;

		dashDist++;
		if (dashDist > 100)
		{
			dashDist = 100;
		}
		dashCooldown -= 0.01f;

		lightAtkRange += 0.01f;
		heavyAtkRange += 0.01f;

		uiViewDist++;
	}
}
