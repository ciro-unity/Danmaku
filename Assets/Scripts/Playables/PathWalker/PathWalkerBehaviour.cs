using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;

[Serializable]
public class PathWalkerBehaviour : PlayableBehaviour
{
    public AnimationCurve path;
    public EnemyDefinition enemyDefinition;
    public BulletPatternDefinition patternDefinition;
    public float xScale = 100f, yScale = 50f;

    private GameObject enemyInstance;
    private GameObject[] bullets;
    private Vector3 offset = Vector3.zero;

    [HideInInspector]
    public Vector3 lanePosition;
    [HideInInspector]
    public float duration;

    //Takes care of instantiating the GameObject
    public override void OnPlayableCreate (Playable playable)
    {
        //create the associated prefab
        if(enemyDefinition != null)
        {
            enemyInstance = GameObject.Instantiate<GameObject>(enemyDefinition.prefab);
            enemyInstance.SetActive(false);
        }

        if(patternDefinition != null)
        {
            int nBullets = Mathf.CeilToInt(duration / patternDefinition.interval);
            bullets = new GameObject[nBullets];
            for(int i=0; i<bullets.Length; i++)
            {
                bullets[i] = GameObject.Instantiate<GameObject>(patternDefinition.prefab);
                bullets[i].SetActive(false);
            }
        }
    }

    //Shows the enemy ship
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if(enemyInstance != null)
        {
            enemyInstance.SetActive(true);
        }
    }

    //Hides the enemy ship
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if(enemyInstance != null)
        {
            enemyInstance.SetActive(false);
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        float clipTime = (float)playable.GetTime();

        //Moves the enemy ship on the path
        Transform lane = playerData as Transform;
        if(lane != null && enemyInstance != null)
        {
            enemyInstance.transform.position = lanePosition + GetOffsetFromPathEnd(clipTime);
        }

        if(patternDefinition != null)
        {
            //Process bullets
            int emittedBullets = Mathf.CeilToInt(clipTime/patternDefinition.interval);
            float timeOfEmission = 0f;
            for(int i = 0; i<emittedBullets; i++)
            {
                timeOfEmission = i * patternDefinition.interval;
                if(timeOfEmission >= clipTime)
                {
                    bullets[i].transform.position = lanePosition + GetOffsetFromPathEnd(timeOfEmission);
                    bullets[i].SetActive(true);
                }
                else
                {
                    //bullets[i].SetActive(false);
                }
            }
        }
    }

    //Given a curve time, it calculates the offset from the path end
    //(the path end is the start position of the lane)
    public Vector3 GetOffsetFromPathEnd(float t)
    {
        float clipTime = t;
        float pathTime = 1f - clipTime;

        offset.x = -clipTime * xScale;
        offset.y = (path.Evaluate(pathTime) - path.Evaluate(1f)) * yScale;

        return offset;
    }

    //Takes care of destroying the GameObject, if it still exists
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

        if(patternDefinition != null)
        {
            for(int i=0; i<bullets.Length; i++)
            {
                if(Application.isPlaying)
                {
                    GameObject.Destroy(bullets[i]);
                }
                else
                {
                    GameObject.DestroyImmediate(bullets[i]);
                }
            }
            bullets = null;
        }
    }
}
