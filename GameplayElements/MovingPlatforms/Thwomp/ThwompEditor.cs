#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThwompController))]
public class ThwompEditor : Editor
{
    DTGUIs guis = new DTGUIs();
    ThwompController tc;
    public override void OnInspectorGUI()
    {
        tc = target as ThwompController;

        EditorUtility.SetDirty(tc.thwomp);

        tc.OnValidate();

        guis.Slider("Speed", ref tc.thwomp.frequency);
        guis.Slider(new GUIContent("Windup", "Time from merangutan entering the activation zone until the attack starts"), ref tc.thwomp.windupTime);
        guis.Slider(new GUIContent("Recovery", "Time thwomp will remain at the target after attacking"), ref tc.thwomp.recoverTime);
        guis.Slider(new GUIContent("Cooldown", "Time before windup can begin after thwomp has returned home"), ref tc.thwomp.cooldown);
    }

    public void OnSceneGUI()
    {
        if (tc != null)
        {
            if (tc.thwomp.home != null)
            {
                if (tc.thwomp.target != null)
                {
                    Handles.color = Color.white;
                    Handles.DrawLine(tc.thwomp.home.position, tc.thwomp.target.position);
                }
            }
        }
    }
}
#endif
