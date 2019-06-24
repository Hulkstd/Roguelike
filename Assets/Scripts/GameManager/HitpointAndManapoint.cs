using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("GameManager/HitpointAndManapoint")]
public class HitpointAndManapoint : MonoBehaviour
{
    public static HitpointAndManapoint Instance { get; private set; }

    [SerializeField]
    private Image HPImage;
    [SerializeField]
    private Image MPImage;

    public int MaxMP = 1;
    public int MaxHP = 1;

    void Awake()
    {
        Instance = this;
    }

    public void SetManapoint(int NowMP)
    {
        MPImage.fillAmount = (float)MaxMP / NowMP;
    }

    public void SetHitpoint(int NowHP)
    {
        HPImage.fillAmount = (float)MaxHP / NowHP;
    }
}
