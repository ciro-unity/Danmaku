using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class PathWalkerBehaviour : PlayableBehaviour
{
    public AnimationCurve path;
    [SerializeField]
    public Enemy enemyDefinition;
    public float xScale = 100f, yScale = 50f;

    private GameObject enemyInstance;
    private Vector3 offset = Vector3.zero;
    private float pathFinalY;

    public override void OnPlayableCreate (Playable playable)
    {
        //create the associated prefab
        if(enemyDefinition != null)
        {
            enemyInstance = GameObject.Instantiate<GameObject>(enemyDefinition.prefab);
            enemyInstance.SetActive(false);
        }

        if(path != null)
        {
            pathFinalY = path.Evaluate(1f);
        }
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if(enemyInstance != null)
        {
            enemyInstance.SetActive(true);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if(enemyInstance != null)
        {
            enemyInstance.SetActive(false);
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Transform lane = playerData as Transform;
        if(lane != null && enemyInstance != null)
        {
            float clipTime = (float)playable.GetTime();
            float pathTime = 1f - clipTime;

            offset.x = -clipTime * xScale;
            offset.y = (path.Evaluate(pathTime) - pathFinalY) * yScale;
            enemyInstance.transform.position = lane.position + offset;

            
        }
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        if(enemyInstance != null)
        {
            if(Application.isPlaying)
            {
                GameObject.Destroy(enemyInstance);
            }
            else
            {
                GameObject.DestroyImmediate(enemyInstance);
            }
        }
    }
}
