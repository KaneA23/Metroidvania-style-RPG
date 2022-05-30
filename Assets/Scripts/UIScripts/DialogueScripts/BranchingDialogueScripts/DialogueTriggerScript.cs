using UnityEngine;

/// <summary>
/// Controls when an NPC can be talked to.
/// Created by: Kane Adams
/// </summary>
public class DialogueTriggerScript : MonoBehaviour
{
	[Header("Visual Cue")]
	private bool isInteractable;
	public GameObject visualCue;
	public bool isNewSpeech;

	[Space(5)]
	public Transform playerCheck;
	public LayerMask playerMask;
	private readonly float checkRadius = 3;

	[Header("Ink JSON")]
	[SerializeField] private TextAsset inkJSON; // NPC's dialogue

	// Start is called before the first frame update
	void Start()
	{
		visualCue.SetActive(false);
		isNewSpeech = true;
	}

	// Update is called once per frame
	void Update()
	{
		CheckIfInteractable();

		// If player is close enough to talk to 
		if (isInteractable && !DialogueManagerScript.GetInstance().IsDialoguePlaying && isNewSpeech)
		{
			//.Log("Can interact");
			if (gameObject.name == "Rambleon")
			{
				DialogueManagerScript.GetInstance().isRambleon = true;
				DialogueManagerScript.GetInstance().NPC = gameObject;
			}
			else
			{
				DialogueManagerScript.GetInstance().isRambleon = false;
			}

			if (gameObject.name == "ArenaRambleon")
			{
				DialogueManagerScript.GetInstance().isArenaRambleon = true;
				DialogueManagerScript.GetInstance().NPC = gameObject;
			}
			else
			{
				DialogueManagerScript.GetInstance().isArenaRambleon = false;
			}

			visualCue.SetActive(true);

			if (Input.GetKeyDown(KeyCode.F)/* && !DialogueManagerScript.GetInstance().IsDialoguePlaying*/)
			{
				visualCue.SetActive(false);
				DialogueManagerScript.GetInstance().StartDialogue(inkJSON);
			}
		}
		else
		{
			visualCue.SetActive(false);
		}
	}

	/// <summary>
	/// Checks if player close enough to talk to NPC
	/// </summary>
	private void CheckIfInteractable()
	{
		isInteractable = Physics2D.OverlapCircle(playerCheck.position, checkRadius, playerMask);
	}
}
