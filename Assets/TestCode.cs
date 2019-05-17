using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public Vector2 MinMaxValue = new Vector2(-0.5f, 2.5f);
    public Material thisMaterial;

    private float time; 

    private void Start()
    {
        thisMaterial = GetComponent<Renderer>().material;
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;

        thisMaterial.SetFloat("Vector1_64E6D321", Mathf.Clamp(MinMaxValue.y * time / 0.3f, MinMaxValue.x, MinMaxValue.y));

        if(MinMaxValue.y * time / 0.3f > MinMaxValue.y)
        {
            time = 0;
        }
    }
}
