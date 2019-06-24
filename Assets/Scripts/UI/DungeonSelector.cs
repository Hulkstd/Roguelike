using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DungeonSelector : MonoBehaviour, IPointerClickHandler
{
    public string DungeonName;
    public string DungeonDescription;
    public Sprite DungeonSprite;

    public Transform LetterBox;
    public Vector2 pivot;

    private GameObject DungeonUI;

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
    }
}