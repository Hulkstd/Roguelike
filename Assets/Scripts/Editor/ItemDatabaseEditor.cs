using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("BuildDatabase"))
        {
            ItemDatabase database = (ItemDatabase)target;
            
            database.BuildDatabase();
        }
    }
}
