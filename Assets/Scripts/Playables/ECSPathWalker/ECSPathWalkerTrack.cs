using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.78996f, 0.9433962f, 0f)]
[TrackClipType(typeof(ECSPathWalkerClip))]
[TrackBindingType(typeof(Transform))]
public class ECSPathWalkerTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<ECSPathWalkerMixerBehaviour>.Create (graph, inputCount);
    }
}