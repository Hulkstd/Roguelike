using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GCManager;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[AddComponentMenu("GameManager/SceneChanger")]
public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    public string DungeonName;
    public Image image;

    private Color[] alphaColor = new Color[30];

    #region Start Setting

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        GetComponent<Canvas>().sortingOrder = 100;

        GameObject obj = new GameObject("Image", typeof(Image));
        obj.transform.SetParent(transform);
        image = obj.GetComponent<Image>();
        image.color = Color.black;
        image.rectTransform.localPosition = Vector3.zero;
        image.rectTransform.localScale = Vector2.one * 10;
        image.gameObject.SetActive(false);

        alphaColor[0] = new Color(0, 0, 0, 0);
        for (int i = 1; i < 30; i++) 
        {
            alphaColor[i] = new Color(0, 0, 0, 0 / i);
        }
    }

    #endregion

    #region Variables
    #endregion

    #region Functions

    #region Public

    public void SceneChangeWithLoadingScene(string SceneName, string DungeonName)
    {
        this.DungeonName = DungeonName;
        StartCoroutine(ChangeScene(SceneName));
    }

    public void SceneChange(string SceneName)
    {

        SceneManager.LoadScene(SceneName);
    }

    #endregion

    #region Private

    private IEnumerator FadeIn()
    {
        image.gameObject.SetActive(true);
        for (int i = 0; i < 30; i++) 
        {
            image.color = alphaColor[i];

            yield return CoroDict.ContainsKey(0.0166f) ? CoroDict[0.0166f] : PushData(0.0166f, new WaitForSeconds(0.0166f));
        }
        image.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut()
    {
        image.gameObject.SetActive(true);
        for (int i = 29; i >= 0; i--)
        {
            image.color = alphaColor[i];

            yield return CoroDict.ContainsKey(0.0166f) ? CoroDict[0.0166f] : PushData(0.0166f, new WaitForSeconds(0.0166f));
        }
        image.gameObject.SetActive(false);
    }

    private IEnumerator ChangeScene(string SceneName)
    {
        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(SceneName);

        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator ChangeSceneWithLoadingScene(string SceneName)
    {
        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene("Loading");
        GameObject.FindGameObjectWithTag("Title").GetComponent<Text>().text = DungeonName + "Area";

        AsyncOperation async = SceneManager.LoadSceneAsync(SceneName);

        yield return new WaitUntil(() => 
        {
            if(async.isDone)
            {
                return CreateMap.IsRoading;
            }

            return false;
        });

        yield return StartCoroutine(FadeIn());

        yield return null;
    }

    #endregion

    #endregion
}
