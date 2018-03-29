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

    [HideInInspector]
    public Vector3 lanePosition;
    [HideInInspector]
    public float duration;

    private GameObject enemyInstance;
    private GameObject[] bullets;
    private Vector3 offset = Vector3.zero;


    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    //Takes care of instantiating the GameObject
    public override void OnGraphStart(Playable playable)
    {
        duration = (float)playable.GetDuration();

        //create the associated prefab
        if(enemyDefinition != null)
        {
            enemyInstance = GameObject.Instantiate<GameObject>(enemyDefinition.prefab);
            enemyInstance.SetActive(false);
        }

        //bullet creation
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


    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //Shows the enemy ship
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
            enemyInstance.transform.position = lanePosition + GetOffsetFromPathEnd(clipTime * enemyDefinition.speed);
        }

        if(patternDefinition != null)
        {
            //Process bullets
            int emittedBulletsSoFar = Mathf.CeilToInt(clipTime/patternDefinition.interval);
            for(int i = 0; i<bullets.Length; i++)
            {
                float timeOfEmission = i * patternDefinition.interval;
                float deltaTime = (Application.isPlaying) ? .02f : .02f;
                Vector3 smallOffset = Vector3.left * 5f;
                Vector3 bulletMovement = patternDefinition.direction.normalized * deltaTime * (clipTime-timeOfEmission) * patternDefinition.speed * 100f;
                bullets[i].transform.position = lanePosition + GetOffsetFromPathEnd(timeOfEmission * enemyDefinition.speed) + bulletMovement + smallOffset;
                if(timeOfEmission < clipTime - .01f) //artificial "one-frame" delay
                {
                    bullets[i].SetActive(true);
                }
                else
                {
                    bullets[i].SetActive(false);
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

    public override void OnGraphStop(Playable playable)
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

    //Takes care of destroying the GameObject, if it still exists
    public override void OnPlayableDestroy(Playable playable)
    {
        
    }
}
