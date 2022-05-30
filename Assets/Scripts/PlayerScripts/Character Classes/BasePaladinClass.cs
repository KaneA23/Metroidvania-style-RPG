/// <summary>
/// Stats for Paladin character class.
/// Created by: Kane Adams
/// </summary>
[System.Serializable]
public class BasePaladinClass : BasePlayerClass
{
    public BasePaladinClass()
	{
		CharacterClassName = "Paladin";
		CharacterClassDescription = "A knight that blindly follows the beliefs of the Goddess.";

		MaximumHealthPoints = 300;

		MaximumMana = 100;
		MaximumManaRegeneration = 0.4f;

		MaximumStamina = 200;

		LightAttackDamage = 50f;
		HeavyAttackDamage = 75f;

		WalkSpeed = 7.5f;
		CrouchingSpeed = 2.5f;
		runSpeed = 10f;

		LightCooldown = 0.5f;
		HeavyCooldown = 1f;
	}
}
