using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public AudioClip hit;
	public AudioSource audioSource;

	public void PlaySfx()
	{
		audioSource.PlayOneShot(hit, 1f);
	}
}
