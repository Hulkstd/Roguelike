using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StstInfo : MonoBehaviour
{
    private void OnEnable()
    {
        InventoryManager.Instance.ApplyStatWindow();
    }
}
