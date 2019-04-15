using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static JoyStickController Instance { get; private set; }
    public static Vector3 MoveValue;
    public static bool Click;

    private Vector3 OriginalPosition;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OriginalPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Click = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;

        Click = false;
    }

    void Update()
    {
        if(Click)
        {
            MoveValue = Input.mousePosition - OriginalPosition;

            transform.position = OriginalPosition + Vector3.ClampMagnitude(MoveValue , 90);
        }
    }

}
