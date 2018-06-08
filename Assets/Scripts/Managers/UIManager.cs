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

	public void ShowDialogue(string charName, string dialogue, GameCharacters character)
	{
		characterNameLabel.text = charName;
		dialogueLabel.text = dialogue;
		portraitDisplayer.DisplayCharacter(character);
		dialogueAnimator.SetTrigger(showDialogueTriggerHash);
	}
}

//Characters appear in this enum in the order they appear in the scene, left to right
public enum GameCharacters
{
	Kyle,
	PunkGirl,
}