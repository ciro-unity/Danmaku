using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour {
	private Material skyboxMaterial;
	private float skyboxRotation = 0f;
	public float rotationSpeed = 10f;

	void Start ()
	{
		//we clone the material so we don't actually modify the asset
		skyboxMaterial = new Material(RenderSettings.skybox);
		skyboxMaterial.name = "Runtime Skybox";
		RenderSettings.skybox = skyboxMaterial;
	}
	
	void Update ()
	{
		skyboxRotation = Mathf.Repeat(skyboxRotation + Time.deltaTime * rotationSpeed, 360f);
		skyboxMaterial.SetFloat("_Rotation", skyboxRotation);
	}
}
