using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathWalkerClip))]
public class PathWalkerClipDrawer : Editor
{
    private Vector3 lanePos;
    private AnimationCurve path;
    private float duration;

    void OnEnable()
    {
        if(Selection.activeObject != null) //little hack to avoid Scene flashing issue on Editor Stop
        {
            SceneView.onSceneGUIDelegate = null;
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;

            lanePos = serializedObject.FindProperty("lanePosition").vector3Value;
            path = serializedObject.FindProperty("template").FindPropertyRelative("path").animationCurveValue;
            duration = (float)serializedObject.FindProperty("realDuration").doubleValue;
        }
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

            Handles.color = Color.magenta;
            float nOfPoints = 10f * duration;
            for(int i=0; i< nOfPoints; i++)
            {
                float t = i / 10f;//= (float)i * .1f * nOfPoints;
                float t1 = (i+1) / 10f;
                //Debug.Log("Point: " + i + " has t=" + t + " and t1=" + t1);
                Handles.SphereHandleCap(0, lanePos + b.GetOffsetFromPathEnd(t), Quaternion.identity, .5f, EventType.Repaint);
                Handles.DrawDottedLine(lanePos + b.GetOffsetFromPathEnd(t),
                                        lanePos + b.GetOffsetFromPathEnd(t1),
                                        3f);
            }
        }
    }
}