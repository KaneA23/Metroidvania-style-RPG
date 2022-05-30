/// <summary>
/// Controls class collection for BasePlayerScript.cs
/// Created by: Kane Adams
/// </summary>
[System.Serializable]
public class BasePlayer
{
	private string playerName;
	private int playerLvl;
	private BasePlayerClass playerClass;
	private string className;

	private int maxHP;
	private int maxMP;
	private float maxMPRegen;
	private int maxStam;
	private float lightDmg;
	private float heavyDmg;
	private float walkSpeed;
	private float crouchSpeed;
	private float runSpeed;
	private float lightCooldown;
	private float heavyCooldown;

	public string PlayerName
	{
		get { return playerName; }
		set { playerName = value; }
	}
	public int PlayerLvl
	{
		get { return playerLvl; }
		set { playerLvl = value; }
	}
	public BasePlayerClass PlayerClass
	{
		get { return playerClass; }
		set { playerClass = value; }
	}
	public string ClassName
	{
		get { return className; }
		set { className = value; }
	}
	public int MaxHP
	{
		get { return maxHP; }
		set { maxHP = value; }
	}
	public int MaxMP
	{
		get { return maxMP; }
		set { maxMP = value; }
	}
	public float MaxMPRegen
	{
		get { return maxMPRegen; }
		set { maxMPRegen = value; }
	}
	public int MaxStam
	{
		get { return maxStam; }
		set { maxStam = value; }
	}
	public float LightDamage
	{
		get { return lightDmg; }
		set { lightDmg = value; }
	}
	public float HeavyDamage
	{
		get { return heavyDmg; }
		set { heavyDmg = value; }
	}
	public float WalkSpeed
	{
		get { return walkSpeed; }
		set { walkSpeed = value; }
	}
	public float CrouchSpeed
	{
		get { return crouchSpeed; }
		set { crouchSpeed = value; }
	}
	public float RunSpeed
	{
		get { return runSpeed; }
		set { runSpeed = value; }
	}
	public float LightAttackCooldown
	{
		get { return lightCooldown; }
		set { lightCooldown = value; }
	}
	public float HeavyAttackCooldown
	{
		get { return heavyCooldown; }
		set { heavyCooldown = value; }
	}
}
