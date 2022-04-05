using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerScript : MonoBehaviour
{
	[Header("Visual Cue")]
	public bool isInteractable;
	public GameObject visualCue;

	[Space(5)]
	public Transform playerCheck;
	public LayerMask playerMask;
	public float checkRadius = 3;

	[Header("Ink JSON")]
	[SerializeField] private TextAsset inkJSON;

	// Start is called before the first frame update
	void Start()
	{
		visualCue.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		CheckIfInteractable();

		if (isInteractable && !DialogueManagerScript.GetInstance().IsDialoguePlaying)
		{
			visualCue.SetActive(true);
			if (Input.GetKeyDown(KeyCode.F) && !DialogueManagerScript.GetInstance().IsDialoguePlaying)
			{
				DialogueManagerScript.GetInstance().StartDialogue(inkJSON);
			}
		}
		else
		{
			visualCue.SetActive(false);
		}
	}

	private void CheckIfInteractable()
	{
		isInteractable = Physics2D.OverlapCircle(playerCheck.position, checkRadius, playerMask);
	}
}
