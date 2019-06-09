using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTestCode : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
        }
    }

}
