﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// Name, Strength, fixed Strength, Shiled, Hazard level, Defense (or defensive power), Defensive form, movement speed (or moving velocity), DropItem, Attack Pattern
[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class EnemyUnit : MonoBehaviour
{
    public delegate void DropItemFunc();
    public delegate void AttackPattern(Transform Me, Transform Player, ref bool IsinFunction);

    public string Name;
    public int Strength;
    public int FixedStrength;
    public int Shield;
    public int HazardLevel;
    public int DefensivePower;
    public int NWayBulletCount;
    public float MovementSpeed;
    public float JumpSpeed;
    public float DashSpeed;
    public float PlayerRange = 2.0f;
    public DefensiveForm[] Forms;
    public DropItemFunc[] DropItems;
    public AttackPattern[] AttackPatterns;
    public Transform Player; // 나중에 움직이는 코드로 바꿔.
    public bool IsinFunction;

    private Coroutine RandomPatternCo;
    private int OneHandMinDamage = 500, TwoHandMinDamage = 500;
    private Dictionary<DefensiveForm, int> GetDamage = new Dictionary<DefensiveForm, int>()
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

    void Start()
    {
        RandomPatternCo = StartCoroutine(RandomPattern());

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
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(2.0f);

            AttackPatterns[Random.Range(0, AttackPatterns.Length)](transform, Player, ref IsinFunction);
        }
    }

    public void DamagedByUnit(Collider collider)
    {
        //BaseWeapon baseWeapon = collider.GetComponent<BaseWeapon>();

        //WeaponType type = baseWeapon.EquipData.WeaponType;

        //int Damage = 0;

        //switch(type)
        //{
        //    case WeaponType.OneHand:
        //        {
        //            Damage = baseWeapon.TryGetStat("Power");

        //            Damage = Damage * OneHandMinDamage / 100;
        //        }
        //        break;

        //    case WeaponType.TwoHand:
        //        {
        //            Damage = baseWeapon.TryGetStat("Power");

        //            Damage = Damage * TwoHandMinDamage / 100;
        //        }
        //        break;
        //}

        //Strength -= Mathf.Clamp(Damage, 1, FixedStrength);
    }
}

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