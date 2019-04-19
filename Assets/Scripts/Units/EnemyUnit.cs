using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Name, Strength, fixed Strength, Shiled, Hazard level, Defense (or defensive power), Defensive form, movement speed (or moving velocity), DropItem, Attack Pattern
public class EnemyUnit : MonoBehaviour
{
    public delegate void DropItemFunc();
    public delegate void AttackPattern();

    public string Name;
    public int Strength;
    public int FixedStrength;
    public int Shield;
    public int HazardLevel;
    public int DefensivePower;
    public DefensiveForm Form;
    public float MovementSpeed;
    public DropItemFunc[] DropItems;
    public AttackPattern[] AttackPatterns;
}

public enum DefensiveForm : short
{
    NormalForm = 1,
    None = -1
}
