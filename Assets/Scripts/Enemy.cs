using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
	[HideInInspector] public int energy = 1; //overwritten by the Timeline, picking it up from EnemyDefinition SO

	public UnityEvent deadEvent;

	private void OnCollisionEnter2D(Collision2D coll)
	{
		GameObject otherObject = coll.gameObject;
		if(otherObject.CompareTag("PlayerBullet"))
		{
			energy --;

			if(energy == 0)
			{
				Die();
			}
		}
	}

	private void Die()
	{
		EffectsManager.Instance.PlayExplosion(transform.position);
		deadEvent.Invoke(); //this will notify the Timeline ClipBehaviour, so it stops spawning bullets
		Destroy(this.gameObject);
	}
}
