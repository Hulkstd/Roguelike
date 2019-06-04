using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCMannager;

public class HommingBullet : BulletBase
{

    private static Vector3 AddAngle;
    private static float Angle;
    private static float bulletAngle;

    protected override IEnumerator MoveBullets()
    {
        StartCoroutine("Homming");
        while (true)
        {
            while (Bullets.Count > 0)
            {
                for (int i = 0; i < Bullets.Count; ++i)
                {
                    Bullets[i].LiveTime -= 0.0625f;

                    if (Bullets[i].LiveTime <= 0 || !Bullets[i].Bullet.gameObject.activeSelf)
                    {
                        ReUseBullet.Enqueue(Bullets[i]);
                        Bullets[i].Bullet.position = transform.position;
                        Bullets[i].Bullet.rotation = Quaternion.identity;
                        Bullets[i].LiveTime = LiveTime;
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }                    

                    MoveVectorBuffer.y = -Speed;

                    Bullets[i].Bullet.Translate(MoveVectorBuffer, Space.Self);
                }

                yield return CoroDict.ContainsKey(0.0625f) ? CoroDict[0.0625f] : PushData(0.0625f, new WaitForSeconds(0.0625f));
            }
            yield return CoroWaitForEndFrame;
        }
    }

    protected virtual IEnumerator Homming()
    {
        while (true)
        {
            while (Bullets.Count > 0)
            {
                for (int i = 0; i < Bullets.Count; ++i)
                {
                    if (LookAtPlayer.GetMagnitude(Bullets[i].Bullet) <= 4)
                    {
                        bulletAngle = Bullets[i].Bullet.eulerAngles.z;
                        Angle = LookAtPlayer.GetAngle(Bullets[i].Bullet);
                        Debug.Log(Angle);
                        Debug.Log(Bullets[i].Bullet.eulerAngles);
                        Angle = Angle < 0 ? Angle + 360 : Angle >= 360 ? Angle - 360 : Angle;
                        if (Angle - bulletAngle < bulletAngle ? bulletAngle - Angle < AddAngle.z : Angle - bulletAngle < AddAngle.z)                         
                        {
                            LookAtPlayer.LookPlayer(Bullets[i].Bullet);
                        }
                        else
                        {
                            Bullets[i].Bullet.eulerAngles += Angle - bulletAngle < bulletAngle ? -AddAngle : AddAngle;
                        }
                    }
                }
                yield return CoroDict.ContainsKey(0.0625f) ? CoroDict[0.0625f] : PushData(0.0625f, new WaitForSeconds(0.0625f));
            }
            yield return CoroWaitForEndFrame;
        }
    }

    protected override void Awake()
    {
        InItalize(0.1f, 10, @"BulletPrefab/BasicBullet");
        AddAngle = new Vector3(0, 0, 10f);
    }
}
