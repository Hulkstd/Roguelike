using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NWayBullet : BulletBase
{
    protected int BulletCount;
    protected float MinAngle;
    protected float MaxAngle;
    protected float BulletDistance; // 탄알간의 간격

    protected void CreateNWayBullet(float AddAngle)
    {
        BulletListParam Bullet;

        if (ReUseBullet.Count == 0)
        {
            Bullet = new BulletListParam();
            Bullet.Bullet = Instantiate(Resources.Load<Transform>(PrefabPath));
        }
        else
        {
            Bullet = ReUseBullet.Pop();
            Bullet.Bullet.gameObject.SetActive(true);
        }

        Bullet.Bullet.position = transform.position;
        Bullet.Bullet.eulerAngles = new Vector3(0, 0, AddAngle);
        Bullet.LiveTime = LiveTime;
        Bullet.Bullet.parent = transform;
        Bullets.Add(Bullet);
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
            CreateNWayBullet(LeftAngle);
            CreateNWayBullet(RightAngle);
            LeftAngle -= BulletDistance;
            RightAngle += BulletDistance;
        }

    }

    protected override void Awake()
    {
        ReUseBullet = new CircleQueue<BulletListParam>();
        Bullets = new List<BulletListParam>();
        BulletDistance = 10f;
        MinAngle = -90;
        MaxAngle = 90;
        LiveTime = 10;
        Speed = 0.5f;
        BulletCount = 9;   // 정할지 안정할지 상의
        PrefabPath = @"BulletPrefab/BasicBullet";
    }
}
