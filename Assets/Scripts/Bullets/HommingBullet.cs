using System.Collections;
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
                        ReUseBullet.Push(Bullets[i]);
                        Bullets[i].Bullet.position = transform.position;
                        Bullets[i].Bullet.rotation = Quaternion.identity;
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }

                    if (LookAtPlayer.GetMagnitude(Bullets[i].Bullet) <= 4f)
                    {
                        LookAtPlayer.LookPlayer(Bullets[i].Bullet);
                    }

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
        Speed = 0.1f;
        PrefabPath = @"BulletPrefab/BasicBullet";
        LiveTime = 10;
    }

}
