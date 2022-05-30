using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls when new round starts.
/// Created by: Kane Adams
/// </summary>
public class ArenaWaveManager : MonoBehaviour
{
	[Header("Referenced Scripts")]
	[SerializeField] private ArenaEnemySpawner AEM1;
	[SerializeField] private ArenaEnemySpawner AEM2;
	[SerializeField] private CameraFollowScript CFS;

	private string enemyTag = "Enemy";

	[SerializeField] private GameObject[] enemies;

	public int wave;

	int countdownTimer;

	bool isSpawning;

	public bool isReady;

	[SerializeField] private GameObject rambleon;

	[SerializeField] private Image countdownImage;

	[SerializeField] private Image leftNumber;
	[SerializeField] private Image rightNumber;

	int roundDigit1;
	int roundDigit2;

	[SerializeField] private Sprite[] numberSprites;

	// Start is called before the first frame update
	void Start()
	{
		CFS.cameraState = CameraState.CAM_FOLLOWING;

		wave = 0;
		roundDigit1 = wave / 10;
		ChangeWaveImage(leftNumber, roundDigit1);
		roundDigit2 = wave % 10;
		ChangeWaveImage(rightNumber, roundDigit2);

		countdownImage.enabled = false;

		isSpawning = false;
		PlayerPrefs.SetInt("WAVENUMBER", wave);

		rambleon.SetActive(true);
	}

	// Update is called once per frame
	void Update()
	{
		enemies = GameObject.FindGameObjectsWithTag(enemyTag);

		if (enemies.Length == 0 && !isSpawning)
		{
			CFS.cameraState = CameraState.CAM_FOLLOWING;
			rambleon.SetActive(true);
		}
	}

	/// <summary>
	/// After Rambleon stops talking, starts to countdown
	/// </summary>
	public void EndDialogue()
	{
		isReady = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("startRound")).value;

		if (isReady)
		{
			CFS.cameraState = CameraState.CAM_ARENA;
			rambleon.SetActive(false);

			wave++;
			roundDigit1 = wave / 10;
			ChangeWaveImage(leftNumber, roundDigit1);
			roundDigit2 = wave % 10;
			ChangeWaveImage(rightNumber, roundDigit2);

			isSpawning = true;
			countdownTimer = 3;
			Invoke(nameof(Countdown), 1f);

			PlayerPrefs.SetInt("WAVENUMBER", wave);
		}
	}

	/// <summary>
	/// Decreases countdown time until reaches zero for round to begin
	/// </summary>
	void Countdown()
	{
		if (countdownTimer >= 0)
		{
			countdownImage.enabled = true;
			ChangeWaveImage(countdownImage, countdownTimer);
			countdownTimer--;
			Invoke(nameof(Countdown), 1f);
		}
		else
		{
			countdownImage.enabled = false;
			BeginSpawn();
		}
	}

	/// <summary>
	/// Calls Spawner functions to start spawning different enemy types
	/// </summary>
	void BeginSpawn()
	{
		AEM1.previousSpawn = null;
		AEM2.previousSpawn = null;

		AEM1.CheckWave(wave);
		AEM2.CheckWave(wave);

		for (int i = 0; i < AEM1.spawnNumber; i++)
		{
			AEM1.SpawnEnemy();
		}

		for (int i = 0; i < AEM2.spawnNumber; i++)
		{
			AEM2.SpawnEnemy();
		}

		isSpawning = false;
	}

	/// <summary>
	/// Alters the numbered image values
	/// </summary>
	/// <param name="a_number">The number image to change</param>
	/// <param name="a_round">The new round (or time)</param>
	void ChangeWaveImage(Image a_number, int a_round)
	{
		a_number.sprite = numberSprites[a_round];
	}
}
