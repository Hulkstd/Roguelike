using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public class BulletListParam
    {
        public Transform Bullet;
        public float LiveTime;    
    }

    protected List<BulletListParam> Bullets;
    protected float Speed;
    protected string PrefabPath;
    protected float LiveTime;
    protected Transform Target;

    protected virtual void AddBullet()
    {
        BulletListParam Bullet = new BulletListParam();
        Bullet.Bullet = Instantiate(Resources.Load<Transform>(PrefabPath));
        Bullet.Bullet.position = transform.position;
        Bullet.Bullet.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        Bullet.LiveTime = LiveTime;
        Bullets.Add(Bullet);
    }

    protected virtual IEnumerator MoveBullets()
    {
        while (Bullets.Count > 0)
        {
            for (int i = 0; i < Bullets.Count; ++i)
            {
                Bullets[i].LiveTime -= 0.25f;
                if (Bullets[i].LiveTime <= 0)
                {
                    Destroy(Bullets[i].Bullet);
                    Bullets.RemoveAt(i--);
                    continue;
                }

                Bullets[i].Bullet.Translate(new Vector2(0, -Speed), Space.Self);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    protected virtual void Awake()
    {
        Bullets = new List<BulletListParam>();
    }
    protected virtual void Start() { }
    protected virtual void Update()
    {
        if (Bullets.Count == 1)
        {
            StartCoroutine("MoveBullets");
        }
    }
    protected virtual void LateUpdate() { }

}
