using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;
using static BulletPulling;

public static class TornadoBullet
{
    private static Transform Unit;
    private static List<BulletListParam> Bullets;
    private static float Speed;
    private static string PrefabPath;
    private static float LiveTime;
    private static Vector3 AngleBuffer;
    private static Vector2 MoveVectorBuffer;
    private static QueueType Type;
    private static bool CoroutineFlag = false;
    private static bool IsLeft;
    private static float AddAngle; // 탄알이 휘는 것처럼 보여주게 더해주는 각도

    private static void AddBullet()
    {
        BulletListParam bullet;

        if (GetQueue(Type).Count <= 0)
        {
            bullet = new BulletListParam();
            bullet.Bullet = MonoBehaviour.Instantiate(Resources.Load<Transform>(PrefabPath));
        }
        else
        {
            bullet = GetQueue(Type).Dequeue();
            bullet.Bullet.gameObject.SetActive(true);
        }

        bullet.Bullet.position = Unit.position;
        AngleBuffer.z = Unit.eulerAngles.z;
        bullet.Bullet.eulerAngles = AngleBuffer;
        bullet.LiveTime = LiveTime;
        Bullets.Add(bullet);
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
                        Bullets[i].LiveTime = LiveTime;
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
        IsLeft = Random.Range(0, 2) == 1 ? true : false;
        AddAngle = 3;
    }

}
