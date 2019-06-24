using System.Collections;
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

    public static void JumpToPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 재호
    {

    }

    public static void DashToPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 재호 
    {

    } 

    public static void RangeOnPlayer(Transform Me, Transform Player, ref bool IsinFunction) // 태형 
    {
        IsinFunction = true;

        EnemyUnit enemyUnit = Me.GetComponent<EnemyUnit>();

        Vector3 ShootPosition = Player.transform.position;

        enemyUnit.WarningPoint.transform.position = Player.position;
        enemyUnit.WarningPoint.GetComponent<PurseMaterial>().SetOrigin(Player.position);
        enemyUnit.WarningPoint.transform.localScale = new Vector3(enemyUnit.PlayerRange, enemyUnit.PlayerRange);

        IsinFunction = false;
    } 

    public static void RangeOnMyself(Transform Me, Transform Player, ref bool IsinFunction) // 태형
    {
        IsinFunction = true;

        EnemyUnit enemyUnit = Me.GetComponent<EnemyUnit>();

        float Range = (Me.position - Player.position).magnitude;

        enemyUnit.WarningPoint.transform.position = Me.transform.position;
        enemyUnit.WarningPoint.GetComponent<PurseMaterial>().SetOrigin(Me.position);
        enemyUnit.WarningPoint.transform.localScale = new Vector3(Range, Range);

        IsinFunction = false;
    }

    public static void N_WayBullet(Transform Me, Transform Player, ref bool IsinFunction) // 보성
    {

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

            StaticClassCoroutineManager.Instance.Perform(Disable(gameObject, 10.0f));

            yield return CoroDict.ContainsKey(0.0333f) ? CoroDict[0.0333f] : PushData(0.0333f, new WaitForSeconds(0.0333f));
        }
    }

    private static IEnumerator Disable(GameObject gameObject, float t)
    {
        yield return CoroDict.ContainsKey(t) ? CoroDict[t] : PushData(t, new WaitForSeconds(t));

        gameObject.SetActive(false);
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