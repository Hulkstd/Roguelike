using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public Vector2 MinMaxValue = new Vector2(-0.5f, 2.5f);
    public Material thisMaterial;
    public float T;
    private float time; 

    private void Start()
    {
        thisMaterial = GetComponent<Renderer>().material;
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;

        thisMaterial.SetFloat("Vector1_64E6D321", Mathf.Clamp(MinMaxValue.y * time / T, MinMaxValue.x, MinMaxValue.y));
        thisMaterial.SetVector("Vector2_34ECAC6E", new Vector4(transform.position.x, transform.position.y));

        if(MinMaxValue.y * time / T > MinMaxValue.y)
        {
            time = 0;
        }
    }
}
