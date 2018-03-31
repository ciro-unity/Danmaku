using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PathWalkerClip : PlayableAsset, ITimelineClipAsset
{
    public PathWalkerBehaviour template = new PathWalkerBehaviour();
    
    //Data provided by the track, will be passed to the Behaviour
    [HideInInspector] public Vector3 lanePosition; //just used as a temporary variable for the CustomInspector to use
    [HideInInspector] public double clipStart;
    [HideInInspector] public double realDuration;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    //this forces the clip to be 1 second at the beginning
    public override double duration
    {
        get
        {
            return 1d;
        }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        template.lanePosition = lanePosition;
        template.clipStart = clipStart;
        template.duration = realDuration;
        var playable = ScriptPlayable<PathWalkerBehaviour>.Create (graph, template);
        return playable;
    }
}
