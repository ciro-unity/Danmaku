using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
	private CinemachineVirtualCamera vCam;
	private CinemachineBasicMultiChannelPerlin noiseModule;
	private Coroutine shakeCo;

	private void Awake()
	{
		vCam = GetComponent<CinemachineVirtualCamera>();
		noiseModule = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}

	public void Shake()
	{
		shakeCo = StartCoroutine(ShakeCoroutine());
	}

	private IEnumerator ShakeCoroutine()
	{
		noiseModule.m_AmplitudeGain = .1f;

		yield return new WaitForSeconds(.1f);

		noiseModule.m_AmplitudeGain = 0f;
	}
}
