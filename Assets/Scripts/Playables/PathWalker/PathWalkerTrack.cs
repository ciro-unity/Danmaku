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
        var playable = ScriptPlayable<PathWalkerMixerBehaviour>.Create(graph, inputCount);

        //store the lane starting position in the clips to allow correct calculation of the paths
        Transform lane = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as Transform;
        foreach (var clip in m_Clips)
        {
            var playableAsset = clip.asset as PathWalkerClip;
			playableAsset.lanePosition = lane.position;
        }

        return playable;
    }
}
