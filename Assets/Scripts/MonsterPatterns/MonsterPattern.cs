using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static GCMannager;

public class MonsterPattern : MonoBehaviour
{

    public virtual void FollowPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 태형
    {
        IsinFunction = true;

        Seeker seeker = Me.GetComponent<Seeker>();
        Rigidbody2D rb2 = Me.GetComponent<Rigidbody2D>();

        seeker.StartPath(Me.position, Player.position, (Path path) => { StartCoroutine(GoToPlayer(seeker, rb2, Player, path)); });
    }

    IEnumerator GoToPlayer(Seeker seeker, Rigidbody2D rb2, Transform Player, Path path)
    {
        int CurrentWaypoint = 0;
        for (int i = 0; i < 3; i++) 
        {
            while (true)
            {
                if (CurrentWaypoint >= path.vectorPath.Count)
                {
                    break;
                }

                Vector3 dir = (path.vectorPath[CurrentWaypoint] - seeker.transform.position).normalized;
                dir *= seeker.transform.position.z * Time.fixedDeltaTime;

                rb2.AddForce(dir, ForceMode2D.Force);

                yield return CoroDict.ContainsKey(Time.fixedDeltaTime) ? CoroDict[Time.fixedDeltaTime] : PushData(Time.fixedDeltaTime, new WaitForSeconds(Time.fixedDeltaTime));
            }

            seeker.StartPath(seeker.transform.position, Player.position, (Path newpath) => { path = newpath; });
        }

        seeker.GetComponent<EnemyUnit>().IsinFunction = false;
    }

    public virtual void JumpToPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 재호
    {

    }

    public virtual void DashToPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 재호 
    {

    } 

    public virtual void RangeOnPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 태형 
    {
        EnemyUnit enemyUnit = Me.GetComponent<EnemyUnit>();

        Vector3 ShootPosition = Player.transform.position;

        enemyUnit.WarningPoint.transform.position = Player.transform.position;

        enemyUnit.WarningPoint.transform.localScale = new Vector3(enemyUnit.PlayerRange, enemyUnit.PlayerRange);
    } 

    public virtual void RangeOnMyself(Transform Me, Transform Player, ref bool IsinFunction) // 태형
    {
        EnemyUnit enemyUnit = Me.GetComponent<EnemyUnit>();

        float Range = (Me.position - Player.position).magnitude;

        enemyUnit.WarningPoint.transform.position = Me.transform.position;

        enemyUnit.WarningPoint.transform.localScale = new Vector3(Range, Range);
    }

    public virtual void N_WayBullet(Transform Me, Transform Player, ref bool IsinFunction) // 재호 진우
    {

    }

    public virtual void Pad(Transform Me, Transform Player, ref bool IsinFunction) // 진우 태형
    {

    } 
}
