using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	public Animator dialogueAnimator;
	public PortraitDisplayer portraitDisplayer;
	public Text characterNameLabel, dialogueLabel;
	
	private int showDialogueTriggerHash, hideDialogueTriggerHash;

	private void Awake()
	{
		showDialogueTriggerHash = Animator.StringToHash("ShowDialogue");
		hideDialogueTriggerHash = Animator.StringToHash("HideDialogue");
	}

	public void ShowDialogue(DialogueBit bitOfDialogue)
	{
		characterNameLabel.text = bitOfDialogue.characterName;
		dialogueLabel.text = bitOfDialogue.line;
		portraitDisplayer.DisplayCharacter(bitOfDialogue.character);
		dialogueAnimator.SetTrigger(showDialogueTriggerHash);
	}

	public void HideDialogue()
	{
		dialogueAnimator.SetTrigger(hideDialogueTriggerHash);
	}
}