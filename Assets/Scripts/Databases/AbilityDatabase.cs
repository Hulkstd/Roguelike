using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AbilityDatabase : MonoBehaviour
{
    public static AbilityDatabase Instance { get; private set; }

    [SerializeField]
    public List<AbilityInfo> AbilityInfos;

    private void Awake()
    {
        Instance = this;
    }

    public void BuildDatabase()
    {
        AbilityInfos = JsonConvert.DeserializeObject<List<AbilityInfo>>(Resources.Load<TextAsset>("Json/Ability").ToString());
    }
}

[System.Serializable]
[SerializeField]
public class AbilityInfo
{
    public string AbilityName;
    public string AbilityDescription;
    public List<Stat> Stats;
    public Sprite AbilityImage;

    public AbilityInfo(string AbilityName, string AbilityDescription, List<Stat> Stats, string AbilityImage)
    {
        this.AbilityName = AbilityName;
        this.AbilityDescription = AbilityDescription;
        this.Stats = Stats;
        this.AbilityImage = Resources.Load<Sprite>(AbilityImage);
    }

    public int TryGetStat(string Stat)
    {
        int value = 0;

        foreach (Stat obj in Stats)
        {
            value = value < obj.GetStat(Stat) ? obj.GetStat(Stat) : value;
        }

        return value;
    }
}