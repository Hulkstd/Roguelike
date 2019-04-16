using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatInfo : MonoBehaviour
{
    private void OnEnable()
    {
        InventoryManager.Instance.ApplyStatWindow();
    }
}
