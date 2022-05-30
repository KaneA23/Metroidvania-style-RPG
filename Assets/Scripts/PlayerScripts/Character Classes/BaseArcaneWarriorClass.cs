/// <summary>
/// Stats for Arcane Warrior character class.
/// Created by: Kane Adams
/// </summary>
[System.Serializable]
public class BaseArcaneWarriorClass : BasePlayerClass
{
	public BaseArcaneWarriorClass()
	{
		CharacterClassName = "Arcane Warrior";
		CharacterClassDescription = "Magic infused warrior";

		MaximumHealthPoints = 100;

		MaximumMana = 200;
		MaximumManaRegeneration = 0.5f;

		MaximumStamina = 100;

		LightAttackDamage = 30f;
		HeavyAttackDamage = 50f;

		WalkSpeed = 10f;
		CrouchingSpeed = 5f;
		runSpeed = 15f;

		//LightAttackSpeed = 0.35f;
		//HeavyAttackSpeed = 0.517f;

		LightCooldown = 0.3f;
		HeavyCooldown = 0.7f;
	}
}
