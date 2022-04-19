using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[Header("Referenced Scripts")]
	public Dialogue dialogue;
	private DialogueManager DM;

	[Header("Interactable distance")]
	public bool isInteractable;
	public GameObject interactText;

	[Space(5)]
	public Transform playerCheck;
	public LayerMask playerMask;
	public float checkRadius = 1;

	private void Awake()
	{
		DM = FindObjectOfType<DialogueManager>();
	}

	// Start is called before the first frame update
	void Start()
	{
		interactText.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		CheckIfInteractable();

		if (isInteractable && !DM.isTalking && !gameObject.GetComponent<BarrelHealthSystem>().isExploding)
		{
			interactText.SetActive(true);
			if (Input.GetKeyDown(KeyCode.F))
			{
				TriggerDialogue();
			}
		}
		else
		{
			interactText.SetActive(false);
		}
	}

	private void CheckIfInteractable()
	{
		isInteractable = Physics2D.OverlapCircle(playerCheck.position, checkRadius, playerMask);
	}

	public void TriggerDialogue()
	{
		DM.StartDialogue(dialogue);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(playerCheck.position, checkRadius);
	}
}
