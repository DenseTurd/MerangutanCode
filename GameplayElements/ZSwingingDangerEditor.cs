#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ZSwingingDanger))]
public class ZSwingingDangerEditor : Editor
{
    DTGUIs guis = new DTGUIs();

    public override void OnInspectorGUI()
    {
        var zsd = target as ZSwingingDanger;

        EditorUtility.SetDirty(target);

        guis.Slider("Frequency", ref zsd.frequency);
        guis.Slider("Offset", ref zsd.offset);

        zsd.OnValidate();
    }
}
#endif
