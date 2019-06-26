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
        material.SetFloat("Vector1_64E6D321", UtilityClass.Remap(value, 0, 30, -6.5f, 23.0f));
        value++;

        if(value == 30)
        {
            value = 0; 
        }
    }

    public void SetOrigin(Vector2 pos)
    {
        material.SetVector("Vector2_34ECAC6E", pos);
    }
}
