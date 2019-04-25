using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RandomRo());
    }

    IEnumerator RandomRo()
    {
        Quaternion quaternion;
        float Time;

        while (true)
        {
            quaternion = Random.rotation;
            quaternion.x = 0;
            quaternion.y = 0;
            Time = Random.Range(0, 0.1f);

            while(true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, Time);

                if (Mathf.Abs(transform.rotation.eulerAngles.z - quaternion.eulerAngles.z) <= 5f)
                    break;

                yield return null;
            }
        }
    }
}
