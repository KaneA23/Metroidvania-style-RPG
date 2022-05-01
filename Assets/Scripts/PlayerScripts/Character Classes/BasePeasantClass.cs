using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePeasantClass : BasePlayerClass
{
	public BasePeasantClass()
	{
		CharacterClassName = "Peasant";
		CharacterClassDescription = "Just a normal bloke... not sure what you expected!";

		MaximumHealthPoints = 50;

		MaximumMana = 50;
		MaximumManaRegeneration = 1f;

		MaximumStamina = 50;

		LightAttackDamage = 10f;
		HeavyAttackDamage = 15f;

		WalkSpeed = 7.5f;
		CrouchingSpeed = 2.5f;
		runSpeed = 10f;

		//LightAttackSpeed = 0.4f;
		//HeavyAttackSpeed = 0.6f;
		LightCooldown = 1f;
		HeavyCooldown = 2f;
	}
}
