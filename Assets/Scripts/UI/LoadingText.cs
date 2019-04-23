using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    private UnityEngine.UI.Text text;

    void Start()
    {
        text = GetComponent<UnityEngine.UI.Text>();

        StartCoroutine(ChangeText());
    }

    IEnumerator ChangeText()
    {
        while(true)
        {
            text.text = "Loading .";
            yield return new WaitForSeconds(1.0f);

            text.text = "Loading . .";
            yield return new WaitForSeconds(1.0f);

            text.text = "Loading . . .";
            yield return new WaitForSeconds(1.0f);
        }
    }
}
