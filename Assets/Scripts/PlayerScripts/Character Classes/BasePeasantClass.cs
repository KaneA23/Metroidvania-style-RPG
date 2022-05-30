/// <summary>
/// Stats for Peasant character class.
/// Created by: Kane Adams
/// </summary>
[System.Serializable]
public class BasePeasantClass : BasePlayerClass
{
	public BasePeasantClass()
	{
		CharacterClassName = "Peasant";
		CharacterClassDescription = "Just a normal bloke... not sure what you expected!";

		MaximumHealthPoints = 100;

		MaximumMana = 100;
		MaximumManaRegeneration = 0.3f;

		MaximumStamina = 100;

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
