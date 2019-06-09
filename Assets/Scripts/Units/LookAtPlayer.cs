using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public class LookAtPlayer : MonoBehaviour
{
    private static FloatPair buffer;
    public static Transform Player { get; private set; }
    private static float ArcAngle;
    private static Vector3 Angle;

    public static void LookPlayer(Transform Obj)
    {
        Angle.z = GetAngle(Obj); 
        Obj.eulerAngles = Angle;
    }

    public static float GetMagnitude(Transform Obj)
    {
        float distance_x = Obj.position.x - Player.position.x;
        float distance_y = Obj.position.y - Player.position.y;
        return distance_x * distance_x + distance_y * distance_y;
    }

    public static float GetAngle(Transform Obj)
    {
        buffer.x = Obj.position.x - Player.position.x;
        buffer.y = Obj.position.y - Player.position.y;
        ArcAngle = AtanDict.ContainsKey(buffer) ? AtanDict[buffer] : PushData(buffer, Mathf.Atan2(buffer.x, buffer.y));
        return -ArcAngle * Mathf.Rad2Deg;
    }

    private void Awake()
    {
        buffer = new FloatPair();
        Angle = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
    }

}
