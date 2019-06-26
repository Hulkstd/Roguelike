using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : EnemyUnit
{
    protected override void Start()
    {
        AttackPatterns = new AttackPattern[4];

        AttackPatterns[0] = MonsterPattern.JumpToPlayer;
        AttackPatterns[1] = MonsterPattern.DashToPlayer;
        AttackPatterns[2] = MonsterPattern.RangeOnMyself;
        AttackPatterns[3] = MonsterPattern.RangeOnPlayer;
    }

    private void DropItem()
    {

    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            AttackPatterns[0](transform, Player.transform, ref IsinFunction);
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            AttackPatterns[1](transform, Player.transform, ref IsinFunction);
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            AttackPatterns[2](transform, Player.transform, ref IsinFunction);
        }
        if(Input.GetKeyUp(KeyCode.F))
        {
            AttackPatterns[3](transform, Player.transform, ref IsinFunction);
        }
    }
}
