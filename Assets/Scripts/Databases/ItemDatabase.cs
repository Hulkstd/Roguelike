﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    [SerializeField]
    public List<EquipmentsObject> Equipments;

    private void Awake()
    {
        Instance = this;
    }

    public void BuildDatabase()
    {
        Equipments = JsonConvert.DeserializeObject<List<EquipmentsObject>>(Resources.Load<TextAsset>("Json/Equipments").ToString());
    }
}

[System.Serializable]
public class EquipmentsObject
{
    public string EquipName;
    public string EquipDescription;
    public Sprite EquipImage;
    public Rating EquipRating;
    public string EquipType;

    #region Weapon
    public Artifact Artifact;
    public WeaponType WeaponType;
    public float Weight;
    public float AttackSpeed;
    #endregion
    #region Suit
    public float Speed;
    #endregion
    #region Accessories

    #endregion

    public List<Stat> Stats;

    [JsonConstructor]
    public EquipmentsObject(string EquipName, string EquipDescription, string EquipImagePath, string EquipRating, string EquipType, int Artifact, int WeaponType, float Weight, float AttackSpeed, float Speed, List<Stat> Stats)
    {
        this.EquipName = EquipName;
        this.EquipDescription = EquipDescription;
        this.EquipImage = Resources.Load<Sprite>(EquipImagePath);
        this.EquipRating = (EquipRating == "Common" ? Rating.Common : (EquipRating == "Rare" ? Rating.Rare : Rating.Unique));
        this.EquipType = EquipType;
        this.Artifact = (Artifact)Artifact;
        this.WeaponType = (WeaponType)WeaponType;
        this.Weight = Weight;
        this.AttackSpeed = AttackSpeed;
        this.Speed = Speed;
        this.Stats = Stats;
    }

    public int TryGetStat(StatType Stat)
    {
        int value = 0;

        foreach (Stat obj in Stats)
        {
            value = value < obj.GetStat(Stat) ? obj.GetStat(Stat) : value;
        }

        return value;
    }
}

[System.Serializable]
[SerializeField]
public class Stat
{
    public Stat(string Name, int Amount)
    {
        switch(Name)
        {
            case "HP":
                {
                    HP = Amount;
                }
                break;

            case "MP":
                {
                    MP = Amount;
                }
                break;

            case "Speed":
                {
                    Speed = Amount;
                }
                break;

            case "Power":
                {
                    Power = Amount;
                }
                break;

            case "CriticalPercent":
                {
                    CriticalPercent = Amount;
                }
                break;

            case "CriticalDamage":
                {
                    CriticalDamage = Amount;
                }
                break;
        }
    }

    public int GetStat(StatType type)
    {
        switch (type)
        {
            case StatType.HP:
                {
                    return HP;
                }

            case StatType.MP:
                {
                    return MP;
                }

            case StatType.Speed:
                {
                    return Speed;
                }

            case StatType.Power:
                {
                    return Power;
                }

            case StatType.CriticalPercent:
                {
                    return CriticalPercent;
                }

            case StatType.CriticalDamage:
                {
                    return CriticalDamage;
                }

            default:
                {
                    return 0;
                }
        }
    }

    public int HP;
    public int MP;
    public int Speed;
    public int Power;
    public int CriticalPercent;
    public int CriticalDamage;
}

public enum Rating : short
{
    Common = 1,
    Rare = 2,
    Unique = 3,
    None = -1
}

public enum Artifact : short
{
    Fire = 1,
    Ice = 2,
    Stone = 3,
    None = -1
}

public enum WeaponType : short
{
    OneHand = 1,
    TwoHand = 2,
    None = -1
}

public enum StatType : short
{
    HP = 1,
    MP = 2,
    Speed = 3,
    Power = 4,
    CriticalPercent = 5,
    CriticalDamage = 6,
    None = -1
}