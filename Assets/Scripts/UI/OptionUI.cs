using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
    [SerializeField]
    private Transform PauseTransform;

    private bool isPause;

    public bool IsPause
    {
        get
        {
            return isPause;
        }

        set
        {
            isPause = value;

            if(isPause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    private void Start()
    {
        IsPause = false;
    }

    public void PauseButton()
    {
        IsPause = !IsPause;

        PauseTransform.gameObject.SetActive(IsPause);
    }
}
