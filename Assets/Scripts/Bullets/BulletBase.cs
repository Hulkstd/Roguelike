using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic
public class BulletBase : MonoBehaviour 
{
    public class BulletListParam
    {
        public Transform Bullet;
        public float LiveTime;    
    }

    protected Queue<int> queue;
    protected List<BulletListParam> Bullets;
    protected float Speed;
    protected string PrefabPath;
    protected float LiveTime;

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
        while (true)
        {
            while (Bullets.Count > 0)
            {
                for (int i = 0; i < Bullets.Count; ++i)
                {
                    Bullets[i].LiveTime -= 0.0625f;

                    if (Bullets[i].LiveTime <= 0)
                    {
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }

                    Bullets[i].Bullet.Translate(new Vector2(0, -Speed), Space.Self);
                }

                yield return new WaitForSeconds(0.0625f);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual void Awake()
    {
        Bullets = new List<BulletListParam>();
        PrefabPath = @"BulletPrefab/BasicBullet";
        LiveTime = 10;
        Speed = 0.5f;
    }
    protected virtual void Start() { StartCoroutine("MoveBullets"); }
    protected virtual void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            AddBullet();
        }
    }
    protected virtual void LateUpdate() { }

}
