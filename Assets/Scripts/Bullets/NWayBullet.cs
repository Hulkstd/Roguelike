using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NWayBullet : BulletBase
{
    protected int BulletCount;
    protected float MinAngle;
    protected float MaxAngle;
    protected float AddAngles; // 탄알간의 간격

    protected override void AddBullet()
    {
        AddAngles = 180 / (BulletCount + 1);

        float Z = -90;

        for (int i = 0; i < BulletCount; ++i)
        {
            Z += AddAngles;
            BulletListParam Bullet = new BulletListParam();
            Bullet.Bullet = Instantiate(Resources.Load<Transform>(PrefabPath));
            Bullet.Bullet.position = transform.position;
            Bullet.Bullet.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Z);
            Bullet.LiveTime = LiveTime;
            Bullets.Add(Bullet);
        }
    }

    protected override void Awake()
    {
        Bullets = new List<BulletListParam>();
        MinAngle = -90f;
        MaxAngle = 90f;
        LiveTime = 10;
        Speed = 10f;
        BulletCount = Random.Range(1,10);   // 정할지 안정할지 상의
    }
}
