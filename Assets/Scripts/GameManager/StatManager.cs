using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[AddComponentMenu("GameManager/StatManager")]
public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    public int HPStat = 0;
    public int MPStat = 0;
    public int SpeedStat = 0;
    public int PowerStat = 0;
    public int CriticalPercent = 0;
    public int CriticalDamage = 0;
    private int remainingStats = 0;
    public int RemainingStats
    {
        get
        {
            return remainingStats;
        }

        set
        {
            remainingStats = value;

            BinarySerialize(remainingStats);
        }
    }

    public int StatperHP = 3;
    public int StatperMP = 2;
    public float StatperSpeed = 0.01f;
    public float StatperPower = 0.5f;
    public float StatperCriticalPercent = 0.5f;
    public float StatperCriticalDamage = 0.05f;

    private AbilityManager AbilityManagerInstance
    {
        get
        {
            return AbilityManager.Instance;
        }
    }
    private InventoryManager InventoryManagerInstance
    {
        get
        {
            return InventoryManager.Instance;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (System.IO.File.Exists(Application.dataPath + "/skadmstmxptfid.dll"))
        {
            remainingStats = BinaryDeserialize();
        }
        else
        {
            RemainingStats = 0;
        }

        InvokeRepeating("ApplyStats", 0.0f, 0.0666666f);
    }

    public void ApplyStats()
    {
        StatManager statManager = StatManager.Instance;

        statManager.HPStat = 0;
        statManager.MPStat = 0;
        statManager.SpeedStat = 0;
        statManager.PowerStat = 0;
        statManager.CriticalDamage = 0;
        statManager.CriticalPercent = 0;

        statManager += AbilityManagerInstance.AbilityStat;
        statManager += InventoryManagerInstance.GetAllEquipmentStatSum();
    }

    public static StatManager operator +(StatManager statManager, Stat stat)
    {
        statManager.HPStat += stat.HP;
        statManager.MPStat += stat.MP;
        statManager.PowerStat += stat.Power;
        statManager.SpeedStat += stat.Speed;
        statManager.CriticalPercent += stat.CriticalPercent;
        statManager.CriticalDamage += stat.CriticalDamage;  

        return statManager;
    }

    public void AddRemainingStat(int value)
    {
        remainingStats += value;
    }

    private void BinarySerialize(int RemainingStat)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.dataPath + "/skadmstmxptfid.dll", FileMode.Create);
        binaryFormatter.Serialize(fileStream, RemainingStat);
        fileStream.Close();
    }

    private int BinaryDeserialize()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.dataPath + "/skadmstmxptfid.dll", FileMode.Open);

        int stat = (int)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();

        return stat;
    }
}
