using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using static GCMannager;
using static BulletPulling;
=======
using static GCManager;
>>>>>>> 61b0e28845b7207d23953a2a3d27874deca20d2e

public class NWayWagWagBullet : NWayBullet
{
    bool IsMoveLeft;

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
                        GetQueue(Type).Enqueue(Bullets[i]);
                        Bullets[i].Bullet.position = transform.position;
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

    protected override void InItalize(float speed, float second, string path)
    {
        base.InItalize(speed, second, path);
        AngleBuffer = new Vector3(0, 0, 0);
        MoveVectorBuffer = new Vector2(0, 0);
        BulletCount = 25;
        BulletDistance = 360f / BulletCount;
        MinAngle = -180;
        MaxAngle = 180;
    }

    protected override void Awake()
    {
        InItalize(0.5f, 10, @"BasicBullet");
    }

}
