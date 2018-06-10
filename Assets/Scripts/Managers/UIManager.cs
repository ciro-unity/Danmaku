using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	public Animator dialogueAnimator;
	public PortraitDisplayer portraitDisplayer;
	public Text characterNameLabel, dialogueLabel;
	public AudioSource panelAudioSource;
	
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
		if(Application.isPlaying)
		{
			dialogueAnimator.SetTrigger(showDialogueTriggerHash);
			StartCoroutine(PlayVoiceClip(bitOfDialogue.audioClip));
		}
	}

	public IEnumerator PlayVoiceClip(AudioClip clip)
	{
		yield return new WaitForSeconds(.5f);

		panelAudioSource.PlayOneShot(clip);
	}

	public void HideDialogue()
	{
		if(Application.isPlaying)
		{
			dialogueAnimator.SetTrigger(hideDialogueTriggerHash);
		}
	}
}