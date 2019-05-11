using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public EquipmentItemNumber EquipmentItem;
    public EquipmentsObject EquipData;

    void Start()
    {
        EquipData = ItemDatabase.Instance.Equipments[(int)EquipmentItem];
    }
}
