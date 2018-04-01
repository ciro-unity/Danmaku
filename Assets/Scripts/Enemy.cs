using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[HideInInspector] public int energy = 1; //overwritten by the Timeline, picking it up from EnemyDefinition SO

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
		Destroy(this.gameObject);
	}
}
