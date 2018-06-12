using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private UIManager uiManager;
	private int hits = 0;

	private void Awake()
	{
		uiManager = UIManager.Instance;
	}

	public void OnPlayerHit(float energyAmount)
	{
		uiManager.OnPlayerHit(Mathf.Max(0f, energyAmount));
	}

	public void OnEnemyDown()
	{
		hits++;

		uiManager.OnEnemyDown(hits);
	}
}
