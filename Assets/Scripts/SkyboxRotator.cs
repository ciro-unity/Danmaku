using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour {
	private Material skyboxMaterial;
	private float skyboxRotation = 0f;
	public float rotationSpeed = 10f;

	void Start ()
	{
		skyboxMaterial = RenderSettings.skybox;
	}
	
	void Update ()
	{
		skyboxRotation = Mathf.Repeat(skyboxRotation + Time.deltaTime * rotationSpeed, 360f);
		skyboxMaterial.SetFloat("_Rotation", skyboxRotation);
	}
}
