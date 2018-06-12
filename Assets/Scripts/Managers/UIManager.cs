using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
	public Animator dialogueAnimator;
	public PortraitDisplayer portraitDisplayer;
	public TextMeshProUGUI characterNameLabel, dialogueLabel, hitsCounter;
	public AudioSource panelAudioSource;
	public Transform healthBar;
	
	private int showDialogueTriggerHash, hideDialogueTriggerHash;

	private void Awake()
	{
		showDialogueTriggerHash = Animator.StringToHash("ShowDialogue");
		hideDialogueTriggerHash = Animator.StringToHash("HideDialogue");
	}

	private void Start()
	{
		hitsCounter.text = "000";
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

	public void OnPlayerHit(float fraction)
	{
		healthBar.localScale = new Vector3(fraction, 1f, 1f);
	}

	public void OnEnemyDown(int howManyHits)
	{
		hitsCounter.text = string.Format("{0:000}", howManyHits);
	}
}