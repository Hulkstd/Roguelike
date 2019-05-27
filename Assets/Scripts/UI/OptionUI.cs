using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseTransform;
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private Button PauseButton;
    [SerializeField]
    private GameObject Option;
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

    public void PressPause()
    {
        IsPause = !IsPause;

        PauseButton.interactable = IsPause;
        PauseTransform.SetActive(IsPause);
        Panel.SetActive(IsPause);
    }

    public void OptionButton()
    {
        Option.gameObject.SetActive(true);
    }

    public void Continue()
    {
        IsPause = !IsPause;

        PauseButton.interactable = IsPause;
        PauseTransform.SetActive(IsPause);
        Panel.SetActive(IsPause);
    }

    public void MenuButton()
    {
        SceneChanger.Instance.SceneChange("이동할곳");  
    }
}
