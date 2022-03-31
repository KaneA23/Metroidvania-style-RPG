using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the stats needed for player.
/// Created by: Kane Adams
/// </summary>
public class BasePlayerClass : MonoBehaviour
{
	[Header("Leveling System")]
	public int maxLvl;
	public int currentLvl;

	[Space(2)]
	public int maxXP;		// max XP for current level (dependent on level)
	public int currentXP;

	[Space(2)]
	public int skillPoints;

	// Health and regen speed
	#region Constitution Stats

	[Header("Constitution Stats")]
	public int constitutionLvl;
	public int maxConstitutionLvl;  

	[Space(5)]
	public int maxHP;				// maximum overall health
	public int minHP;				// the lowest the healthbar size can go
	public int currentHP = 20;
	public int currentMaxHP = 20;	// maximum health player can reach at current levels

	[Space(2)]
	public float regenRateHP;
	public float maxRegenHP = 10;	// Max health to naturally regen

	#endregion

	// Attack damage and jump heights
	#region Strength Stats

	[Header("Strength Stats")]
	public int strengthLvl;
	public int maxStrenthLvl;  

	#region attacks
	[Space(5)]
	public float lightAtkDamage = 35f;
	public float lightAtkMultiplier;
	public float lightKnockbackDist = 200;

	[Space(5)]
	public bool hasHeavyAtk;
	public float heavyAtkDamage = 60f;
	public float heavyAtkMultiplier;
	public float heavyKnockbackDist = 300;

	[Space(5)]
	// Post COMX
	public float rangeAtkDamage;	//post COMX
	public float rangeAtkMultiplier;	//post COMX
	public float rangeKnockbackDist;	//post COMX
	#endregion

	#region jump power
	[Space(5)]
	public float jumpForce = 25f;

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
	public int maxStam;					// max overall stamina
	public int minStam;					// the lowest the stamina bar can go
	public float currentStam;
	public int currentMaxStam = 75;	// maximum stamina player can reach at current levels

	[Space(5)]
	public float walkSpeed = 10f;
	public float crouchSpeed = 5f;
	public float speedMultiplier;

	[Space(2)]
	public float runSpeed = 15f;
	public float runSpeedMultiplier;
	public float runCost;

	[Space(5)]
	// Speeds determined by animation?
	public float lightAtkSpeed = 0.35f;
	public float heavyAtkSpeed = 0.517f;
	public float rangeAtkSpeed;	//post COMX

	[Space(5)]
	public float lightAtkCooldown = 0.5f;
	public float heavyAtkCooldown = 1f;
	public float rangeAtkCooldown;	//post COMX

	[Space(5)]
	public float knockbackTaken = 500f;

	#endregion

	// Magic, dash
	#region Wisdom Stats

	[Header("Wisdom Stats")]
	public int wisdomLvl;
	public int maxWisdomLvl;

	[Space(5)]
	public int maxMP;				// maximum overal mana
	public int minMP;				// the lowest the Manabar can go
	public float currentMP;
	public int currentMaxMP = 50;	// maximum mana player can reach at current levels
	public float regenRateMP;   

	[Space(5)]
	public bool hasDash;
	public float dashDist = 50f;
	public float dashCooldown = 1f;
	public int dashCost = 7;

	[Space(5)]
	public float lightAtkRange = 1.5f;
	public float heavyAtkRange = 2f;
	public float rangeAtkRange;	//post COMX

	[Space(5)]
	public float uiViewDist = 5f;	// distance where an enemy's UI is visible

	#endregion

	// Shop cost, rableon talk speed
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
}
