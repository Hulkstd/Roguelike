using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    public int HPStat = 0;
    public int MPStat = 0;
    public int SpeedStat = 0;
    public int PowerStat = 0;
    public int CriticalPercent = 0;
    public int CriticalDamage = 0;
    public int RemainingStats = 0;

    public int StatperHP = 3;
    public int StatperMP = 2;
    public float StatperSpeed = 0.01f;
    public float StatperPower = 0.5f;
    public float StatperCriticalPercent = 0.5f;
    public float StatperCriticalDamage = 0.05f;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Image AskApply;
    [SerializeField]
    private Text HPDesc;
    [SerializeField]
    private Text HPCount;
    [SerializeField]
    private Text MPDesc;
    [SerializeField]
    private Text MPCount;
    [SerializeField]
    private Text SpeedDesc;
    [SerializeField]
    private Text SpeedCount;
    [SerializeField]
    private Text PowerDesc;
    [SerializeField]
    private Text PowerCount;
    [SerializeField]
    private Text RemainingStatsText;
    private int[] UpgradeStat = new int[4];

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        HPDesc.text = ("HP + " + ((HPStat + UpgradeStat[0]) * StatperHP).ToString());
        MPDesc.text = ("MP + " + ((MPStat + UpgradeStat[1]) * StatperMP).ToString());
        SpeedDesc.text = ("SPEED + " + ((SpeedStat + UpgradeStat[2]) * StatperSpeed).ToString());
        PowerDesc.text = ("POWER + " + ((PowerStat + UpgradeStat[3]) * StatperPower).ToString());

        HPCount.text = "+ " + UpgradeStat[0];
        MPCount.text = "+ " + UpgradeStat[1];
        SpeedCount.text = "+ " + UpgradeStat[2];
        PowerCount.text = "+ " + UpgradeStat[3];
        RemainingStatsText.text = "Remaining Stats : " + RemainingStats;
    }

    public void ShowWindow()
    {
        animator.SetBool("Open StatWindow", true);
    }

    public void PlusStat(string Stat)
    {
        if (RemainingStats == 0)
            return;

        switch(Stat)
        {
            case "HP":
                {
                    UpgradeStat[0]++;
                }
                break;

            case "MP":
                {
                    UpgradeStat[1]++;
                }
                break;

            case "Speed":
                {
                    UpgradeStat[2]++;
                }
                break;

            case "Power":
                {
                    UpgradeStat[3]++;
                }
                break;
        }

        RemainingStats--;
    }

    public void MinusStat(string Stat)
    {
        if (RemainingStats == 0)
            return;

        switch (Stat)
        {
            case "HP":
                {
                    if(UpgradeStat[0] == 0)
                    {
                        return;
                    }

                    UpgradeStat[0]--;
                }
                break;

            case "MP":
                {
                    if (UpgradeStat[1] == 0)
                    {
                        return;
                    }

                    UpgradeStat[1]--;
                }
                break;

            case "Speed":
                {
                    if (UpgradeStat[2] == 0)
                    {
                        return;
                    }

                    UpgradeStat[2]--;
                }
                break;

            case "Power":
                {
                    if (UpgradeStat[3] == 0)
                    {
                        return;
                    }

                    UpgradeStat[3]--;
                }
                break;
        }

        RemainingStats++;
    }

    public void Apply()
    {
        AskApply.gameObject.SetActive(true);
    }

    public void AskApplyYes()
    {
        animator.SetBool("Open StatWindow", false);

        HPStat += UpgradeStat[0];
        MPStat += UpgradeStat[1];
        SpeedStat += UpgradeStat[2];
        PowerStat += UpgradeStat[3];
        UpgradeStat[0] = UpgradeStat[1] = UpgradeStat[2] = UpgradeStat[3] = 0;
        AskApply.gameObject.SetActive(false);
    }

    public void AskApplyNo()
    {
        AskApply.gameObject.SetActive(false);
    }
}
