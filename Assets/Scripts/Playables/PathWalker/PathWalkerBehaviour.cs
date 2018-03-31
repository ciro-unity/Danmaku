using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEngine.Animations;

[Serializable]
public class PathWalkerBehaviour : PlayableBehaviour
{
    public AnimationCurve path;
    public EnemyDefinition enemyDefinition;
    public BulletPatternDefinition patternDefinition;
    public float xScale = 100f, yScale = 50f;

    [HideInInspector] public Vector3 lanePosition; //fed by the clip
    [HideInInspector] public double clipStart; //fed by the clip
    [HideInInspector] public double duration; //used in the custom inspector

    private GameObject shipInstance;
    private GameObject[] bullets;
    private Vector3 offset = Vector3.zero;
    private Vector3 previousShipPosition;
    private Vector3 moveDifference;
    private float previousHorDiff, previousVertDiff;
    private Animator shipAnimator;

    private int horizontalHash, verticalHash, shootHash;

    public override void OnPlayableCreate (Playable playable)
    {
        //hashing the strings into ints for performance
        horizontalHash = Animator.StringToHash("HorizontalMovement");
        verticalHash = Animator.StringToHash("VerticalMovement");
        shootHash = Animator.StringToHash("Shoot");
        previousShipPosition = lanePosition;
    }

    //Takes care of instantiating the GameObject
    public override void OnGraphStart(Playable playable)
    {
        //create the associated prefab
        if(enemyDefinition != null)
        {
            shipInstance = GameObject.Instantiate<GameObject>(enemyDefinition.prefab);
            shipInstance.transform.right = Vector3.left;
            shipAnimator = shipInstance.GetComponent<Animator>();
            shipInstance.SetActive(false);
        }

        //bullet creation
        if(patternDefinition != null)
        {
            int nBullets = Mathf.CeilToInt((float)duration / patternDefinition.interval);
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
        if(shipInstance != null)
        {
            shipInstance.SetActive(true);
        }
    }

    //Hides the enemy ship
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if(shipInstance != null)
        {
            //enemyInstance.SetActive(false);
        }
    }

    public void MixerProcessFrame(Playable thisPlayable, FrameData info, object playerData, double timelineCurrentTime)
    {
        //double clipTime = thisPlayable.GetTime();
        double globalClipTime = timelineCurrentTime - clipStart;

        //Moves the enemy ship on the path
        Transform lane = playerData as Transform;
        if(lane != null && shipInstance != null)
        {
            shipInstance.transform.position = lanePosition + GetOffsetFromPathEnd(globalClipTime * enemyDefinition.speed);
            if(Application.isPlaying)
            {
                moveDifference = (shipInstance.transform.position - previousShipPosition) * 2f;
                float step = Time.deltaTime * 2f;
                float newAnimatorX = Mathf.Lerp(previousHorDiff, moveDifference.x, step);
                float newAnimatorY = Mathf.Lerp(previousVertDiff, moveDifference.y, step);
                shipAnimator.SetFloat(horizontalHash, newAnimatorX);
                shipAnimator.SetFloat(verticalHash, newAnimatorY);
                previousHorDiff = newAnimatorX;
                previousVertDiff = newAnimatorY;

                previousShipPosition = shipInstance.transform.position;
            }
        }

        if(patternDefinition != null)
        {
            //Process bullets
            //int emittedBulletsSoFar = Mathf.CeilToInt(globalClipTime/patternDefinition.interval);
            for(int i = 0; i<bullets.Length; i++)
            {
                float timeOfEmission = i * patternDefinition.interval;
                
                if(timeOfEmission < globalClipTime)
                {
                    //bullet has been shot
                    if(!bullets[i].activeSelf)
                        bullets[i].SetActive(true);

                    float deltaTime = .02f; //(Application.isPlaying) ? Time.smoothDeltaTime : .02f;
                    Vector3 smallOffset = Vector3.left * 5f;
                    Vector3 bulletMovement = patternDefinition.direction.normalized * deltaTime * ((float)globalClipTime-timeOfEmission) * patternDefinition.speed * 100f;
                    bullets[i].transform.position = lanePosition + GetOffsetFromPathEnd(timeOfEmission * enemyDefinition.speed) + bulletMovement + smallOffset;
                }
                else
                {
                    //bullet hasn't been shot yet
                    if(bullets[i].activeSelf)
                        bullets[i].SetActive(false);
                }
            }
        }
    }

    //Given a curve time, it calculates the offset from the path end
    //(the path end is the start position of the lane)
    public Vector3 GetOffsetFromPathEnd(double t)
    {
        double pathTime = 1f - t;

        offset.x = (float)(-t * xScale);
        offset.y = (path.Evaluate((float)pathTime) - path.Evaluate(1f)) * yScale;

        return offset;
    }

    public override void OnGraphStop(Playable playable)
    {
        if(shipInstance != null)
        {
            if(Application.isPlaying)
            {
                GameObject.Destroy(shipInstance);
            }
            else
            {
                GameObject.DestroyImmediate(shipInstance);
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
