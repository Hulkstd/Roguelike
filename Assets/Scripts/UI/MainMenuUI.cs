using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private SceneChanger SceneChangerInstance
    {
        get
        {
            return SceneChanger.Instance;
        }
    }

    public GameObject OptionTransform;

    public void EnterToGame()
    {
        SceneChangerInstance.SceneChange("StageSelectScene");
    }

    public void OpenOption()
    {
        OptionTransform.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
