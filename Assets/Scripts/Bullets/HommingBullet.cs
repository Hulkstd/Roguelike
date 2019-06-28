﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;
using static BulletPulling;

public static class HommingBullet
{
    private static Transform Unit;
    private static Transform Player;
    private static List<BulletListParam> Bullets;
    private static float Speed;
    private static string PrefabPath;
    private static float LiveTime;
    private static Vector3 AngleBuffer;
    private static Vector2 MoveVectorBuffer;
    private static QueueType Type;
    private static bool CoroutineFlag = false;
    private static Vector3 AddAngle;
    private static float Angle;
    private static bool isAdd;

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
        CoroutineFlag = true;
        while (true)
        {
            while (Bullets.Count > 0)
            {
                for (int i = 0; i < Bullets.Count; ++i)
                {
                    Bullets[i].LiveTime -= 0.0625f;

                    if (Bullets[i].LiveTime <= 0 || !Bullets[i].Bullet.gameObject.activeSelf)
                    {
                        GetQueue(Type).Enqueue(Bullets[i]);
                        Bullets[i].Bullet.position = Unit.position;
                        Bullets[i].Bullet.rotation = Quaternion.identity;
                        Bullets[i].LiveTime = LiveTime;
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }                    

                    MoveVectorBuffer.y = -Speed;

                    Angle = LookAtPlayer.GetAngle(Bullets[i].Bullet, Player);
                    Angle -= Bullets[i].Bullet.eulerAngles.z;

                    while (Angle < 0) { Angle += 360; }
                    while (Angle >= 360) { Angle -= 360; }

                    AddAngle.z = ((Angle / 10 > 3) ? 3 : (Angle / 10));
                    isAdd = (Angle <= 180 ? true : false);

                    Bullets[i].Bullet.eulerAngles += isAdd ? AddAngle : -AddAngle;
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
        Player = TransformDictionary.ContainsKey("Player") ? TransformDictionary["Player"] : PushData("Player", GameObject.FindGameObjectWithTag("Player").transform);
        Bullets = new List<BulletListParam>();
        Type = GetQueueType(path);
        AngleBuffer = new Vector3(0, 0, 0);
        MoveVectorBuffer = new Vector2(0, 0);
        LiveTime = second;
        Speed = speed;
        PrefabPath = @"BulletPrefab/" + path;
        AddAngle = Vector3.zero;
    }


}
