using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	public PlayerController player;

	private Vector2 input, inputRaw;
	private bool isShooting = false;

	private void Update()
	{
		input.x = Input.GetAxis("Horizontal");
		input.y = Input.GetAxis("Vertical");
		inputRaw.x = Input.GetAxisRaw("Horizontal");
		inputRaw.y = Input.GetAxisRaw("Vertical");
		isShooting = Input.GetButton("Fire1");

		player.Inputs(input, inputRaw, isShooting);
	}

	//When disabled by the Timeline, the InputManager will notify the player one last time with null input
	//so the player stops moving and shooting
	private void OnDisable()
	{
		player.Inputs(Vector2.zero, Vector2.zero, false);
	}
}
