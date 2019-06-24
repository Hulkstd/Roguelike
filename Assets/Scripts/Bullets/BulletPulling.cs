using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPulling : MonoBehaviour
{

    public class BulletListParam
    {
        public Transform Bullet;
        public float LiveTime;
    }

    public enum QueueType
    {
        Basic = 0
    }

    public static Queue<BulletListParam> BasicBullets { get; set; }

    public static Queue<BulletListParam> GetQueue(QueueType type)
    {
        switch (type)
        {
            case QueueType.Basic: return BasicBullets;
        }

        Debug.LogError("path error");
        return null;
    }

    public static QueueType GetQueueType(string path)
    {
        switch (path)
        {
            case @"BasicBullet": return QueueType.Basic;
        }

        Debug.LogError("path error");
        return QueueType.Basic;
    }

    private void Awake()
    {
        BasicBullets = new Queue<BulletListParam>();
    }

}
