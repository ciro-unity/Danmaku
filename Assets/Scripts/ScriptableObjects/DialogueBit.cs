using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DialogueBit", menuName = "Danmaku/Dialogue Bit", order = 0)]
[Serializable]
public class DialogueBit : ScriptableObject
{
	public GameCharacters character;
	public string line;

	public string characterName
	{
		get {
			switch (character)
			{
				case GameCharacters.Kyle:
					return "Kyle";
				case GameCharacters.PunkGirl:
					return "Rebecca";
				default:
					return "Unnamed character";
			};
		}
	}
}

//Characters appear in this enum in the order they appear in the scene, left to right
public enum GameCharacters
{
	Kyle,
	PunkGirl,
}