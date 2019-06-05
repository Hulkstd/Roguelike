using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTestChangeRocation : MonoBehaviour
{
    public float AddValue;

    private void Start()
    {
        AddValue = 1f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + AddValue);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - AddValue);
        }
        if (Input.GetKey(KeyCode.L))
        {
            LookAtPlayer.LookPlayer(transform);
        }
    }
}
