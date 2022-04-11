using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls Dialogue System and animations of dialogue box.
/// Created by: Kane Adams
/// </summary>
public class DialogueManager : MonoBehaviour
{
	private Queue<string> sentences;

	public GameObject dialogueBox;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;

	public Animator anim;
	public string currentAnimState;
	public float animDelay;

	public bool isTalking;

	// Start is called before the first frame update
	void Start()
	{
		isTalking = false;
		sentences = new Queue<string>();
		dialogueBox.SetActive(false);

		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// Actives dialogue box and sets up sentences
	/// </summary>
	/// <param name="a_dialogue">the NPC talking and their speach</param>
	public void StartDialogue(Dialogue a_dialogue)
	{
		Cursor.lockState = CursorLockMode.None;
		isTalking = true;
		dialogueBox.SetActive(true);

		nameText.text = a_dialogue.name;
		dialogueText.text = "";

		ChangeAnimationState("DialogueBox_Open");
		animDelay = 1f;

		sentences.Clear();

		foreach (string sentence in a_dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		Invoke(nameof(DisplayNextSentence), animDelay);
	}

	/// <summary>
	/// Adds the new sentence to dialogue box
	/// </summary>
	public void DisplayNextSentence()
	{
		ChangeAnimationState("DialogueBox_IdleOpen");

		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		//dialogueText.text = sentence;
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	/// <summary>
	/// Closes dialogue box
	/// </summary>
	void EndDialogue()
	{
		dialogueText.text = "";
		ChangeAnimationState("DialogueBox_Close");
		animDelay = 1f;

		Cursor.lockState = CursorLockMode.Locked;

		Invoke(nameof(CompleteDialogueAnim), animDelay);
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
		isTalking = false;
	}

	/// <summary>
	/// Adds text letter by letter
	/// </summary>
	/// <param name="a_sentence">new dialogue that is being typed</param>
	/// <returns>letter typed each frame</returns>
	IEnumerator TypeSentence(string a_sentence)
	{
		dialogueText.text = "";
		foreach (char letter in a_sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}
}
