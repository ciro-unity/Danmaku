using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEditor;
using UnityEngine.Animations;
using UnityEngine.Serialization;

[Serializable]
public class PathWalkerBehaviour : PlayableBehaviour
{
    public AnimationCurve path;
    public PathObjectDefinition objectDefinition;
    public BulletPatternDefinition patternDefinition;
    public float xScale = 100f, yScale = 50f;

    [HideInInspector] public Vector3 lanePosition; //fed by the clip
    [HideInInspector] public double clipStart; //fed by the clip
    [HideInInspector] public double duration; //used in the custom inspector

    private GameObject objectInstance;
    private PathObject pathObjScript;
    private GameObject[] bullets;
    private Vector3 offset = Vector3.zero;
    private double globalClipTime; //clip time, but can be negative or past clip duration
    private bool objectIsAlive = true;
    private double objectDeadTime; //time the ship associated was killed
    private float objectSpeed; //cached because the SO might provide randomly changing speeds

    public override void OnPlayableCreate (Playable playable)
    {

    }

    //Takes care of instantiating the GameObject
    public override void OnGraphStart(Playable playable)
    {
        //create the associated prefab
        if(objectDefinition != null)
        {
            objectInstance = GameObject.Instantiate<GameObject>(objectDefinition.prefab);
            objectInstance.transform.right = Vector3.left;
            objectInstance.transform.localScale = Vector3.one * objectDefinition.sizeMultiplier;
            if(objectDefinition.randomiseRotation) objectInstance.transform.rotation = UnityEngine.Random.rotation;
            
            pathObjScript = objectInstance.GetComponent<PathObject>();
            pathObjScript.Initialize(objectDefinition);
            pathObjScript.deadEvent.AddListener(objectDeadHandler);
            
            Transform poolContainerTransform = GameObject.Find("PathObjectPool").transform;
            objectInstance.transform.SetParent(poolContainerTransform);
            objectInstance.SetActive(false);
            
            objectSpeed = objectDefinition.Speed; //cached because the SO would otherwise give us a random one each time
        }

        
		//bullet creation
        if(patternDefinition != null)
        {
            Transform poolContainerTransform = GameObject.Find("BulletPool").transform;
            int nBullets = Mathf.CeilToInt((float)duration / patternDefinition.interval);
            bullets = new GameObject[nBullets];
            for(int i=0; i<bullets.Length; i++)
            {
                bullets[i] = GameObject.Instantiate<GameObject>(patternDefinition.prefab);
                bullets[i].transform.SetParent(poolContainerTransform);
                bullets[i].SetActive(false);
            }
        }
    }

    private void objectDeadHandler()
    {
        //stop spawning bullets in MixerProcessFrame
        objectIsAlive = false;
        objectDeadTime = globalClipTime;
    }


    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        //Shows the path object
        if(objectInstance != null)
        {
            objectInstance.SetActive(true);
        }
    }

    //Hides the path object
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if(objectInstance != null)
        {
            //objectInstance.SetActive(false);
        }
    }

    public void MixerProcessFrame(Playable thisPlayable, FrameData info, object playerData, double timelineCurrentTime)
    {
        /* Calculate the clip time starting from the actual Timeline time
            the only reason why we need this is because we need it to be able to be negative or past the clip's duration,
            so we can handle bullets also after the clip ends
            thisPlayable.GetTime() only gives time constrained to the clip duration */
        globalClipTime = timelineCurrentTime - clipStart;

        //Moves the enemy ship on the path
        Transform lane = playerData as Transform;
        if(lane != null && objectInstance != null)
        {
            pathObjScript.Move(lanePosition + GetOffsetFromLaneStart(globalClipTime * objectSpeed));
        }

        if(patternDefinition != null)
        {
            //Process bullets
            for(int i = 0; i<bullets.Length; i++)
            {
                //check if the bullet was destroyed by a collision
                if(bullets[i] == null)
                    continue;

                float timeOfEmission = i * patternDefinition.interval;
                
                //Check if we actually need to move this bullet
                bool bulletWasShot = timeOfEmission < globalClipTime;
                bool shipAliveAtTimeOfShooting = objectIsAlive || timeOfEmission < objectDeadTime; //if ship is dead, then time of shot needs to be before it died
                
                if(bulletWasShot
                    && shipAliveAtTimeOfShooting)
                {
                    //bullet has been shot, so we need to move it
                    if(!bullets[i].activeSelf)
                    {
                        bullets[i].SetActive(true);
                        if(Application.isPlaying)
                        {
                            //objectAudioSource.Play(); //play shot SFX
                        }
                    }

                    float deltaTime = .02f; //(Application.isPlaying) ? Time.smoothDeltaTime : .02f;
                    Vector3 smallOffset = Vector3.left * 5f;
                    Vector3 bulletMovement = patternDefinition.direction.normalized * deltaTime * ((float)globalClipTime-timeOfEmission) * patternDefinition.speed * 100f;
                    bullets[i].transform.position = lanePosition + GetOffsetFromLaneStart(timeOfEmission * objectSpeed) + bulletMovement + smallOffset;
                }
                else
                {
                    //bullet hasn't been shot yet, it stays hidden
                    if(bullets[i].activeSelf)
                        bullets[i].SetActive(false);
                }
            }
        }
    }

    //Given a curve time, it calculates the offset from the path end
    //(the path end is the start position of the lane)
    public Vector3 GetOffsetFromLaneStart(double t)
    {
        double pathTime = 1f - t;

        offset.x = (float)(-t * xScale);
        offset.y = (path.Evaluate((float)pathTime) - path.Evaluate(1f)) * yScale;

        return offset;
    }

    public override void OnGraphStop(Playable playable)
    {
        if(objectInstance != null)
        {
            if(Application.isPlaying)
            {
                GameObject.Destroy(objectInstance);
            }
            else
            {
                GameObject.DestroyImmediate(objectInstance);
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
