using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathWalkerClip))]
public class PathWalkerClipDrawer : Editor
{
    private Vector3 lanePos;

    void OnEnable()
    {
        SceneView.onSceneGUIDelegate = null;
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        lanePos = serializedObject.FindProperty("lanePosition").vector3Value;
     }
  
     void OnDisable()
     {
        SceneView.onSceneGUIDelegate = null;
     }

    public void OnSceneGUI(SceneView sceneView)
	{
        PathWalkerClip c = target as PathWalkerClip;
        PathWalkerBehaviour b = c.template;
        float duration = (float)c.duration;

        Debug.Log(duration);

        return;
        Handles.color = Color.magenta;
        for(int i=0; i<10*duration; i++)
        {
            float t = (float)i * .1f;
            Handles.SphereHandleCap(0, lanePos + b.GetOffsetFromPathEnd(t), Quaternion.identity, 1f, EventType.Repaint);
            Handles.DrawDottedLine(lanePos + b.GetOffsetFromPathEnd(t),
                                    lanePos + b.GetOffsetFromPathEnd(t + duration * .1f),
                                    3f);
        }
    }
}