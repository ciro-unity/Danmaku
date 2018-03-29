using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathWalkerClip))]
public class PathWalkerClipDrawer : Editor
{
    private Vector3 lanePos;
    private AnimationCurve path;

    void OnEnable()
    {
        SceneView.onSceneGUIDelegate = null;
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        lanePos = serializedObject.FindProperty("lanePosition").vector3Value;
        path = serializedObject.FindProperty("template").FindPropertyRelative("path").animationCurveValue;
    }

    void OnDisable()
    {
        SceneView.onSceneGUIDelegate = null;
    }

    // public override void OnInspectorGUI()
    // {
    //     serializedObject.Update();

    //     EditorGUILayout.PropertyField(serializedObject.FindProperty("path"));
    //     EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyDefinition"));
    //     EditorGUILayout.PropertyField(serializedObject.FindProperty("patternDefinition"));
    //     EditorGUILayout.PropertyField(serializedObject.FindProperty("xScale"));
    //     EditorGUILayout.PropertyField(serializedObject.FindProperty("yScale"));

    //     serializedObject.ApplyModifiedProperties();
    // }

    public void OnSceneGUI(SceneView sceneView)
	{
        if(path != null && path.length != 0)
        {
            PathWalkerClip c = target as PathWalkerClip;
            PathWalkerBehaviour b = c.template;
            float duration = (float)c.duration; //this is currently forced to 1

            Handles.color = Color.magenta;
            int nOfPoints = 30;
            for(int i=0; i< 30*duration; i++)
            {
                float t = (float)i * 1f/nOfPoints;
                Handles.SphereHandleCap(0, lanePos + b.GetOffsetFromPathEnd(t), Quaternion.identity, .5f, EventType.Repaint);
                Handles.DrawDottedLine(lanePos + b.GetOffsetFromPathEnd(t),
                                        lanePos + b.GetOffsetFromPathEnd(t + duration * 1f/nOfPoints),
                                        3f);
            }
        }
    }
}