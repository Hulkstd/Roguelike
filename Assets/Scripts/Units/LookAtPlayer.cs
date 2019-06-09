using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public class LookAtPlayer : MonoBehaviour
{
    private static FloatPair buffer;
    private static Transform Player;
    private static float ArcAngle;
    private static Vector3 Angle;

    public static void LookPlayer(Transform Enemy)
    {
        buffer.x = Enemy.position.x - Player.position.x;
        buffer.y = Enemy.position.y - Player.position.y;
        ArcAngle = AtanDict.ContainsKey(buffer) ? AtanDict[buffer] : PushData(buffer, Mathf.Atan2(buffer.x, buffer.y));
        Angle.z = -ArcAngle * Mathf.Rad2Deg;
        Enemy.eulerAngles = Angle;
    }

    public static float GetMagnitude(Transform Obj)
    {
        float distance_x = Obj.position.x - Player.position.x;
        float distance_y = Obj.position.y - Player.position.y;
        return distance_x * distance_x + distance_y * distance_y;
    }

    private void Awake()
    {
        buffer = new FloatPair();
    }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        Angle = new Vector3(0, 0, 0);
    }

}
