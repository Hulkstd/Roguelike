using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DungeonSelector : MonoBehaviour, IPointerClickHandler
{
    public string DungeonName;
    public string DungeonDescription;
    public Sprite[] DungeonImages;
    public string[] Titles;
    public string[] Descriptions;
    public string[] Infos;
    public Sprite DungeonSprite;

    public Transform LetterBox;
    public Vector2 pivot;

    private GameObject DungeonUI;
    private List<float> SizeValues;
    private Vector3 offset = new Vector3(2, -0.8f, 0);
    private MoveCamera MoveCameraInstance
    {
        get
        {
            return MoveCamera.Instance;
        }
    }

    void Start()
    {
        Initialize();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MoveCameraInstance.MainCamera.orthographicSize == 5)
        {
            StartCoroutine(MoveCameraInstance.Move_Camera(1.5f, transform.position + offset, true, 1, DungeonName, DungeonImages, Titles, Descriptions, Infos));
        }
    }

    private void Initialize()
    {

    }
}