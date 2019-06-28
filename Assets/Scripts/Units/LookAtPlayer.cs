using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public static class LookAtPlayer
{
    private static FloatPair Buffer = new FloatPair();
    private static float ArcAngle;
    private static Vector3 Angle = Vector2.zero;

    public static void LookPlayer(Transform Obj, Transform Player)
    {
        Angle.z = GetAngle(Obj, Player);
        Obj.eulerAngles = Angle;
    }

    public static float GetMagnitude(Transform Obj, Transform Player)
    {
        float distance_x = Obj.position.x - Player.position.x;
        float distance_y = Obj.position.y - Player.position.y;
        return distance_x * distance_x + distance_y * distance_y;
    }

    public static float GetAngle(Transform obj, Transform Player)
    {
        Buffer.x = obj.position.x - Player.position.x;
        Buffer.y = obj.position.y - Player.position.y;
        ArcAngle = AtanDict.ContainsKey(Buffer) ? AtanDict[Buffer] : PushData(Buffer, Mathf.Atan2(Buffer.y, Buffer.x));
        ArcAngle *= Mathf.Rad2Deg;
        ArcAngle -= 90;
        return ArcAngle;
    }
}
