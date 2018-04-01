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

	private const float RIGH_BOUNDARY = 200f;

	private Transform[] bulletPool;
	
	private new Rigidbody2D rigidbody2D;
	private Animator animator;
	private Vector2 input, inputRaw;
	private Vector2 speedOperations;
	private bool isShooting = false;
	private float lastBulletTime = -1f;

	void Start ()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		//instantiate bullet pool
		bulletPool = new Transform[100];
		GameObject newBullet;
		for(int i=0; i<bulletPool.Length; i++)
		{
			newBullet = Instantiate<GameObject>(bulletPrefab);
			newBullet.SetActive(false);
			bulletPool[i] = newBullet.transform;
		}
	}
	
	void Update ()
	{
		input.x = Input.GetAxis("Horizontal");
		input.y = Input.GetAxis("Vertical");
		inputRaw.x = Input.GetAxisRaw("Horizontal");
		inputRaw.y = Input.GetAxisRaw("Vertical");
		isShooting = Input.GetButton("Fire1");

		animator.SetFloat("HorizontalMovement", input.x);
		animator.SetFloat("VerticalMovement", input.y);
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
		for(int i=0; i<bulletPool.Length; i++)
		{
			//find an inactive bullet
			if(!bulletPool[i].gameObject.activeSelf)
			{
				bulletPool[i].position = transform.position + new Vector3(3f, -2.3f, 0f);
				bulletPool[i].gameObject.SetActive(true);

				break;
			}
		}
	}

	private void FixedUpdate()
	{
		rigidbody2D.AddForce(input * speed * 100f, ForceMode2D.Force);

		speedOperations = rigidbody2D.velocity;

		//cap speed
		if(speedOperations.sqrMagnitude > maxSpeed * maxSpeed)
		{
			speedOperations.Normalize();
			speedOperations *= maxSpeed;
		}

		if(inputRaw.sqrMagnitude < .02f)
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
			Destroy(otherBody);
			energy --;
		}
	}
}
