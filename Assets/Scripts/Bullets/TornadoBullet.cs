using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public class TornadoBullet : BulletBase
{
    [SerializeField]
    protected bool IsLeft;
    protected float AddAngle; // 탄알이 휘는 것처럼 보여주게 더해주는 각도

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
        IsLeft = Random.Range(0, 2) == 1 ? true : false;
        AddAngle = 3;
    }

    protected override void Awake()
    {
        InItalize(1, 10, @"BulletPrefab/BasicBullet");
    }

    protected override void Start()
    {
        StartCoroutine("MoveBullets");
    }
}
