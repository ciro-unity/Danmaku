using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PathWalkerClip : PlayableAsset, ITimelineClipAsset
{
    public PathWalkerBehaviour template = new PathWalkerBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PathWalkerBehaviour>.Create (graph, template);
        PathWalkerBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
