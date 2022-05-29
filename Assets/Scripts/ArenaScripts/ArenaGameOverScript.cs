using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaGameOverScript : MonoBehaviour
{
	int finalRound;

	int roundDigit1;
	int roundDigit2;

	[SerializeField] private Image leftNumber;
	[SerializeField] private Image rightNumber;

	[SerializeField] private Sprite[] numberSprites;

	// Start is called before the first frame update
	void Start()
	{
		finalRound = PlayerPrefs.GetInt("WAVENUMBER");
		Debug.Log("Round: " + finalRound);

		// Separates the round numbers into individual digits
		roundDigit1 = finalRound / 10;
		ChangeRoundImage(leftNumber, roundDigit1);
		roundDigit2 = finalRound % 10;
		ChangeRoundImage(rightNumber, roundDigit2);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void ChangeRoundImage(Image a_number, int a_round)
	{
		a_number.sprite = numberSprites[a_round];
	}
}
