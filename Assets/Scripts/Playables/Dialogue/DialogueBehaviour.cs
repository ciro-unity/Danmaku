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
        if(Application.isPlaying)
        {
            UIManager.Instance.ShowDialogue(bitOfDialogue);
        }
    }

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if(!dialogueHidden
            && (float)(playable.GetDuration() - playable.GetTime()) < .4f)
        {
            if(Application.isPlaying)
            {
                UIManager.Instance.HideDialogue();
                dialogueHidden = true;
            }
        }
    }
}
