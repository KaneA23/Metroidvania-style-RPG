using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePaladinClass : BasePlayerClass
{
    public BasePaladinClass()
	{
		CharacterClassName = "Paladin";
		CharacterClassDescription = "A knight that blindly follows the beliefs of the Goddess.";

		MaximumHealthPoints = 300;

		MaximumMana = 50;
		MaximumManaRegeneration = 1f;

		MaximumStamina = 200;

		LightAttackDamage = 50f;
		HeavyAttackDamage = 100f;

		WalkSpeed = 7.5f;
		CrouchingSpeed = 2.5f;
		runSpeed = 10f;

		LightCooldown = 0.5f;
		HeavyCooldown = 1f;
	}
}
