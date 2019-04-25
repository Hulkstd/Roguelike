using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalUnitHealth : MonoBehaviour
{
    [SerializeField]
    private Text UnitName;
    [SerializeField]
    private Image HealthBar;

    public void SetUnitNmae(string UnitName)
    {
        this.UnitName.text = UnitName;
    }

    public void SetHealth(int OriginalHealth, int Health)
    {
        HealthBar.fillAmount = Health / OriginalHealth;
    }
}
