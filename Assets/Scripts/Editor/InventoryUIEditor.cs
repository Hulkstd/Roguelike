using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryManager))]
public class InventoryUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if( GUILayout.Button("GetInventorySpace"))
        {
            InventoryManager manager = (InventoryManager)target;

            manager.InventorySpaces.Clear();
            for (int i = 0; i < manager.Inventories.childCount; i++)
            {
                manager.InventorySpaces.Add(manager.Inventories.GetChild(i));
            }

            manager.EquipmentSpaces.Clear();
            for (int i = 0; i < manager.Equipments.childCount; i++)
            {
                manager.EquipmentSpaces.Add(manager.Equipments.GetChild(i));
            }
        }
    }
}
