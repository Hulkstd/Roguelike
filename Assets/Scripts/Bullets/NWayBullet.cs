using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletPulling;

public class NWayBullet : BulletBase
{
    protected int BulletCount;
    protected float MinAngle;
    protected float MaxAngle;
    protected float BulletDistance; // 탄알간의 간격

    protected void CreateNWayBullet(float AddAngle)
    {
        BulletListParam bullet;

        if (GetQueue(Type).Count <= 0)
        {
            bullet = new BulletListParam();
            bullet.Bullet = Instantiate(Resources.Load<Transform>(PrefabPath));
        }
        else
        {
            bullet = GetQueue(Type).Dequeue();
            bullet.Bullet.gameObject.SetActive(true);
        }
        AngleBuffer.z = AddAngle;

        bullet.Bullet.position = transform.position;
        bullet.Bullet.eulerAngles = AngleBuffer;
        bullet.LiveTime = LiveTime;
        Bullets.Add(bullet);
    }

    protected override void AddBullet()
    {
        float LeftAngle, RightAngle;

        Debug.Log(transform.eulerAngles);

        if (BulletCount % 2 == 1) { CreateNWayBullet(transform.eulerAngles.z); }

        LeftAngle = BulletCount % 2 == 0 ? transform.eulerAngles.z - BulletDistance / 2 : transform.eulerAngles.z - BulletDistance;
        RightAngle = BulletCount % 2 == 0 ? transform.eulerAngles.z + BulletDistance / 2 : transform.eulerAngles.z + BulletDistance;

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

    protected override void InItalize(float speed, float second, string path)
    {
        base.InItalize(speed, second, path);
        MinAngle = -90;
        MaxAngle = 90;
        BulletCount = 15;
        BulletDistance = 180f / BulletCount < 15 ? 180f / BulletCount : 15;
    }

    protected override void Awake()
    {
        InItalize(0.5f, 10, @"BasicBullet");
    }
}
