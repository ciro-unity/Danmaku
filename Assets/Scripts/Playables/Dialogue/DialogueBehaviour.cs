using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class DialogueBehaviour : PlayableBehaviour
{
    public DialogueBit bitOfDialogue;

    private bool dialogueHidden = false;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if(bitOfDialogue != null)
        {
            UIManager.Instance.ShowDialogue(bitOfDialogue);
        }
        dialogueHidden = false;
    }

    //Will hide the dialogue in the last .4f seconds of the clip
    //so that a following piece of dialogue doesn't overlap
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if(!dialogueHidden
            && (float)(playable.GetDuration() - playable.GetTime()) < .4f)
        {
            UIManager.Instance.HideDialogue();
            dialogueHidden = true;
        }
    }
}
