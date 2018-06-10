using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed, maxSpeed; //5, 100
	public float bulletSpeed; //10
	public float bulletInterval; //.1
	public int energy; //50
	public GameObject bulletPrefab;

	private AudioSource audioSource;

	private const float RIGH_BOUNDARY = 200f;

	private Transform[] bulletPool;
	private CameraManager cameraManager;
	private AudioManager audioManager;
	private new Rigidbody2D rigidbody2D;
	private Animator animator;
	private Vector2 movement, rawMovement;
	private Vector2 speedOperations;
	private bool isShooting = false;
	private float lastBulletTime = -1f;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		cameraManager = CameraManager.Instance;
		audioManager = AudioManager.Instance;
	}

	void Start ()
	{
		//instantiate bullet pool
		Transform poolContainerTransform = GameObject.Find("BulletPool").transform;
		bulletPool = new Transform[20];
		GameObject newBullet;
		for(int i=0; i<bulletPool.Length; i++)
		{
			newBullet = Instantiate<GameObject>(bulletPrefab);
			newBullet.SetActive(false);
			newBullet.transform.SetParent(poolContainerTransform);
			bulletPool[i] = newBullet.transform;
		}
	}

	//called by the InputManager or Timeline
	public void Inputs(Vector2 moveInput, Vector2 rawMoveInput, bool shootInput)
	{
		movement = moveInput;
		rawMovement = rawMoveInput;
		isShooting = shootInput;
	}
	
	void Update ()
	{
		animator.SetFloat("HorizontalMovement", movement.x);
		animator.SetFloat("VerticalMovement", movement.y);
		animator.SetBool("IsShooting", isShooting);

		if(isShooting
		&& Time.time >= lastBulletTime + bulletInterval)
		{
			ShootBullet();
			lastBulletTime = Time.time;
		}

		MoveAllBullets();
	}

	private void MoveAllBullets()
	{
		//bullet moving pattern
		for(int i=0; i<bulletPool.Length; i++)
		{
			if(bulletPool[i].gameObject.activeSelf)
			{
				bulletPool[i].Translate(Vector3.right * bulletSpeed * Time.deltaTime);

				if(bulletPool[i].position.x > RIGH_BOUNDARY)
				{
					//bullet is off-screen, deactivate
					bulletPool[i].gameObject.SetActive(false);
				}
			}
		}
	}

	private void ShootBullet()
	{
		bool found = false;
		for(int i=0; i<bulletPool.Length; i++)
		{
			//find an inactive bullet
			if(!bulletPool[i].gameObject.activeSelf)
			{
				bulletPool[i].position = transform.position + new Vector3(3f, -2.3f, 0f);
				bulletPool[i].gameObject.SetActive(true);
				found = true;
				break;
			}
		}

		audioSource.pitch = Random.Range(.9f, 1.1f);
		audioSource.Play();

		if(!found)
			Debug.LogError("Wasn't able to find a free bullet. Maybe make the pool bigger?");
	}

	private void FixedUpdate()
	{
		rigidbody2D.AddForce(movement * speed * 100f, ForceMode2D.Force);

		speedOperations = rigidbody2D.velocity;

		//cap speed
		if(speedOperations.sqrMagnitude > maxSpeed * maxSpeed)
		{
			speedOperations.Normalize();
			speedOperations *= maxSpeed;
		}

		if(rawMovement.sqrMagnitude < .02f)
		{
			speedOperations *= .9f; //slow down
		}

		rigidbody2D.velocity = speedOperations;
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		GameObject otherBody = coll.gameObject;
		if(otherBody.CompareTag("EnemyBullet"))
		{
			Destroy(otherBody); //remove the bullet after collision
			energy --;
			cameraManager.Shake(1f);
			audioManager.PlaySfx();
		}
	}
}
