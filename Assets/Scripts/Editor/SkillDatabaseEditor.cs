using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillDatabase))]
public class SkillDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("BuildDatabase"))
        {
            SkillDatabase database = (SkillDatabase)target;

            database.BuildDatabase();
        }
    }
}
