using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PathWalkerBehaviour : PlayableBehaviour
{
    public AnimationCurve path;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
