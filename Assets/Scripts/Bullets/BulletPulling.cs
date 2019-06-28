using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletPulling
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

    private static List<int> test = new List<int>();
    public static Dictionary<QueueType, Queue<BulletListParam>> BulletDict = Enum.GetValues(typeof(QueueType)).OfType<QueueType>()
                                                                            .ToDictionary<QueueType, QueueType, Queue<BulletListParam>>(x => x, x => new Queue<BulletListParam>());

    public static Queue<BulletListParam> GetQueue(QueueType type)
    {
        return BulletDict[type];
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
}
