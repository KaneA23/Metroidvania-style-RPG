using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArcaneWarriorClass : BasePlayerClass
{
    public BaseArcaneWarriorClass()
	{
		CharacterClassName = "Arcane Warrior";
		CharacterClassDescription = "Magic infused warrior";

		MaximumHealthPoints = 100;

		MaximumMana = 200;
		MaximumManaRegeneration = 0.3f;

		MaximumStamina = 100;

		LightAttackDamage = 35f;
		HeavyAttackDamage = 60f;

		WalkSpeed = 10f;
		CrouchingSpeed = 5f;
		runSpeed = 15f;

		//LightAttackSpeed = 0.35f;
		//HeavyAttackSpeed = 0.517f;

		LightCooldown = 0.3f;
		HeavyCooldown = 0.7f;
	}
}
