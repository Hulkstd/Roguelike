using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    void Start()
    {
        using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter("Assets\\test.txt"))
        {
            float angle = 20;
            for (int i = 0; i < 20; i++)
            {
                float x = Mathf.Cos(Mathf.Deg2Rad * angle) * 250;
                float y = Mathf.Sin(Mathf.Deg2Rad * angle) * 250;

                Debug.Log(x + " " + y);
                streamWriter.WriteLine(x + " " + y);

                angle += (360 / 20);
            }
        }
    }

    
}
