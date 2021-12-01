#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaterfallPlatformController))]
public class WaterfallPlatformEditor : Editor
{
    DTGUIs guis = new DTGUIs();
    WaterfallPlatformController wpc;
    public override void OnInspectorGUI()
    {
        wpc = target as WaterfallPlatformController;

        EditorUtility.SetDirty(wpc.waterfallPlatform);

        wpc.OnValidate();

        guis.Slider(new GUIContent("Frequency", "How many times per second the platform completes a cycle"), ref wpc.waterfallPlatform.frequency);
        guis.Slider(new GUIContent("Offset", "How far through the cycle the platform starts"), ref wpc.waterfallPlatform.offset);
    }

    public void OnSceneGUI()
    {
        if (wpc != null)
        {
            if (wpc.waterfallPlatform.pointA != null)
            {
                if (wpc.waterfallPlatform.pointB != null)
                {
                    Handles.color = Color.white;
                    Handles.DrawLine(wpc.waterfallPlatform.pointA.position, wpc.waterfallPlatform.pointB.position);
                }
            }
        }
    }
}
#endif
