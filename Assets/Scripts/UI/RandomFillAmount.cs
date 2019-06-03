using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCMannager;

public class RandomFillAmount : MonoBehaviour
{
    public Vector2 MinMaxTime;
    public Vector2 MinMaxFillAmount;

    void Start()
    {
        StartCoroutine(FillAmount());
    }

    IEnumerator FillAmount()
    {
        float Amount;
        float Time;
        float TimePercent;
        UnityEngine.UI.Image Image = GetComponent<UnityEngine.UI.Image>();
        Image.fillAmount = Random.Range(MinMaxFillAmount.x, MinMaxFillAmount.y);
        float Original;

        while (true)
        {
            Amount = Random.Range(MinMaxFillAmount.x, MinMaxFillAmount.y);
            Time = Random.Range(MinMaxTime.x, MinMaxTime.y);
            TimePercent = 0;
            Original = Image.fillAmount;

            while (true)
            {
                TimePercent += UnityEngine.Time.deltaTime;

                Image.fillAmount = Mathf.Lerp(Original, Amount, TimePercent / Time);

                if (Mathf.Abs(Image.fillAmount - Amount) <= 0.05f)
                    break;

                yield return CoroWaitForEndFrame;
            }

            yield return null;
        }
    }
}
