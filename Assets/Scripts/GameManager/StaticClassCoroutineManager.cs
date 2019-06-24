using UnityEngine;
using System.Collections;

public class StaticClassCoroutineManager : MonoBehaviour
{
    public static StaticClassCoroutineManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Perform(IEnumerator Coroutine, out Coroutine enumerator)
    {
        enumerator = StartCoroutine(Coroutine);
    }

    public void Perform(IEnumerator Coroutine)
    {
        StartCoroutine(Coroutine);
    }
}
