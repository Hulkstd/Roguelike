using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static GCManager;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Name, Strength, fixed Strength, Shiled, Hazard level, Defense (or defensive power), Defensive form, movement speed (or moving velocity), DropItem, Attack Pattern
[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class EnemyUnit : MonoBehaviour 
{
    public delegate void DropItemFunc();
    public delegate void AttackPattern(Transform Me, Transform Player, ref bool IsinFunction);
    public delegate void DeathDelegate();

    public string Name;
    public int HP;
    public int FixedStrength;
    public int Shield;
    public int HazardLevel;
    public int DefensivePower;
    public int NWayBulletCount;
    public float MovementSpeed;
    public float JumpSpeed;
    public float DashSpeed;
    public float PlayerRange = 1.2f;
    public string BulletPrefPath = "BasicBullet";
    public GameObject WarningPoint;
    public GameObject PadPrefabs;
    public DefensiveForm[] Forms;
    public DropItemFunc DropItems;
    public AttackPattern[] AttackPatterns;
    public DeathDelegate DeathEvent;
    public PlayerMove Player;
    public bool IsinFunction;

    protected Coroutine RandomPatternCo;
    protected int OneHandMinDamage = 500, TwoHandMinDamage = 500;
    protected Dictionary<DefensiveForm, int> GetDamage = new Dictionary<DefensiveForm, int>()
    {
        { DefensiveForm.Shield , 100100 },
        { DefensiveForm.Racing_Armor, 200025 },
        { DefensiveForm.Heavy_Armor, 025200 },
        { DefensiveForm.Armament, 050050 },
        { DefensiveForm.Biology, 100100 },
        { DefensiveForm.Machine, 020020 },
        { DefensiveForm.Weak, 200200 },
        { DefensiveForm.Giant, 150150 },
    };

    protected virtual void Start()
    {
        RandomPatternCo = StartCoroutine(RandomPattern());
        DeathEvent += OnDeath;
        
        int tmp;
        int OneHandDamage, TwoHandDamage;

        foreach (DefensiveForm form in Forms)
        {
            tmp = GetDamage[form];

            TwoHandDamage = tmp % 1000;
            OneHandDamage = tmp / 1000;

            if (OneHandMinDamage > OneHandDamage)
                OneHandMinDamage = OneHandDamage;

            if (TwoHandMinDamage > TwoHandDamage)
                TwoHandMinDamage = TwoHandDamage;
        }
    }

    IEnumerator RandomPattern()
    {
        while(true)
        {
            while(IsinFunction)
            {
                yield return CoroWaitForFixedUpdate;
            }

            yield return CoroDict.ContainsKey(2.0f) ? CoroDict[2.0f] : PushData(2.0f, new WaitForSeconds(2.0f));

            AttackPatterns[Random.Range(0, AttackPatterns.Length)](transform, Player.transform, ref IsinFunction);
        }
    }

    public virtual void DamagedByUnit(Collider collider)
    {
        BaseWeapon baseWeapon = collider.GetComponent<BaseWeapon>();

        WeaponType type = baseWeapon.EquipData.WeaponType;

        int Damage = 0;

        switch (type)
        {
            case WeaponType.OneHand:
                {
                    Damage = baseWeapon.EquipData.TryGetStat(StatType.Power);

                    Damage = Damage * OneHandMinDamage / 100;
                }
                break;

            case WeaponType.TwoHand:
                {
                    Damage = baseWeapon.EquipData.TryGetStat(StatType.Power);

                    Damage = Damage * TwoHandMinDamage / 100;
                } 
                break;
        }

        HP -= Mathf.Clamp(Damage, 1, FixedStrength);
    }

    public virtual void OnDeath()
    {
        StopCoroutine(RandomPatternCo);
        gameObject.SetActive(false);
    }

    public virtual IEnumerator RangeOnMySelfCoroutine()
    {
        yield return null;
    }
    public virtual IEnumerator RangeOnPlayerCoroutine()
    {
        yield return null;
    }
}

[System.Serializable]
public enum DefensiveForm : short
{
    Shield = 1,
    Racing_Armor = 2,
    Heavy_Armor = 3,
    Armament = 4,
    Biology = 5,
    Machine = 6,
    Weak = 7,
    Giant = 8,
    None = -1
}

[System.Serializable]
public enum DropItemType : short
{
    SkillPoint = 1,
    Essence = 2,
    FireArtifact = 3,
    IceArtifact = 4,
    StoneArtifact = 5,
    None = -1
}