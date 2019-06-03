using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCMannager;

public class BulletBase : MonoBehaviour 
{

    public class Node<DataTy>
    {
        public DataTy Data;
        public Node<DataTy> Next;

        public Node(DataTy Data) { this.Data = Data; }
    }

    public class CircleQueue<DataTy>
    {
        private Node<DataTy> front;
        private Node<DataTy> Back;
        public int Count { get; private set; }

        public CircleQueue()
        {
            front = null;
            Back = null;
            Count = 0;
        }

        public void Push(DataTy Data)
        {
            if (Back == null)
            {
                Back = new Node<DataTy>(Data);
                front = Back;
                Back.Next = front;
                front.Next = Back;
            }
            else
            {
                Back.Next = new Node<DataTy>(Data);
                Back = Back.Next;
                Back.Next = front;
            }
            Count++;
        }

        public DataTy Pop()
        {
            DataTy Data = front.Data;
            if (front.Next == null) { front = null; }
            else { front = front.Next; }
            Count--;
            return Data;
        }

        public DataTy Peek() { return front.Data; }
    }

    public class BulletListParam
    {
        public Transform Bullet;
        public float LiveTime;    
    }

    protected CircleQueue<BulletListParam> ReUseBullet;
    protected List<BulletListParam> Bullets;
    protected float Speed;
    protected string PrefabPath;
    protected float LiveTime;

    protected virtual void AddBullet()
    {
        BulletListParam Bullet;

        if (ReUseBullet.Count == 0)
        {
            Bullet = new BulletListParam();
            Bullet.Bullet = Instantiate(Resources.Load<Transform>(PrefabPath));
        }
        else
        {
            Bullet = ReUseBullet.Pop();
            Bullet.Bullet.gameObject.SetActive(true);
        }

        Bullet.Bullet.position = transform.position;
        Bullet.Bullet.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        Bullet.LiveTime = LiveTime;
        Bullet.Bullet.parent = transform;
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
                        ReUseBullet.Push(Bullets[i]);
                        Bullets[i].Bullet.position = transform.position;
                        Bullets[i].Bullet.rotation = Quaternion.identity;
                        Bullets[i].Bullet.gameObject.SetActive(false);
                        Bullets.RemoveAt(i--);
                        continue;
                    }

                    Bullets[i].Bullet.Translate(new Vector2(0, -Speed), Space.Self);
                }
                yield return CoroDict.ContainsKey(0.0625f) ? CoroDict[0.0625f] : PushData(0.0625f, new WaitForSeconds(0.0625f));
            }
            yield return CoroWaitForEndFrame;
        }
    }

    protected virtual void Awake()
    {
        ReUseBullet = new CircleQueue<BulletListParam>();
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
