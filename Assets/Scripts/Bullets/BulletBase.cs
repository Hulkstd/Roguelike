using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using static GCMannager;
using static BulletPulling;
=======
using static GCManager;
>>>>>>> 61b0e28845b7207d23953a2a3d27874deca20d2e

public class BulletBase : MonoBehaviour 
{
    protected List<BulletListParam> Bullets;
    protected float Speed;
    protected string PrefabPath;
    protected float LiveTime;
    protected Vector3 AngleBuffer;
    protected Vector2 MoveVectorBuffer;
    protected QueueType Type;

    protected virtual void AddBullet()
    {
        BulletListParam bullet;

        if (GetQueue(Type).Count <= 0)
        {
            bullet = new BulletListParam();
            bullet.Bullet = Instantiate(Resources.Load<Transform>(PrefabPath));
        }
        else
        {
            bullet = GetQueue(Type).Dequeue();
            bullet.Bullet.gameObject.SetActive(true);
        }

        bullet.Bullet.position = transform.position;
        AngleBuffer.z = transform.eulerAngles.z;
        bullet.Bullet.eulerAngles = AngleBuffer;
        bullet.LiveTime = LiveTime;
        Bullets.Add(bullet);
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
                        GetQueue(Type).Enqueue(Bullets[i]);
                        Bullets[i].Bullet.position = transform.position;
                        Bullets[i].Bullet.eulerAngles = transform.eulerAngles;
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
        Bullets = new List<BulletListParam>();
        Type = GetQueueType(path);
        AngleBuffer = new Vector3(0, 0, 0);
        MoveVectorBuffer = new Vector2(0, 0);
        LiveTime = second;
        Speed = speed;
        PrefabPath = @"BulletPrefab/" + path;
    }

    protected virtual void Awake()
    {
        InItalize(0.5f, 10, @"BasicBullet");
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
