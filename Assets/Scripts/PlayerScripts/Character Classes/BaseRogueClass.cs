/// <summary>
/// Stats for Rogue character class.
/// Created by: Kane Adams
/// </summary>
[System.Serializable]
public class BaseRogueClass : BasePlayerClass
{
    public BaseRogueClass()
	{
		CharacterClassName = "Rogue";
		CharacterClassDescription = "A silent killer that lives in shadows";

		MaximumHealthPoints = 200;

		MaximumMana = 150;
		MaximumManaRegeneration = 0.3f;

		MaximumStamina = 300;

		LightAttackDamage = 20f;
		HeavyAttackDamage = 30f;

		WalkSpeed = 15f;
		CrouchingSpeed = 7.5f;
		runSpeed = 20f;

		//LightAttackSpeed = 0.3f;
		//HeavyAttackSpeed = 0.5f;
		LightCooldown = 0.2f;
		HeavyCooldown = 0.3f;
	}
}
