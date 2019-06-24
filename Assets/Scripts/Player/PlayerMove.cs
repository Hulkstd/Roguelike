using UnityEngine;

public class PlayerMove : Character
{

    public static bool IsFight { get; set; }

    protected void Start()
    {
        Instance = this;
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (JoyStickController.Click)
        {
            Move(JoyStickController.MoveValue);
        }
#elif UNITY_STANDALONE_WIN
        Vector3 MoveVec = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(MoveVec);
#endif
        if (IsChangeState)
        {
            DoAnimation();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsFight)
        {
            if (collision.CompareTag("Door"))
            {
                // TODO Move next Map
            }
            // TOOD else
        }
        else
        {
            if (collision.CompareTag("Bullet"))
            {
                // TODO Decrease HP
            }
            // TODO else
        }
    }

}
