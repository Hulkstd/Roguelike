using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;
using static BulletPulling;

public class NWayTornadoBullet
{
    protected static Transform Unit;
    protected static List<BulletListParam> Bullets;
    protected static float Speed;
    protected static string PrefabPath;
    protected static float LiveTime;
    protected static Vector3 AngleBuffer;
    protected static Vector2 MoveVectorBuffer;
    protected static QueueType Type;
    protected static bool CoroutineFlag = false;
    protected static bool IsLeft;
    protected static int BulletCount;
    protected static float MinAngle;
    protected static float MaxAngle;
    protected static float AddAngle;
    protected static float BulletDistance;

    private static void AddBullet()
    {
        BulletDistance = 360f / BulletCount;

        float add_z = MinAngle;

        BulletListParam bullet;

        for (int i = 0; i < BulletCount; ++i)
        {
            if (BasicBullets.Count <= 0)
            {
                bullet = new BulletListParam();
                bullet.Bullet = MonoBehaviour.Instantiate(Resources.Load<Transform>(PrefabPath));
            }
            else
            {
                bullet = BasicBullets.Dequeue();
                bullet.Bullet.gameObject.SetActive(true);
            }

            AngleBuffer.z = Unit.eulerAngles.z + add_z;

            bullet.Bullet.position = Unit.position;
            bullet.Bullet.eulerAngles = AngleBuffer;
            bullet.LiveTime = LiveTime;
            Bullets.Add(bullet);

            add_z += BulletDistance;
        }
    }

    private static IEnumerator MoveBullets()
    {
        while (true)
        {
            CoroutineFlag = true;
            while (Bullets.Count > 0)
            {
                for (int i = 0; i < Bullets.Count; ++i)
                {
                    Bullets[i].LiveTime -= 0.0625f;

                    if (Bullets[i].LiveTime <= 0)
                    {
                        GetQueue(Type).Enqueue(Bullets[i]);
                        Bullets[i].Bullet.position = Unit.position;
                        Bullets[i].Bullet.rotation = Quaternion.identity;
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }

                    AngleBuffer.z = Bullets[i].Bullet.eulerAngles.z + (IsLeft ? AddAngle : -AddAngle);
                    MoveVectorBuffer.y = -Speed;

                    Bullets[i].Bullet.eulerAngles = AngleBuffer;
                    Bullets[i].Bullet.Translate(MoveVectorBuffer, Space.Self);
                }

                yield return CoroDict.ContainsKey(0.0625f) ? CoroDict[0.0625f] : PushData(0.0625f, new WaitForSeconds(0.0625f));
            }
            yield return CoroWaitForEndFrame;
        }
    }

    public static void StartCoroutine()
    {
        if (!CoroutineFlag)
        {
            StaticClassCoroutineManager.Instance.Perform(MoveBullets());
        }
    }

    public static void ShotBullet()
    {
        AddBullet();
    }

    public static void InItalize(float speed, float second, string path, Transform transform)
    {
        Unit = transform;
        Bullets = new List<BulletListParam>();
        Type = GetQueueType(path);
        AngleBuffer = new Vector3(0, 0, 0);
        MoveVectorBuffer = new Vector2(0, 0);
        LiveTime = second;
        Speed = speed;
        PrefabPath = @"BulletPrefab/" + path;
        AddAngle = 5;
        BulletCount = 18;
        MinAngle = -180;
        MaxAngle = 180;
        IsLeft = Random.Range(0, 2) == 1 ? true : false;
    }

}
