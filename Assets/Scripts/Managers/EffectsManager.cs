using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : Singleton<EffectsManager>
{
	public GameObject explosionPrefab;
	
	private ParticleSystem[] explosionPool;
	private AudioSource audioSource;

	private void Start()
	{
		//instantiate explosions pool
		Transform poolContainerTransform = GameObject.Find("EffectsPool").transform;
		explosionPool = new ParticleSystem[40];
		GameObject newExplosion;
		for(int i=0; i<explosionPool.Length; i++)
		{
			newExplosion = Instantiate<GameObject>(explosionPrefab);
			newExplosion.transform.SetParent(poolContainerTransform);
			
			explosionPool[i] = newExplosion.GetComponent<ParticleSystem>();
			explosionPool[i].Stop();
		}
	}

	public void PlayExplosion(Vector3 positionInSpace)
	{
		bool found = false;
		for(int i=0; i<explosionPool.Length; i++)
		{
			//find an inactive bullet
			if(!explosionPool[i].isPlaying)
			{
				explosionPool[i].transform.position = positionInSpace;
				explosionPool[i].Play();
				
				audioSource = explosionPool[i].GetComponent<AudioSource>();
				audioSource.pitch = Random.Range(.9f, 1.1f);
				audioSource.Play();
				
				found = true;
				break;
			}
		}

		if(!found)
			Debug.LogError("Wasn't able to find an explosion to play. Maybe make the pool bigger?");
	}
}
