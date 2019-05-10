using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityInfoStorage : MonoBehaviour
{
    public AbilityName Ability;

    [HideInInspector]
    public AbilityInfo AbilityInfo { get { return AbilityDatabase.Instance.AbilityInfos[(int)Ability]; } }
    [HideInInspector]
    public UnityEngine.UI.Text AbilityName;
    [HideInInspector]
    public UnityEngine.UI.Text AbilityDescription;
    [HideInInspector]
    public UnityEngine.UI.Button Yes;
    [HideInInspector]
    public UnityEngine.UI.Button No;
    [HideInInspector]
    public RectTransform StatsPanel;
    [HideInInspector]
    public UnityEngine.UI.Text Stats;

    private void Start()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = AbilityInfo.AbilityImage;

        AbilityName = transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();
        AbilityName.text = AbilityInfo.AbilityName;

        AbilityDescription = transform.GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Text>();
        AbilityDescription.text = AbilityInfo.AbilityDescription;

        //Yes = transform.GetChild(0).GetChild(2).GetComponent<UnityEngine.UI.Button>();
        //Yes.onClick.AddListener(() =>
        //{
        //    AbilityManager.Instance.SelectNode(gameObject);
        //});

        //No = transform.GetChild(0).GetChild(3).GetComponent<UnityEngine.UI.Button>();
        //No.onClick.AddListener(() =>
        //{
        //    transform.GetChild(0).gameObject.SetActive(false);
        //});

        StatsPanel = transform.GetChild(0).GetChild(4).GetComponent<RectTransform>();
        StatsPanel.offsetMin = new Vector2(StatsPanel.offsetMin.x, 150 - AbilityInfo.Stats.Count * 25);

        transform.GetChild(0).GetChild(5).GetComponent<UnityEngine.UI.Image>().sprite = AbilityInfo.AbilityImage;

        Stats = StatsPanel.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        string str = "";
        foreach (Stat stat in AbilityInfo.Stats)
        {
            if(stat.GetStat(StatType.HP) != 0)
            {
                str +=  "HP : " + stat.GetStat(StatType.HP) + '\n';
            }
            else if(stat.GetStat(StatType.MP) != 0)
            {
                str += "MP : " + stat.GetStat(StatType.MP) + '\n';
            }
            else if (stat.GetStat(StatType.Speed) != 0)
            {
                str += "SP : " + stat.GetStat(StatType.Speed) + '\n';
            }
            else if (stat.GetStat(StatType.Power) != 0)
            {
                str += "PO : " + stat.GetStat(StatType.Power) + '\n';
            }
            else if (stat.GetStat(StatType.CriticalPercent) != 0)
            {
                str += "CP : " + stat.GetStat(StatType.CriticalPercent) + '\n';
            }
            else if (stat.GetStat(StatType.CriticalDamage) != 0)
            {
                str += "CD : " + stat.GetStat(StatType.CriticalDamage) + '\n';
            }
        }
        Stats.text = str;
    }
}
