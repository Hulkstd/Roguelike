using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCManager : MonoBehaviour
{
    public struct FloatPair
    {
        public float x;
        public float y;
    }
    
    public static Dictionary<FloatPair, float> AtanDict { get; private set; }
    public static Dictionary<float, WaitForSeconds> CoroDict { get; private set; }
    public static WaitForEndOfFrame CoroWaitForEndFrame { get; private set; }
    public static WaitForFixedUpdate CoroWaitForFixedUpdate { get; private set; }
    public static Dictionary<string, Transform> TransformDictionary { get; private set; }

    public static float PushData(FloatPair key, float value)
    {
        AtanDict.Add(key, value);
        return value;
    }

    public static Transform PushData(string key, Transform value)
    {
        TransformDictionary.Add(key, value);
        return value;
    }

    public static WaitForSeconds PushData(float key, WaitForSeconds value)
    {
        CoroDict.Add(key, value);
        return value;
    }

    private void Awake()
    {
        AtanDict = new Dictionary<FloatPair, float>();
        CoroDict = new Dictionary<float, WaitForSeconds>();
        CoroWaitForEndFrame = new WaitForEndOfFrame();
        CoroWaitForFixedUpdate = new WaitForFixedUpdate();
        TransformDictionary = new Dictionary<string, Transform>();
    }
}
