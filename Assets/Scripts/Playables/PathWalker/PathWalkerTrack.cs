using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.7846687f, 0.4103774f, 1f)]
[TrackClipType(typeof(PathWalkerClip))]
[TrackBindingType(typeof(Transform))]
public class PathWalkerTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<PathWalkerMixerBehaviour>.Create (graph, inputCount);
    }
}
