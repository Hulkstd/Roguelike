using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

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
                        ReUseBullet.Enqueue(Bullets[i]);
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
        ReUseBullet = new Queue<BulletListParam>();
        Bullets = new List<BulletListParam>();
        AngleBuffer = new Vector3(0, 0, 0);
        MoveVectorBuffer = new Vector2(0, 0);
        LiveTime = second;
        Speed = speed;
        PrefabPath = path;
        BulletCount = 25;
        BulletDistance = 360f / BulletCount;
        MinAngle = -180;
        MaxAngle = 180;
    }

    protected override void Awake()
    {
        InItalize(0.5f, 10, @"BulletPrefab/BasicBullet");
    }

}
