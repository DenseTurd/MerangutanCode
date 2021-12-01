#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BreakingPlatformController))]
public class BreakingPlatformEditor : Editor
{
    DTGUIs guis = new DTGUIs();
    BreakingPlatformController bpc;
    public override void OnInspectorGUI()
    {
        bpc = target as BreakingPlatformController;

        EditorUtility.SetDirty(bpc.breakingPlatform);
        EditorUtility.SetDirty(bpc);

        bpc.OnValidate();

        guis.Slider("Life time", ref bpc.breakingPlatform.lifeTime);
        guis.Slider("Respawn time", ref bpc.respawnTime);

        bpc.breakingPlatform.singleUse = GUILayout.Toggle(bpc.breakingPlatform.singleUse, new GUIContent("Single use", "The platform won't respawn if checked"));
    }

    public void OnSceneGUI()
    {
        if (bpc != null)
        {
            if (bpc.breakingPlatform.home != null)
            {
                if (bpc.breakingPlatform.breakingPoint != null)
                {
                    Handles.color = Color.white;
                    Handles.DrawLine(bpc.breakingPlatform.home.position, bpc.breakingPlatform.breakingPoint.position);
                }
            }
        }
    }
}
#endif
