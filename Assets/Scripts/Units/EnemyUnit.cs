using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Name, Strength, fixed Strength, Shiled, Hazard level, Defense (or defensive power), Defensive form, movement speed (or moving velocity), DropItem, Attack Pattern
public class EnemyUnit : MonoBehaviour
{
    public delegate void DropItemFunc();
    public delegate void AttackPattern(ref bool IsinFunction);

    public string Name;
    public int Strength;
    public int FixedStrength;
    public int Shield;
    public int HazardLevel;
    public int DefensivePower;
    public DefensiveForm[] Forms;
    public float MovementSpeed;
    public DropItemFunc[] DropItems;
    public AttackPattern[] AttackPatterns;

    private Coroutine RandomPatternCo;
    private bool IsinFunction;
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
            AttackPatterns[Random.Range(0, AttackPatterns.Length)](ref IsinFunction);
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