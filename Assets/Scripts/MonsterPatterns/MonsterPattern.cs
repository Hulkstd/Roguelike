﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static GCManager;

public static class MonsterPattern
{
    private static ObjectPooling PadPooling;

    public static void FollowPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 태형
    {
        IsinFunction = true;

        Seeker seeker = Me.GetComponent<Seeker>();
        Rigidbody2D rb2 = Me.GetComponent<Rigidbody2D>();

        seeker.StartPath(Me.position, Player.position, (Path path) => { StaticClassCoroutineManager.Instance.Perform(GoToPlayer(seeker, rb2, Player, path)); });
    }

    static IEnumerator GoToPlayer(Seeker seeker, Rigidbody2D rb2, Transform Player, Path path)
    {
        int CurrentWaypoint = 0;
        for (int i = 0; i < 3; i++) 
        {
            CurrentWaypoint = 0;
            while (true)
            {
                if (CurrentWaypoint >= path.vectorPath.Count)
                {
                    break;
                }

                Vector3 dir = (path.vectorPath[CurrentWaypoint] - seeker.transform.position).normalized;
                dir *= rb2.GetComponent<EnemyUnit>().MovementSpeed * Time.fixedDeltaTime;

                rb2.AddForce(dir, ForceMode2D.Force);

                if(Vector2.Distance(seeker.transform.position, path.vectorPath[CurrentWaypoint]) < 3)
                {
                    CurrentWaypoint++;
                }

                yield return CoroDict.ContainsKey(Time.fixedDeltaTime) ? CoroDict[Time.fixedDeltaTime] : PushData(Time.fixedDeltaTime, new WaitForSeconds(Time.fixedDeltaTime));
            }

            seeker.StartPath(seeker.transform.position, Player.position, (Path newpath) => { path = newpath; });
        }

        seeker.GetComponent<EnemyUnit>().IsinFunction = false;
    }

    public static void JumpToPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 재호 -> 태형
    {
        IsinFunction = true;

        StaticClassCoroutineManager.Instance.Perform(JumpScript(Me, Player));
    }

    private static IEnumerator JumpScript(Transform Me, Transform Player)
    {
        Vector3 vector = Player.position - Me.position;
        Vector3 clampDistance = Vector3.ClampMagnitude(vector, 10.0f);
        Vector3 dest = Me.position + clampDistance;
        Vector3 up = Quaternion.AngleAxis(-90, Vector3.back) * clampDistance;
        up.y = Mathf.Abs(up.y);

        Debug.Log(vector + " " + clampDistance + " " + dest + " " + up);
        int i = 0;
        float height = 0;

        for( ; i < 60; i++ )
        {
            Me.position = Vector3.Lerp(Me.position, dest, i * 0.01666f);

            if(i != 0)
            {
                Me.position -= up * height;
                height = Mathf.Sin(UtilityClass.Remap(i, 0, 59, -Mathf.PI, 0));
                Me.position += up * height;
            }

            yield return CoroWaitForFixedUpdate;
        }

        Me.GetComponent<EnemyUnit>().IsinFunction = false;
    }

    public static void DashToPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 재호 -> 태형
    {
        IsinFunction = true;

        StaticClassCoroutineManager.Instance.Perform(DashScript(Me, Player));
    } 

    private static IEnumerator DashScript(Transform Me, Transform Player)
    {
        Vector3 vector = Player.position - Me.position;
        Vector3 clampDistance = Vector3.ClampMagnitude(vector, 10.0f);
        Vector3 dest = Me.position + clampDistance;
        int i = 0;

        for (; i < 60; i++)
        {
            Me.position = Vector3.Lerp(Me.position, dest, i * 0.01666f);

            yield return CoroWaitForFixedUpdate;
        }
    }

    public static void RangeOnPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 태형 
    {
        IsinFunction = true;

        EnemyUnit enemyUnit = Me.GetComponent<EnemyUnit>();

        Vector3 ShootPosition = Player.transform.position;

        enemyUnit.WarningPoint.SetActive(true);
        enemyUnit.WarningPoint.transform.position = Player.position;
        enemyUnit.WarningPoint.transform.localScale = Vector3.one * enemyUnit.PlayerRange;

        StaticClassCoroutineManager.Instance.Perform(enemyUnit.RangeOnPlayerCoroutine());

        IsinFunction = false;
    } 

    public static void RangeOnMyself(Transform Me, Transform Player, ref bool IsinFunction) // 태형
    {
        IsinFunction = true;

        EnemyUnit enemyUnit = Me.GetComponent<EnemyUnit>();

        float Range = (Me.position - Player.position).magnitude;
        Range /= 3.38f;

        enemyUnit.WarningPoint.SetActive(true);
        enemyUnit.WarningPoint.transform.position = Me.transform.position;
        enemyUnit.WarningPoint.transform.localScale = Vector3.one * Range;

        StaticClassCoroutineManager.Instance.Perform(enemyUnit.RangeOnMySelfCoroutine());

        IsinFunction = false;
    }

    public static void N_WayBullet(Transform Me, Transform Player, ref bool IsinFunction) // 보성
    {
        IsinFunction = true;
        StaticClassCoroutineManager.Instance.Perform(ShotBullet(Me));
    }

    private static IEnumerator ShotBullet(Transform Me)
    {
        for (int i = 0; i < 10; i++)
        {
            NWayBullet.InItalize(0.1f, 4, Me.GetComponent<EnemyUnit>().BulletPrefPath, Me);
            NWayBullet.ShotBullet();
            NWayBullet.StartCoroutine();

            yield return CoroDict.ContainsKey(0.5f) ? CoroDict[0.5f] : PushData(0.5f, new WaitForSeconds(0.5f));
        }

        Me.GetComponent<EnemyUnit>().IsinFunction = false;
    }

    public static void Pad(Transform Me, Transform Player, ref bool IsinFunction) // 진우 태형
    {
        IsinFunction = true;

        if(PadPooling == null)
        {
            PadPooling = new ObjectPooling(Me.GetComponent<EnemyUnit>().PadPrefabs);
        }

        StaticClassCoroutineManager.Instance.Perform(SpawnPad(Me.GetComponent<EnemyUnit>()));
        FollowPlayer(Me, Player, ref IsinFunction);
    } 

    static IEnumerator SpawnPad(EnemyUnit enemyUnit)
    {
        while(enemyUnit.IsinFunction)
        {
            GameObject gameObject = PadPooling.PopObject();
            gameObject.transform.position = enemyUnit.transform.position;
            gameObject.SetActive(true);

            StaticClassCoroutineManager.Instance.Perform(UtilityClass.Disable(gameObject, 10.0f));

            yield return CoroDict.ContainsKey(0.0333f) ? CoroDict[0.0333f] : PushData(0.0333f, new WaitForSeconds(0.0333f));
        }
    }
}

public class ObjectPooling
{
    private GameObject OriginalPrefabs;
    private Queue<GameObject> Objects;

    public ObjectPooling(GameObject Prefabs)
    {
        OriginalPrefabs = Prefabs;
        Objects = new Queue<GameObject>();
    }

    public GameObject PopObject()
    {
        if (Objects.Count == 0 || Objects.Peek().activeSelf)
        {
            GameObject returnobject = MonoBehaviour.Instantiate(OriginalPrefabs) as GameObject;
            Objects.Enqueue(returnobject);

            return returnobject;
        }
        else
        {
            GameObject returnvalue = Objects.Peek();
            Objects.Enqueue(Objects.Dequeue());

            return returnvalue;
        }
    }
}