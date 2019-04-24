using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    #region Variables
    #endregion

    #region Functions

    #region Public
    public void SceneChangeWithLoadingScene(string SceneName)
    {
        StartCoroutine(ChangeScene(SceneName));
    }

    public void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    #endregion

    #region Private
    private IEnumerator ChangeScene(string SceneName)
    {
        SceneManager.LoadScene("Loading");

        AsyncOperation async = SceneManager.LoadSceneAsync(SceneName);

        yield return new WaitUntil(() => 
        {
            if(async.isDone)
            {
                return CreateMap.IsRoading;
            }

            return false;
        });

        yield return null;
    }
    #endregion

    #endregion
}
