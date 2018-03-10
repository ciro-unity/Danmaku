﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 5f, maxSpeed = 100f;
	
	private new Rigidbody2D rigidbody2D;
	private Vector2 input, inputRaw;
	private Vector2 speedOperations;

	void Start ()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
		input.x = Input.GetAxis("Horizontal");
		input.y = Input.GetAxis("Vertical");
		inputRaw.x = Input.GetAxisRaw("Horizontal");
		inputRaw.y = Input.GetAxisRaw("Vertical");
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
}
