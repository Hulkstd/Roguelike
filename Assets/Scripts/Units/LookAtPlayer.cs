using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public class LookAtPlayer : MonoBehaviour
{
    private static FloatPair Buffer;
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

    public static float GetAngle(Transform obj)
    {
        Buffer.x = obj.position.x - Player.position.x;
        Buffer.y = obj.position.y - Player.position.y;
        ArcAngle = AtanDict.ContainsKey(Buffer) ? AtanDict[Buffer] : PushData(Buffer, Mathf.Atan2(Buffer.y, Buffer.x));
        ArcAngle *= Mathf.Rad2Deg;
        ArcAngle -= 90;
        return ArcAngle;
    }

    private void Awake()
    {
        Buffer = new FloatPair();
        Angle = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
    }

}
