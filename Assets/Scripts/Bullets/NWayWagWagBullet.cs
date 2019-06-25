﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;
using static BulletPulling;


public class NWayWagWagBullet
{
    protected static Transform Unit;
    protected static List<BulletListParam> Bullets;
    protected static float Speed;
    protected static string PrefabPath;
    protected static float LiveTime;
    protected static Vector3 AngleBuffer;
    protected static Vector2 MoveVectorBuffer;
    protected static QueueType Type;
    protected static bool IsLeft;
    protected static int BulletCount;
    protected static float MinAngle;
    protected static float MaxAngle;
    protected static float AddAngle;
    protected static float BulletDistance;
    protected static bool IsMoveLeft;

    private static void CreateNWayBullet(float AddAngle)
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
        AngleBuffer.z = AddAngle;

        bullet.Bullet.position = Unit.position;
        bullet.Bullet.eulerAngles = AngleBuffer;
        bullet.LiveTime = LiveTime;
        Bullets.Add(bullet);
    }

    private static void AddBullet()
    {
        float LeftAngle, RightAngle;

        if (BulletCount % 2 == 1) { CreateNWayBullet(Unit.eulerAngles.z); }

        LeftAngle = BulletCount % 2 == 0 ? Unit.eulerAngles.z - BulletDistance / 2 : Unit.eulerAngles.z - BulletDistance;
        RightAngle = BulletCount % 2 == 0 ? Unit.eulerAngles.z + BulletDistance / 2 : Unit.eulerAngles.z + BulletDistance;

        for (int i = 0; i < BulletCount / 2; ++i)
        {
            Debug.Log("LeftAngle = " + LeftAngle);
            Debug.Log("RightAngled = " + RightAngle);
            CreateNWayBullet(LeftAngle);
            CreateNWayBullet(RightAngle);
            LeftAngle -= BulletDistance;
            RightAngle += BulletDistance;
        }

    }

    private static IEnumerator MoveBullets()
    {
        while (true)
        {
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
                    MoveVectorBuffer.x = IsMoveLeft ? -0.3f : 0.3f;
                    MoveVectorBuffer.y = -Speed;
                    Bullets[i].Bullet.Translate(MoveVectorBuffer, Space.Self);
                }
                IsMoveLeft = !IsMoveLeft;
                yield return CoroDict.ContainsKey(0.0625f) ? CoroDict[0.0625f] : PushData(0.0625f, new WaitForSeconds(0.0625f));
            }
            yield return CoroWaitForEndFrame;
        }
    }

    public static void StartCoroutine()
    {
        StaticClassCoroutineManager.Instance.Perform(MoveBullets());
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
        MinAngle = -90;
        MaxAngle = 90;
        BulletCount = 15;
        BulletDistance = 180f / BulletCount < 15 ? 180f / BulletCount : 15;
        AngleBuffer = new Vector3(0, 0, 0);
        MoveVectorBuffer = new Vector2(0, 0);
        BulletCount = 25;
        BulletDistance = 360f / BulletCount;
        MinAngle = -180;
        MaxAngle = 180;
    }

}
