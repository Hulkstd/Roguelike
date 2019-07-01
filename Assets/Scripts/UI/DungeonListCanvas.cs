using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonListCanvas : MonoBehaviour
{
    public Text Title;

    public void SetTitle(string text)
    {
        Title.text = text;
    }
}
