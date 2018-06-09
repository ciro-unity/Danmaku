using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Bullet", menuName = "Danmaku/Bullet Pattern Definition", order = 2)]
[Serializable]
public class BulletPatternDefinition : ScriptableObject
{
	public GameObject prefab;
	public float startTime = 0f;
	public float interval = 1f;
	public int maxBullets = 0;
	public float speed = 10f;
	public Vector2 direction = new Vector2(-1f, -1f);
}
