using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Enemy", menuName = "Danmaku/Enemy Definition", order = 1)]
[Serializable]
public class EnemyDefinition : ScriptableObject
{
	public GameObject prefab;
	public Color tintColor = Color.white;

	public float speed = 10f;
	public float life = 2f;
}
