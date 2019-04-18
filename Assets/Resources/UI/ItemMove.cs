using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ItemMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool MouseDown = false;
    private Vector3 OriginalPosition;
    public delegate bool WhenPointerUp(Transform transform, ItemMove image);
    public WhenPointerUp Activity = null;
    public UnityEngine.UI.Image Image;
    public EquipmentsObject Item;

    public void OnPointerDown(PointerEventData eventData)
    {
        MouseDown = true;
        OriginalPosition = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MouseDown = false;
        if (!Activity(transform, this))
        {
            transform.position = OriginalPosition;
        }
        else
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        if(MouseDown)
        {
            transform.position = Input.mousePosition;
        }
    }
}
