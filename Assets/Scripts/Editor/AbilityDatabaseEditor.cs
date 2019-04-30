using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityDatabase))]
public class AbilityDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("BuildDatabase"))
        {
            AbilityDatabase database = (AbilityDatabase)target;

            database.BuildDatabase();
        }
    }
}
