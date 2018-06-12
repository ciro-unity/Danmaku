using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathObject : MonoBehaviour
{
	[HideInInspector] public int energy = 1; //overwritten by the Timeline, picking it up from EnemyDefinition SO
	[HideInInspector] public UnityEvent deadEvent;
	
	private CameraManager cameraManager;
	private Animator animator;
    private AudioSource objectAudioSource;
    private int horizontalHash, verticalHash;
    private Vector3 moveDifference;
    private float previousHorDiff, previousVertDiff;
    private Vector3 previousObjectPosition;
    private GameObject[] bullets;
	private bool hasAnimator = false;

	private void Awake()
	{
		cameraManager = CameraManager.Instance;
	}

	private void Start()
	{
		previousObjectPosition = transform.position;
	}

	public void Initialize(PathObjectDefinition pathObjDef)
	{
		energy = pathObjDef.energy;
		
		animator = GetComponent<Animator>();
		if(animator != null)
		{
			hasAnimator = true;
		}
        //hashing the strings into ints for performance
        horizontalHash = Animator.StringToHash("HorizontalMovement");
        verticalHash = Animator.StringToHash("VerticalMovement");
	}

	//Called by the Timeline or other scripts
	public void Move(Vector3 newPosition)
	{
		transform.position = newPosition;

		//Updating the Animator to match state
		if(hasAnimator
			&& animator.playableGraph.IsPlaying())
		{
			moveDifference = (transform.position - previousObjectPosition) * 2f;
			float step = Time.deltaTime * 2f;
			float newAnimatorX = Mathf.Lerp(previousHorDiff, moveDifference.x, step);
			float newAnimatorY = Mathf.Lerp(previousVertDiff, moveDifference.y, step);
			animator.SetFloat(horizontalHash, newAnimatorX);
			animator.SetFloat(verticalHash, newAnimatorY);
			previousHorDiff = newAnimatorX;
			previousVertDiff = newAnimatorY;

			previousObjectPosition = transform.position;
		}
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		GameObject otherObject = coll.gameObject;
		if(otherObject.CompareTag("PlayerBullet"))
		{
			energy --;
			otherObject.SetActive(false); //remove the bullet after collision, but player bullets are just disabled
			//CameraManager.Instance.Shake(1f);

			if(energy == 0)
			{
				Die();
			}
		}
	}

	private void Die()
	{
		CameraManager.Instance.Shake(2f);
		EffectsManager.Instance.PlayExplosion(transform.position);
		GameManager.Instance.OnEnemyDown();
		deadEvent.Invoke(); //this will notify the Timeline ClipBehaviour, so it stops spawning bullets
		Destroy(this.gameObject);
	}
}
