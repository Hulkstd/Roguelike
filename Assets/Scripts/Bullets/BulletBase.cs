using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public class BulletBase : MonoBehaviour 
{
    public class BulletListParam
    {
        public Transform Bullet;
        public float LiveTime;    
    }

    protected Queue<BulletListParam> ReUseBullet;
    protected List<BulletListParam> Bullets;
    protected float Speed;
    protected string PrefabPath;
    protected float LiveTime;
    protected Vector3 AngleBuffer;
    protected Vector2 MoveVectorBuffer;

    protected virtual void AddBullet()
    {
        BulletListParam Bullet;

        if (ReUseBullet.Count <= 0)
        {
            Bullet = new BulletListParam();
            Bullet.Bullet = Instantiate(Resources.Load<Transform>(PrefabPath));
        }
        else
        {
            Bullet = ReUseBullet.Dequeue();
            Bullet.Bullet.gameObject.SetActive(true);
        }

        Bullet.Bullet.position = transform.position;
        AngleBuffer.z = transform.eulerAngles.z;
        Bullet.Bullet.eulerAngles = AngleBuffer;
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

    public virtual void ShotBullet()
    {
        AddBullet();
    }

    protected virtual void InItalize(float speed, float second, string path)
    {
        ReUseBullet = new Queue<BulletListParam>();
        Bullets = new List<BulletListParam>();
        AngleBuffer = new Vector3(0, 0, 0);
        MoveVectorBuffer = new Vector2(0, 0);
        LiveTime = second;
        Speed = speed;
        PrefabPath = path;
    }

    protected virtual void Awake()
    {
        InItalize(0.5f, 10, @"BulletPrefab/BasicBullet");
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
