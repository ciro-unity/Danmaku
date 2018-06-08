using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PortraitDisplayer : MonoBehaviour
{
	public CinemachineVirtualCamera[] characterCameras;

	private int currentlyActiveCamera = 0;

	//Bring the camera associated with the new character to top priority
	public void DisplayCharacter(GameCharacters characterToDisplay)
	{
		characterCameras[currentlyActiveCamera].Priority = 0;
		characterCameras[(int)characterToDisplay].Priority = 10;
	}
}
