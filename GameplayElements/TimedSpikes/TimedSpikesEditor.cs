#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TimedSpikesController))]
public class TImedSpikesEditor : Editor
{
    DTGUIs guis = new DTGUIs();
    TimedSpikesController tsc;
    public override void OnInspectorGUI()
    {
        tsc = target as TimedSpikesController;

        EditorUtility.SetDirty(tsc.timedSpikes);

        tsc.OnValidate();

        GUILayout.Label(new GUIContent("Extended position...",  "is wherever you place the spikes"));

        guis.Slider(new GUIContent("Spike time", "Amount of time the spikes are extended"), ref tsc.timedSpikes.spikeTime);
        guis.Slider(new GUIContent("Safe time", "Amount of time the spikes are retracted"), ref tsc.timedSpikes.safeTime);
    }

    public void OnSceneGUI()
    {
        if (tsc != null)
        {
            if (tsc.timedSpikes != null)
            {
                Handles.color = Color.white;
                Handles.DrawLine(tsc.timedSpikes.transform.position, tsc.timedSpikes.retractedPos.position);
            }
        }
    }
}
#endif
