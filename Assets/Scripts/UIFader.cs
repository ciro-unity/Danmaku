using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour
{
	public CanvasGroup groupToControl;

	private void OnTriggerEnter2D(Collider2D coll)
	{
		groupToControl.alpha = .05f;
	}

	private void OnTriggerExit2D(Collider2D coll)
	{
		groupToControl.alpha = 1f;
	}
}
