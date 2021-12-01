#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Section))]
public class SectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var section = target as Section;

        EditorUtility.SetDirty(target);

        SerializedObject so = new SerializedObject(target);

        section.coins = GUILayout.Toggle(section.coins, new GUIContent("Coins", "Provide parent objects containing coins, each has a 33% chance of remaining active when the section is instantiated"));
        if (section.coins)
        {
            SerializedProperty coinRents = so.FindProperty("coinParents");
            EditorGUILayout.PropertyField(coinRents, true);
        }

        section.enemies = GUILayout.Toggle(section.enemies, new GUIContent("Enemies", "Provide locations for the EnemyGenerator to spawn enemies"));
        if (section.enemies)
        {
            SerializedProperty enemyLocs = so.FindProperty("enemySpawnLocations");
            EditorGUILayout.PropertyField(enemyLocs, true);
        }

        section.spawnables = GUILayout.Toggle(section.spawnables, new GUIContent("Spawnables", "Provide a list of spawnables and locations to have this section randomly spawn them"));
        if (section.spawnables)
        {
            SerializedProperty spawnsAndLocs = so.FindProperty("spawnsAndLocations");
            EditorGUILayout.PropertyField(spawnsAndLocs, true);
        }

        section.surprise = GUILayout.Toggle(section.surprise, new GUIContent("Surprise!", "Provide a list of spawnables a spawn location and a triggerer to setup a surprise"));
        if (section.surprise)
        {
            SerializedProperty surpriseAndLoc = so.FindProperty("surprisesAndLocation");
            EditorGUILayout.PropertyField(surpriseAndLoc, true);
            SerializedProperty trigr = so.FindProperty("triggerer");
            EditorGUILayout.PropertyField(trigr, true);
        }

        

        section.checkPointSection = GUILayout.Toggle(section.checkPoint, new GUIContent("CheckPoint section", "Provide CheckPoint object and location for this section to function as a checkpoint"));
        if (section.checkPointSection)
        {
            SerializedProperty chekPnt = so.FindProperty("checkPoint");
            EditorGUILayout.PropertyField(chekPnt, true);
            SerializedProperty chekLoc = so.FindProperty("checkPointLocation");
            EditorGUILayout.PropertyField(chekLoc, true);
        }

        section.levelEndSection = GUILayout.Toggle(section.levelEndSection, new GUIContent("Level end section", "Provide LevelEnd object and location for this section to function as a level end"));
        if (section.levelEndSection)
        {
            SerializedProperty levEnd = so.FindProperty("levelEnd");
            EditorGUILayout.PropertyField(levEnd, true);
            SerializedProperty levEndLoc = so.FindProperty("levelEndSpawnLocation");
            EditorGUILayout.PropertyField(levEndLoc, true);
        }

        so.ApplyModifiedProperties();
    }
}
#endif
