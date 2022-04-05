using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;

public class DialogueManagerScript : MonoBehaviour
{
	[Header("Dialogue UI")]
	[SerializeField] private GameObject dialogueBox;

	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI dialogueText;

	[Header("Choices UI")]
	[SerializeField] private GameObject[] choices;
	private TextMeshProUGUI[] choicesText;

	private Story currentStory;
	public bool IsDialoguePlaying { get; private set; }

	private bool canContinueNextLine;

	private Coroutine typeLineCoroutine;

	private static DialogueManagerScript instance;

	public GameObject continueButton;

	private const string SPEAKER_TAG = "speaker";

	private readonly float typingSpeed = 0.03f;

	[Header("Dialogue Box Animations")]
	public Animator anim;
	public string currentAnimState;
	public float animDelay;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Found more than one Dialogue Manager in the scene");
		}
		instance = this;
	}

	public static DialogueManagerScript GetInstance()
	{
		return instance;
	}

	// Start is called before the first frame update
	void Start()
	{
		IsDialoguePlaying = false;
		canContinueNextLine = false;
		dialogueBox.SetActive(false);

		// Get all choices text
		choicesText = new TextMeshProUGUI[choices.Length];
		int index = 0;
		foreach (GameObject choice in choices)
		{
			choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
			index++;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!IsDialoguePlaying)
		{
			return;
		}
	}

	public void StartDialogue(TextAsset a_inkJSON)
	{
		currentStory = new Story(a_inkJSON.text);
		IsDialoguePlaying = true;
		canContinueNextLine = true;
		dialogueBox.SetActive(true);

		nameText.text = "";
		dialogueText.text = "";
		continueButton.SetActive(false);
		HideChoices();

		ChangeAnimationState("DialogueBox_Open");
		animDelay = 1f;

		Invoke(nameof(ContinueDialogue), animDelay);
		//ContinueDialogue();
	}

	public void ContinueDialogue()
	{
		ChangeAnimationState("DialogueBox_IdleOpen");

		if (currentStory.canContinue)
		{
			if (typeLineCoroutine != null)
			{
				StopCoroutine(typeLineCoroutine);
			}

			typeLineCoroutine = StartCoroutine(TypeLine(currentStory.Continue()));
			HandleTags(currentStory.currentTags);
		}
		else
		{
			EndDialogue();
		}
	}

	private void EndDialogue()
	{
		//IsDialoguePlaying = false;
		//.SetActive(false);
		dialogueText.text = "";

		ChangeAnimationState("DialogueBox_Close");
		animDelay = 1f;

		Invoke(nameof(CompleteDialogueAnim), animDelay);
	}

	private void HideChoices()
	{
		foreach (GameObject choiceButton in choices)
		{
			choiceButton.SetActive(false);
		}
	}

	private void DisplayChoices()
	{
		List<Choice> currentChoices = currentStory.currentChoices;

		if (currentChoices.Count > 0)
		{
			continueButton.SetActive(false);
		}
		else
		{
			continueButton.SetActive(true);
		}

		// Checks if UI can support number of choices
		if (currentChoices.Count > choices.Length)
		{
			Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
		}

		int index = 0;
		// enable and initialize choices up to amount of choices for line of dialogue
		foreach (Choice choice in currentChoices)
		{
			choices[index].SetActive(true);
			choicesText[index].text = choice.text;
			index++;
		}

		// Hides unrequired choice boxes
		for (int i = index; i < choices.Length; i++)
		{
			choices[i].SetActive(false);
		}

		//StartCoroutine(SelectFirstChoice());
	}

	public void MakeChoice(int a_choiceIndex)
	{
		currentStory.ChooseChoiceIndex(a_choiceIndex);
		ContinueDialogue();
	}

	public void ContinueClicked()
	{
		if (!canContinueNextLine)
		{
			return;
		}
		else
		{
			ContinueDialogue();
		}
	}

	private void HandleTags(List<string> a_currentTags)
	{
		foreach (string tag in a_currentTags)
		{
			string[] splitTag = tag.Split(':');
			if (splitTag.Length != 2)
			{
				Debug.Log("Tag could not be appropiately parsed: " + tag);
			}
			string tagKey = splitTag[0].Trim();
			string tagValue = splitTag[1].Trim();

			switch (tagKey)
			{
				case SPEAKER_TAG:
					nameText.text = tagValue;
					break;

				default:
					Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
					break;
			}
		}
	}



	/// <summary>
	/// Changes the animation for the dialogue box
	/// </summary>
	/// <param name="a_newAnim">is the dialogue box opening or closing?</param>
	public void ChangeAnimationState(string a_newAnim)
	{
		// Stops the same animation from interrupting itself
		if (currentAnimState == a_newAnim)
		{
			return;
		}

		// Play the animation
		anim.Play(a_newAnim);

		// reassign current state
		currentAnimState = a_newAnim;
	}

	/// <summary>
	/// Deactivates Dialogue box
	/// </summary>
	void CompleteDialogueAnim()
	{
		dialogueBox.SetActive(false);
		IsDialoguePlaying = false;
	}

	private IEnumerator TypeLine(string a_line)
	{
		dialogueText.text = a_line;
		dialogueText.maxVisibleCharacters = 0;
		canContinueNextLine = false;
		continueButton.SetActive(false);
		HideChoices();

		for (int i = 0; i < a_line.ToCharArray().Length; i++)
		{
			dialogueText.maxVisibleCharacters++;
			yield return new WaitForSeconds(typingSpeed);
		}

		canContinueNextLine = true;
		DisplayChoices();
	}

	//private IEnumerator SelectFirstChoice()
	//{
	//	EventSystem.current.SetSelectedGameObject(null);
	//	yield return new WaitForEndOfFrame();

	//	EventSystem.current.SetSelectedGameObject(choices[0]);
	//}
}
