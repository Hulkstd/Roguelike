﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCMannager;

public class HommingBullet : BulletBase
{
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
                        ReUseBullet.Enqueue(Bullets[i]);
                        Bullets[i].Bullet.position = transform.position;
                        Bullets[i].Bullet.rotation = Quaternion.identity;
                        Bullets[i].LiveTime = LiveTime;
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }

                    if (LookAtPlayer.GetMagnitude(Bullets[i].Bullet) <= 4f)
                    {
                        LookAtPlayer.LookPlayer(Bullets[i].Bullet);
                    }

                    MoveVectorBuffer.y = -Speed;

                    Bullets[i].Bullet.Translate(MoveVectorBuffer, Space.Self);
                }

                yield return CoroDict.ContainsKey(0.0625f) ? CoroDict[0.0625f] : PushData(0.0625f, new WaitForSeconds(0.0625f));
            }
            yield return CoroWaitForEndFrame;
        }
    }

    protected override void Awake()
    {
        InItalize(0.1f, 10, @"BulletPrefab/BasicBullet");
    }

}
