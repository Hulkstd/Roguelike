using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public class TestCode : EnemyUnit
{
    protected override void Start()
    {
        AttackPatterns = new AttackPattern[4];

        AttackPatterns[0] = MonsterPattern.JumpToPlayer;
        AttackPatterns[1] = MonsterPattern.DashToPlayer;
        AttackPatterns[2] = MonsterPattern.RangeOnMyself;
        AttackPatterns[3] = MonsterPattern.N_WayBullet;
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

    public override IEnumerator RangeOnMySelfCoroutine()
    {
        yield return CoroDict.ContainsKey(10.0f) ? CoroDict[10.0f] : PushData(10.0f, new WaitForSeconds(10.0f));

        WarningPoint.SetActive(false);
    }

    public override IEnumerator RangeOnPlayerCoroutine()
    {
        yield return CoroDict.ContainsKey(2.0f) ? CoroDict[2.0f] : PushData(2.0f, new WaitForSeconds(2.0f));

        WarningPoint.SetActive(false);
    }
}
