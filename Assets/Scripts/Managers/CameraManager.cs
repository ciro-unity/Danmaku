using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

public class CameraManager : Singleton<CameraManager>
{
	private CinemachineImpulseSource impulseSource;

	private void Awake()
	{
		impulseSource = GetComponent<CinemachineImpulseSource>();
	}

	public void Shake(float amount = 1f)
	{
		impulseSource.GenerateImpulse(Vector3.down * amount); 
	}
}
