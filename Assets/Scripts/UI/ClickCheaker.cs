using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCheaker : MonoBehaviour
{
    //protected bool letPlay = true;
    public ParticleSystem followingparticle;

    public void Update()
    {
        Camera camera = GetComponent<Camera>();
        Vector3 newPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

        if (Input.GetMouseButtonDown(0))
        {
            followingparticle.transform.position = newPosition - Vector3.forward * newPosition.z;
            followingparticle.Play();
        }
        else
        {
            followingparticle.Stop();
        }
    }
}
