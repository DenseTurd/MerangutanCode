#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrbitalPlatformController))]
public class OrbitalPlatformControllerEditor : Editor
{
    DTGUIs guis = new DTGUIs();
    OrbitalPlatformController opc;
    public override void OnInspectorGUI()
    {
        opc = target as OrbitalPlatformController;

        EditorUtility.SetDirty(opc.orbitalPlatform);

        opc.OnValidate();

        guis.Slider(new GUIContent("Frequency", "How many times per second the platform completes an orbit"), ref opc.orbitalPlatform.frequency);
        guis.Slider(new GUIContent("Radius", "How large the orbit is"), ref opc.orbitalPlatform.radius);
        guis.Slider(new GUIContent("Starting angle", "Where in the orbit the platform starts"), ref opc.orbitalPlatform.startingAngle);

        opc.orbitalPlatform.clockwise = GUILayout.Toggle(opc.orbitalPlatform.clockwise, "Clockwise");
    }

    public void OnSceneGUI()
    {
        if (opc != null)
        {
            if (opc.orbitalPlatform.centre != null)
            {
                Handles.color = Color.white;
                Handles.DrawWireDisc(opc.orbitalPlatform.centre.position, opc.orbitalPlatform.centre.forward * -1, opc.orbitalPlatform.radius.x);
            }
        }
    }
}
#endif
