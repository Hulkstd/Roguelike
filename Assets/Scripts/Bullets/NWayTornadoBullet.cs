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
        BulletDistance = 360 / BulletCount;

        float Z = MinAngle;

        for (int i = 0; i < BulletCount; ++i)
        {
            if (Z > MaxAngle) { Debug.LogError("BulletDistance to long"); return; }

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
            Bullet.Bullet.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Z);
            Bullet.LiveTime = LiveTime;
            Bullet.Bullet.parent = transform;
            Bullets.Add(Bullet);

            Z += BulletDistance;
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
                        ReUseBullet.Push(Bullets[i]);
                        Bullets[i].Bullet.position = transform.position;
                        Bullets[i].Bullet.rotation = Quaternion.identity;
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }

                    Bullets[i].Bullet.eulerAngles = new Vector3(0, 0, Bullets[i].Bullet.eulerAngles.z + (IsLeft ? AddAngle : -AddAngle));
                    Bullets[i].Bullet.Translate(new Vector2(0, -Speed), Space.Self);
                }

                yield return CoroDict.ContainsKey(0.0625f) ? CoroDict[0.0625f] : PushData(0.0625f, new WaitForSeconds(0.0625f));
            }
            yield return CoroWaitForEndFrame;
        }
    }

    protected override void Awake()
    {
        ReUseBullet = new CircleQueue<BulletListParam>();
        Bullets = new List<BulletListParam>();
        IsLeft = Random.Range(0, 2) == 1 ? true : false;
        MinAngle = -180;
        MaxAngle = 180;
        LiveTime = 10;
        Speed = 0.3f;
        BulletCount = 10;
        AddAngle = 5;
        PrefabPath = @"BulletPrefab/BasicBullet";
    }

}
