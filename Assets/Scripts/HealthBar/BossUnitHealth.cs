using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUnitHealth : MonoBehaviour
{
    public static BossUnitHealth Instance { get; private set; }

    [SerializeField]
    private Image HealthBar;

    public void SetHealth(int OriginalHealth, int Health)
    {
        HealthBar.fillAmount = Health / OriginalHealth;
    }
}
