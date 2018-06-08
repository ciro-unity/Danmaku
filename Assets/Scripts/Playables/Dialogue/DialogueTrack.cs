using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 1f, 0.6335554f)]
[TrackClipType(typeof(DialogueClip))]
public class DialogueTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DialogueMixerBehaviour>.Create (graph, inputCount);
    }
}
