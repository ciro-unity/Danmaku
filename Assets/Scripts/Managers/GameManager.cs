using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private UIManager uiManager;

	private void Awake()
	{
		uiManager = UIManager.Instance;
	}

	public void OnPlayerHit(float energyAmount)
	{
		uiManager.OnPlayerHit(energyAmount);
	}
}
