using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the stats needed for player.
/// Created by: Kane Adams
/// </summary>
public class BasePlayerClass
{
	public int maxLvl;
	public int currentLvl;

	public int maxXP;       // max XP for current level (dependent on level)
	public int currentXP;

	// Health and regen speed
	#region Constitution Stats

	public int constitutionLvl;
	public int maxConstitutionLvl;  //check if needed

	public int maxHP;				// maximum overall health
	public int minHP;				// the lowest the healthbar can go
	public float currentHP;
	public int currentMaxHP;		// maximum health player can reach at current levels

	public float regenRateHP;
	public float maxRegenRateHP;

	#endregion

	// Attack damage and jump heights
	#region Strength Stats

	public int strengthLvl;
	public int maxStrenthLvl;  //check if needed

	#region attacks
	public float lightDamage;
	public float lightMultiplier;
	public float lightKnockbackDist;

	public bool isHeavyAttackActive;
	public float heavyDamage;
	public float heavyMultiplier;
	public float heavyKnockbackDist;

	public float rangeDamage;
	public float rangeMultiplier;
	public float rangeKnockbackDist;
	#endregion

	#region jump power
	public float jumpHeight;

	public bool isDoubleJumpActive;
	public float doubleJumpHeight;

	public bool isWallJumpActive;
	public float wallJumpHeight;
	public float wallJumpWidth;   //rename? width don't sound right...
	#endregion

	public int inventorySize;   //check if fits here

	#endregion

	// Speed, knockback taken, stamina
	#region Agility Stats

	public int agilityLvl;
	public int maxAgilityLvl;

	public int maxStam;         // max overall stamina
	public int minStam;         // the lowest the stamina bar can go
	public float currentStam;
	public int currentMaxStam;  // maximum stamina player can reach at current levels

	public float walkSpeed;
	public float crouchSpeed;

	public float runSpeed;
	public float runSpeedMultiplier;
	public float runCost;

	public float lightAttackSpeed;
	public float heavyDecaySpeed;
	public float rangeAttackSpeed;  //post COMX

	#endregion

	// Magic, dash
	#region Wisdom Stats

	public int wisdomLvl;
	public int maxWisdomLvl;

	public int maxMP;           // maximum overal mana
	public int minMP;           // the lowest the Manabar can go
	public float currentMP;
	public float regenRateMP;   // maximum mana player can reach at current levels

	public bool isDashActive;
	public float dashDist;
	public float dashCooldown;
	public float dashCost;

	public float uiViewDist;    // distance where an enemy's UI is visible

	#endregion

	// Shop cost, rableon talk speed
	#region Charisma Stats

	public int charismaLvl;
	public int maxCharismaLvl;

	#endregion

	// Drop rate, enemy spawns
	#region Luck Stats

	public int luckLvl;
	public int maxLuckLvl;

	#endregion
}
