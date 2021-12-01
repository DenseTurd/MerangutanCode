#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LinearPlatformController))]
public class LinearPlatformEditor : Editor
{
    DTGUIs guis = new DTGUIs();
    LinearPlatformController lpc;
    public override void OnInspectorGUI()
    {
        lpc = target as LinearPlatformController;

        EditorUtility.SetDirty(lpc.linearPlatform);

        lpc.OnValidate();

        guis.Slider(new GUIContent("Frequency", "How many times per second the platform completes a cycle"), ref lpc.linearPlatform.frequency);
        guis.Slider(new GUIContent("Offset", "How far through the cycle the platform starts"), ref lpc.linearPlatform.offset);
    }

    public void OnSceneGUI()
    {
        if (lpc != null)
        {
            if (lpc.linearPlatform.pointA != null)
            {
                if (lpc.linearPlatform.pointB != null)
                {
                    Handles.color = Color.white;
                    Handles.DrawLine(lpc.linearPlatform.pointA.position, lpc.linearPlatform.pointB.position);
                }
            }
        }
    }
}
#endif
