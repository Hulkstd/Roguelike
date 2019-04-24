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
            if(CreateMap.Instance != null)
            {
                text.text = "Creating Map .";
                yield return new WaitForSeconds(1.0f);

                text.text = "Creating Map . .";
                yield return new WaitForSeconds(1.0f);

                text.text = "Creating Map . . .";
                yield return new WaitForSeconds(1.0f);

                if(CreateMap.IsRoading)
                {
                    break;
                }
            }
            else
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
}
