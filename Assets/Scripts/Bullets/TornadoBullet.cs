using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBullet : BulletBase
{
    protected bool IsLeft;
    protected float AddAngle; // 탄알이 휘는 것처럼 보여주게 더해주는 각도

    protected override IEnumerator MoveBullets()
    {
        while (Bullets.Count > 0)
        {
            for (int i = 0; i < Bullets.Count; ++i)
            {
                Bullets[i].LiveTime -= 0.25f;

                if (Bullets[i].LiveTime <= 0)
                {
                    Bullets[i].Bullet.gameObject.SetActive(false);
                    Bullets.RemoveAt(i--);
                    continue;
                }

                Bullets[i].Bullet.eulerAngles = new Vector3(0, 0, Bullets[i].Bullet.eulerAngles.z + (IsLeft ? AddAngle : -AddAngle));
                Bullets[i].Bullet.Translate(new Vector2(0, -Speed), Space.Self);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    protected override void Awake()
    {
        IsLeft = Random.Range(0, 1) == 1 ? true : false;
        Bullets = new List<BulletListParam>();
        Speed = 10f;
        AddAngle = 5; // add frame
        LiveTime = 10;
        PrefabPath = @"BulletPrefab/Tornado";
    }
}
