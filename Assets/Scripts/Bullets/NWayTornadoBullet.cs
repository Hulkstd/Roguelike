using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCMannager;

public class NWayTornadoBullet : BulletBase
{
    protected bool IsLeft;
    protected int BulletCount;
    protected float MinAngle;
    protected float MaxAngle;
    protected float AddAngle;
    protected float BulletDistance;

    protected override void AddBullet()
    {
        BulletDistance = 360f / BulletCount;

        float add_z = MinAngle;

        BulletListParam Bullet;

        for (int i = 0; i < BulletCount; ++i)
        {
            if (ReUseBullet.Count == 0)
            {
                Bullet = new BulletListParam();
                Bullet.Bullet = Instantiate(Resources.Load<Transform>(PrefabPath));
            }
            else
            {
                Bullet = ReUseBullet.Dequeue();
                Bullet.Bullet.gameObject.SetActive(true);
            }

            AngleBuffer.z = transform.eulerAngles.z + add_z;

            Bullet.Bullet.position = transform.position;
            Bullet.Bullet.eulerAngles = AngleBuffer;
            Bullet.LiveTime = LiveTime;
            Bullets.Add(Bullet);

            add_z += BulletDistance;
        }
    }

    protected override IEnumerator MoveBullets()
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
                        ReUseBullet.Enqueue(Bullets[i]);
                        Bullets[i].Bullet.position = transform.position;
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

    protected override void InItalize(float speed, float second, string path)
    {
        base.InItalize(speed, second, path);
        AddAngle = 2;
        BulletCount = 18;
        MinAngle = -180;
        MaxAngle = 180;
        IsLeft = Random.Range(0, 2) == 1 ? true : false;
    }

    protected override void Awake()
    {
        InItalize(0.1f, 10, @"BulletPrefab/BasicBullet");
    }

}
