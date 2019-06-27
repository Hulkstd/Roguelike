using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCheaker : MonoBehaviour
{
    //protected bool letPlay = true;
    public ParticleSystem followingparticle;

    private ObjectPooling Particles;

    void Start()
    {
        Particles = new ObjectPooling(followingparticle.gameObject);
    }

    public void Update()
    {
        Camera camera = GetComponent<Camera>();
        Vector3 newPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

        if (Input.GetMouseButtonDown(0))
        {
            followingparticle = Particles.PopObject().GetComponent<ParticleSystem>();
            followingparticle.gameObject.SetActive(true);

            followingparticle.transform.position = newPosition - Vector3.forward * newPosition.z;
            followingparticle.Play();

            StartCoroutine(UtilityClass.Disable(followingparticle.gameObject, 1.0f));
        }
    }
}
