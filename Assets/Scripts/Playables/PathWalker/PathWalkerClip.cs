using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PathWalkerClip : PlayableAsset, ITimelineClipAsset
{
    public PathWalkerBehaviour template = new PathWalkerBehaviour();
    
    // public AnimationCurve path;
    // public EnemyDefinition enemyDefinition;
    // public BulletPatternDefinition patternDefinition;
    // public float xScale = 100f, yScale = 100f;
    
    [HideInInspector]
    public Vector3 lanePosition; //just used as a temporary variable for the CustomInspector to use

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
        var playable = ScriptPlayable<PathWalkerBehaviour>.Create (graph, template);
        return playable;
    }
}
