using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArenaWaveManager : MonoBehaviour
{
	[Header("Referenced Scripts")]
	[SerializeField] private ArenaEnemySpawner AEM1;
	[SerializeField] private ArenaEnemySpawner AEM2;
	[SerializeField] private CameraFollowScript CFS;

	private string enemyTag = "Enemy";

	[SerializeField] private GameObject[] enemies;

	public int wave;

	//[SerializeField] private TextMeshProUGUI waveText;
	//[SerializeField] private TextMeshProUGUI countdownText;
	int countdownTimer;

	bool isSpawning;

	//bool isWave;
	public bool isReady;

	[SerializeField] private GameObject rambleon;

	[SerializeField] private Image countdownImage;

	[SerializeField] private Image leftNumber;
	[SerializeField] private Image rightNumber;

	int roundDigit1;
	int roundDigit2;

	[SerializeField] private Sprite[] numberSprites;

	private void Awake()
	{

	}

	// Start is called before the first frame update
	void Start()
	{
		CFS.cameraState = CameraState.CAM_FOLLOWING;

		wave = 0;
		//waveText.text = "Wave: " + wave;
		roundDigit1 = wave / 10;
		ChangeWaveImage(leftNumber, roundDigit1);
		roundDigit2 = wave % 10;
		ChangeWaveImage(rightNumber, roundDigit2);

		//countdownText.text = string.Empty;
		countdownImage.enabled = false;

		isSpawning = false;
		PlayerPrefs.SetInt("WAVENUMBER", wave);
		//isWave = false;

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

		//if (isReady && enemies.Length <= 0 && countdownTimer <= 0 && !isSpawning)
		//{
		//	rambleon.SetActive(false);
		//	wave++;
		//	waveText.text = "Wave: " + wave;
		//	isSpawning = true;
		//	countdownTimer = 3;
		//	Invoke(nameof(Countdown), 1f);
		//}
	}

	public void EndDialogue()
	{
		isReady = ((Ink.Runtime.BoolValue)DialogueManagerScript.GetInstance().GetVariableState("startRound")).value;

		if (isReady)
		{
			CFS.cameraState = CameraState.CAM_ARENA;
			rambleon.SetActive(false);
			wave++;
			//waveText.text = "Wave: " + wave;
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

	void Countdown()
	{
		if (countdownTimer >= 0)
		{
			countdownImage.enabled = true;
			ChangeWaveImage(countdownImage, countdownTimer);
			//countdownText.text = countdownTimer.ToString();
			countdownTimer--;
			Invoke(nameof(Countdown), 1f);
		}
		else
		{
			//countdownText.text = string.Empty;
			countdownImage.enabled = false;
			BeginSpawn();
		}
	}

	void BeginSpawn()
	{
		//isWave = true;

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

	void ChangeWaveImage(Image a_number, int a_round)
	{
		a_number.sprite = numberSprites[a_round];
	}
}
