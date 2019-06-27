using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTracer : MonoBehaviour
{
    public GameObject followingCube;
    private float initialZPos = 0f;

    void Awake() 
    { 
        initialZPos = followingCube.transform.position.y; 
    }

    void Update () 
    {
        Camera camera = GetComponent<Camera>();
        Vector3 newPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        //followingCube.transform.position = new Vector3(newPosition.x, followingCube.transform.position.y, 0f);
        followingCube.transform.position = newPosition - Vector3.forward * newPosition.z;

        //print("X : " + newPosition.x + ", Y : " + newPosition.y); 
    } 
}
