﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCMannager;

public class RandomRotation : MonoBehaviour
{
    public Vector2 MinMaxTime;
    public Vector2 DelayTimeRange;

    void Start()
    {
        StartCoroutine(Rotator());
    }

    IEnumerator Rotator()
    {
        Quaternion quaternion;
        float Time;

        while (true)
        {
            quaternion = Random.rotation;
            quaternion.x = 0;
            quaternion.y = 0;
            Time = Random.Range(MinMaxTime.x, MinMaxTime.y);

            while(true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, Time);

                if (Mathf.Abs(transform.rotation.eulerAngles.z - quaternion.eulerAngles.z) <= 5f)
                    break;

                yield return null;
            }

            yield return CoroDict.ContainsKey(Random.Range(DelayTimeRange.x, DelayTimeRange.y)) ? 
                                              CoroDict[Random.Range(DelayTimeRange.x, DelayTimeRange.y)] : 
                                              PushData(Random.Range(DelayTimeRange.x, DelayTimeRange.y), 
                                                       new WaitForSeconds(Random.Range(DelayTimeRange.x, DelayTimeRange.y)));
        }
    }
}
