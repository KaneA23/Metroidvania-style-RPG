using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

/// <summary>
/// Controls NPC's dialogue, what they say and how they respond to player choices
/// Created by: Kane Adams
/// </summary>
public class DialogueManagerScript : MonoBehaviour
{
	[Header("Globals Ink File")]
	//[SerializeField] private InkFile globalsInkFile;
	[SerializeField] private TextAsset loadGlobalsJSON;

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

	[SerializeField] private GameObject continueButton;

	private const string SPEAKER_TAG = "speaker";

	private readonly float typingSpeed = 0.03f;

	[Header("Dialogue Box Animations")]
	[SerializeField] private Animator anim;
	private string currentAnimState;
	private float animDelay;

	public bool isRambleon;
	public bool isArenaRambleon;
	public GameObject NPC;
	public bool isReady;

	private DialogueVariablesScript DVS;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Found more than one Dialogue Manager in the scene");
		}
		instance = this;

		//DVS = new DialogueVariablesScript(globalsInkFile.filePath);
		DVS = new DialogueVariablesScript(loadGlobalsJSON);
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

	/// <summary>
	/// Adds dialogue box and NPC begins speaking
	/// </summary>
	/// <param name="a_inkJSON">NPC's dialogue</param>
	public void StartDialogue(TextAsset a_inkJSON)
	{
		currentStory = new Story(a_inkJSON.text);
		IsDialoguePlaying = true;
		canContinueNextLine = true;
		dialogueBox.SetActive(true);

		DVS.StartListening(currentStory);

		nameText.text = "";
		dialogueText.text = "";
		continueButton.SetActive(false);
		HideChoices();

		ChangeAnimationState("DialogueBox_Open");
		animDelay = 1f;
		Invoke(nameof(ContinueDialogue), animDelay);
	}

	/// <summary>
	/// If the NPC has more dialogue, the next line is outputted
	/// </summary>
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

	/// <summary>
	/// Removes dialogue box
	/// </summary>
	private void EndDialogue()
	{
		dialogueText.text = "";

		ChangeAnimationState("DialogueBox_Close");
		animDelay = 1f;
		//Debug.Log("ending dialogue");

		if (isRambleon)
		{
			NPC.GetComponent<BarrelHealthSystem>().ActivateParticle();
			NPC.GetComponent<DialogueTriggerScript>().isNewSpeech = false;
		}

		if (isArenaRambleon)
		{
			FindObjectOfType<ArenaWaveManager>().EndDialogue();
		}

		Invoke(nameof(CompleteDialogueAnim), animDelay);
	}

	/// <summary>
	/// Hides choice buttons when not needed
	/// </summary>
	private void HideChoices()
	{
		foreach (GameObject choiceButton in choices)
		{
			choiceButton.SetActive(false);
		}
	}

	/// <summary>
	/// Shows choice buttons when choice to be made
	/// </summary>
	private void DisplayChoices()
	{
		List<Choice> currentChoices = currentStory.currentChoices;

		// hides continue button if choice to be made
		if (currentChoices.Count > 0)
		{
			continueButton.SetActive(false);
		}
		else
		{
			continueButton.GetComponentInChildren<Toggle>().isOn = false;
			continueButton.SetActive(true);
			continueButton.GetComponentInChildren<Toggle>().interactable = true;
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

	/// <summary>
	/// Changes dialogue branch depending on choice player presses
	/// </summary>
	/// <param name="a_choiceIndex">button pressed</param>
	public void MakeChoice(int a_choiceIndex)
	{
		currentStory.ChooseChoiceIndex(a_choiceIndex);
		ContinueDialogue();
	}

	/// <summary>
	/// Changes line when player clicks continue dialogue
	/// </summary>
	public void ContinueClicked()
	{
		if (!continueButton.GetComponentInChildren<Toggle>().isOn)
		{
			return;
		}

		if (!canContinueNextLine)
		{
			return;
		}
		else
		{
			Invoke(nameof(ContinueDialogue), 0.1f);
			continueButton.GetComponentInChildren<Toggle>().interactable = false;
		}
	}

	/// <summary>
	/// Assigns NPC names dependent on who is speaking
	/// </summary>
	/// <param name="a_currentTags">who is speaking</param>
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
		//Debug.Log("Dialogue Completed");

		DVS.StopListening(currentStory);

		dialogueBox.SetActive(false);
		IsDialoguePlaying = false;
	}

	/// <summary>
	/// Types out a line of speach as indivual characters
	/// </summary>
	/// <param name="a_line">current line to be split into letters</param>
	/// <returns>Waits set time before typing next letter</returns>
	private IEnumerator TypeLine(string a_line)
	{
		dialogueText.text = a_line;
		dialogueText.maxVisibleCharacters = 0;
		canContinueNextLine = false;
		continueButton.SetActive(false);
		HideChoices();

		bool isAddingRichTextTag = false;

		for (int i = 0; i < a_line.ToCharArray().Length; i++)
		{
			if (a_line[i] == '<' || isAddingRichTextTag)
			{
				isAddingRichTextTag = true;
				if (a_line[i] == '>')
				{
					isAddingRichTextTag = false;
				}
			}
			else
			{
				dialogueText.maxVisibleCharacters++;
				yield return new WaitForSeconds(typingSpeed);
			}
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

	/// <summary>
	/// Checks if any variable has been changed by inky file
	/// </summary>
	/// <param name="a_variableName">Variable that is being checked</param>
	/// <returns>New value stored in inky variable</returns>
	public Ink.Runtime.Object GetVariableState(string a_variableName)
	{
		Ink.Runtime.Object variableValue = null;
		DVS.Variables.TryGetValue(a_variableName, out variableValue);
		if (variableValue == null)
		{
			Debug.LogWarning("Ink Variable was found to be null: " + a_variableName);
		}
		return variableValue;
	}
}
