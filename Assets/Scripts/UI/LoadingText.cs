using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

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
                yield return CoroDict.ContainsKey(0.5f) ? CoroDict[0.5f] : PushData(0.5f, new WaitForSeconds(0.5f));

                text.text = "Creating Map . .";
                yield return CoroDict.ContainsKey(0.5f) ? CoroDict[0.5f] : PushData(0.5f, new WaitForSeconds(0.5f));

                text.text = "Creating Map . . .";
                yield return CoroDict.ContainsKey(0.5f) ? CoroDict[0.5f] : PushData(0.5f, new WaitForSeconds(0.5f));

                if (CreateMap.IsRoading)
                {
                    break;
                }
            }
            else
            {
                text.text = "Loading .";
                yield return CoroDict.ContainsKey(0.5f) ? CoroDict[0.5f] : PushData(0.5f, new WaitForSeconds(0.5f));

                text.text = "Loading . .";
                yield return CoroDict.ContainsKey(0.5f) ? CoroDict[0.5f] : PushData(0.5f, new WaitForSeconds(0.5f));

                text.text = "Loading . . .";
                yield return CoroDict.ContainsKey(0.5f) ? CoroDict[0.5f] : PushData(0.5f, new WaitForSeconds(0.5f));
            }

        }

    }
}
