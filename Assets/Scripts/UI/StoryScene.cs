using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static GCMannager;

public class StoryScene : MonoBehaviour
{
    private SceneChanger SceneChangerInstance
    {
        get
        {
            return SceneChanger.Instance;
        }
    }

    public PostProcessProfile processProfile;
    public Sprite[] sprites = new Sprite[11];
    public string[] story = new string[11];
    public CanvasGroup Alpha;
    public UnityEngine.UI.Image SceneImage;
    public UnityEngine.UI.Text StoryText;

    bool IsImage2 = false;
    bool IsImage5 = false;
    Coroutine ie = null;

    private void Start()
    {
        StartCoroutine(StoryRoutine());
    }

    private void Update()
    {
        if(IsImage2 || IsImage5)
        {
            if(IsImage2)
            {
                ie = StartCoroutine(ProfileColorLerp(new Color(0, 0, 1), new Color(0, 1, 1), 0.2f));
                IsImage2 = false;
            }
            else if(IsImage5)
            {
                ie = StartCoroutine(ProfileColorLerp(new Color(0, 0, 1), new Color(0, 1, 1), 0.2f));
                IsImage5 = false;
            }
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopAllCoroutines();
            SceneChangerInstance.SceneChange("MainMenu");
        }
    }

    IEnumerator StoryRoutine()
    {
        processProfile.GetSetting<Bloom>().active = true;

        if (processProfile.HasSettings<Bloom>())
        {
            processProfile.GetSetting<Bloom>().color.value = new Color(1, 0.5f, 0.5f);
        }
        yield return StartCoroutine(ChangeScneeImage(0));


        if(processProfile.HasSettings<Bloom>())
        {
            IsImage2 = true;
        }
        yield return StartCoroutine(ChangeScneeImage(1));

        StopCoroutine(ie);
        processProfile.GetSetting<Bloom>().active = false;

        yield return StartCoroutine(ChangeScneeImage(2));


        yield return StartCoroutine(ChangeScneeImage(3));

        processProfile.GetSetting<Bloom>().active = true;

        if (processProfile.HasSettings<Bloom>())
        {
            IsImage5 = true;
        }

        yield return StartCoroutine(ChangeScneeImage(4));

        StopCoroutine(ie);
        processProfile.GetSetting<Bloom>().active = false;

        yield return StartCoroutine(ChangeScneeImage(5));


        yield return StartCoroutine(ChangeScneeImage(6));


        yield return StartCoroutine(ChangeScneeImage(7));


        yield return StartCoroutine(ChangeScneeImage(8));


        yield return StartCoroutine(ChangeScneeImage(9));


        yield return StartCoroutine(ChangeScneeImage(10));

        SceneChangerInstance.SceneChange("MainMenu");
    }

    IEnumerator ChangeScneeImage(int index)
    {
        SceneImage.sprite = sprites[index];
        StoryText.text = story[index];

        yield return StartCoroutine(FadeInImage());

        yield return CoroDict.ContainsKey(0.35f) ? CoroDict[0.35f] : PushData(0.35f, new WaitForSeconds(0.35f));

        yield return StartCoroutine(FadeOutImage());
    }

    IEnumerator FadeOutImage()
    {
        float i = 0;

        while (Alpha.alpha != 0)
        {
            i++;
            Alpha.alpha = Mathf.Lerp(1, 0, i / 25);
            yield return CoroDict.ContainsKey(0.04f) ? CoroDict[0.04f] : PushData(0.04f, new WaitForSeconds(0.04f)); 
        }

        yield return null;
    }

    IEnumerator FadeInImage()
    {
        float i = 0;

        while (Alpha.alpha != 1)
        {
            i++;
            Alpha.alpha = Mathf.Lerp(0, 1, i / 25);
            yield return CoroDict.ContainsKey(0.04f) ? CoroDict[0.04f] : PushData(0.04f, new WaitForSeconds(0.04f));
        }

        yield return null;
    }

    IEnumerator ProfileColorLerp(Color a, Color b, float time)
    {
        Color From = a;
        Color To = b;
        float i = 0;

        while(true)
        {
            yield return CoroDict.ContainsKey(0.02f) ? CoroDict[0.02f] : PushData(0.02f, new WaitForSeconds(0.02f));
            i++;
            processProfile.GetSetting<Bloom>().color.value = Color.Lerp(From, To, i / 25);

            if(processProfile.GetSetting<Bloom>().color.value == To)
            {
                To = (To == a ? b : a);
                From = (From == a ? b : a);
                i = 0;
            }
        }
    }
}
