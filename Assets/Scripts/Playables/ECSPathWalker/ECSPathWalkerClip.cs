using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ECSPathWalkerClip : PlayableAsset, ITimelineClipAsset
{
    public ECSPathWalkerBehaviour template = new ECSPathWalkerBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ECSPathWalkerBehaviour>.Create (graph, template);
        ECSPathWalkerBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
