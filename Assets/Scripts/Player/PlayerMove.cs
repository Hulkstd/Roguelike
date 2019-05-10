using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Character
{

    public static bool IsFight { get; set; }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (JoyStickController.Click)
        {
            PlayerRigidbody.AddForce(JoyStickController.MoveValue, ForceMode2D.Force);
        }

        if (IsChangeState)
        {
            DoAnimation();
        }

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsFight)
        {
            if (collision.CompareTag("Door"))
            {
                // TODO Move next Map
            }
        }
    }

}
