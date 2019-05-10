using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PressedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool PointerDown;
    private float PointerDownTimer;

    public float RequiredHoldTime;
    public UnityEvent OnLongClick;
    public UnityEvent OnClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDown = true;
        PointerDownTimer = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerDown = false;
        if(PointerDownTimer < RequiredHoldTime)
        {
            OnClick.Invoke();
        }
    }

    void Update()
    {
        if(PointerDown)
        {
            PointerDownTimer += Time.deltaTime;

            if(PointerDownTimer > RequiredHoldTime)
            {
                if (OnLongClick != null)
                    OnLongClick.Invoke();
            }
        }
    }
}
