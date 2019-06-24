using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCMannager;
using static BulletPulling;

public class HommingBullet : BulletBase
{
    protected static Vector3 AddAngle;
    protected static float Angle;
    protected static bool isAdd;

    protected override IEnumerator MoveBullets()
    {
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
                        Bullets[i].Bullet.position = transform.position;
                        Bullets[i].Bullet.rotation = Quaternion.identity;
                        Bullets[i].LiveTime = LiveTime;
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }                    

                    MoveVectorBuffer.y = -Speed;

                    Angle = LookAtPlayer.GetAngle(Bullets[i].Bullet);
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

    protected override void InItalize(float speed, float second, string path)
    {
        base.InItalize(speed, second, path);
        AddAngle = Vector3.zero;
    }

    protected override void Awake()
    {
        InItalize(0.1f, 5, @"BasicBullet");
    }

    protected override void Start()
    {
        base.Start();
    }

}
