using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurseMaterial : MonoBehaviour
{
    private Material material;
    private int value;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        material.SetFloat("Vector1_64E6D321", UtilityClass.Remap(value, 0, 30 * transform.lossyScale.x, -6.5f, 23.0f * transform.lossyScale.x));
        material.SetVector("Vector2_34ECAC6E", transform.position);
        value++;

        if(value >= 30 * transform.lossyScale.x)
        {
            value = 0; 
        }
    }
}
