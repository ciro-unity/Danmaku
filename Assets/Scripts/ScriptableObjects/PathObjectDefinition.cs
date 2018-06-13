using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "PathObject", menuName = "Danmaku/Path Object Definition", order = 1)]
[System.Serializable]
public class PathObjectDefinition : ScriptableObject
{
	public GameObject prefab;
	public bool randomiseRotation = false;
	public bool randomiseSize = false;
	[SerializeField]
	private float sizeVariation = .2f;
	public float sizeMultiplier
	{
		get {
			float baseSize = 1f;
			if(randomiseSize)
			{
				if(prefab != null)
				{
					//we'll use X for all three, assuming it's uniformly scaled
					baseSize = prefab.transform.localScale.x;
				}
				return baseSize + Random.Range(-sizeVariation, sizeVariation);
			}
			else
			{
				return baseSize;
			}
		}
	}

	public int energy = 2;

	[SerializeField]
	[Range(.1f, 1f)]
	private float speed = .5f;
	public float Speed {
		get {
			if(randomiseSpeed)
			{
				return speed + Random.Range(-speedVariation, speedVariation);
			}
			else
			{
				return speed;
			}
		}
	}

	public bool randomiseSpeed = false;
	public float speedVariation = .2f;
}

#if UNITY_EDITOR

[CustomEditor(typeof(PathObjectDefinition))]
public class PathObjectDefinitionDrawer : Editor
{
	public override void OnInspectorGUI()
    {
		serializedObject.Update();
		SerializedProperty randomProp;

		EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("randomiseRotation"));
		randomProp = serializedObject.FindProperty("randomiseSize");
		EditorGUILayout.PropertyField(randomProp);
		if(randomProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeVariation"));
			EditorGUILayout.HelpBox("Size variation is relative to the Prefab's base size.", MessageType.Info);
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Space();
		
		EditorGUILayout.PropertyField(serializedObject.FindProperty("energy"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));

		randomProp = serializedObject.FindProperty("randomiseSpeed");
		EditorGUILayout.PropertyField(randomProp);
		if(randomProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("speedVariation"));
			EditorGUILayout.HelpBox("Speed variation is added and subtracted from the base speed.", MessageType.Info);
			EditorGUI.indentLevel--;
		}

		serializedObject.ApplyModifiedProperties();
	}
}

#endif