using UnityEditor;
using UnityEngine;
/* 
[CustomPropertyDrawer(typeof(PathWalkerBehaviour))]
public class PathWalkerDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.PropertyField(property.FindPropertyRelative("path"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("enemyDefinition"));
    } 

    private void OnSceneGUI(SceneView v)
    {

    }
}

[CustomEditor(typeof(PathWalkerClip))]
public class PathWalkerClipInspector : Editor
{
    private void OnEnable()
	{
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

    private void OnSceneGUI(SceneView v)
	{
        for(int i=0; i<10; i++)
            {
                float f = i/10f;
                //Vector3 spherePos = lane.position + new Vector3(-f * xScale, 0f, 0f);
                Handles.DrawWireCube(Vector3.zero, Vector3.one);
            }
    }
}*/