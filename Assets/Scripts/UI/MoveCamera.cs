using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public class MoveCamera : MonoBehaviour
{
    public static MoveCamera Instance;

    public Camera MainCamera;
    public GameObject InfoBox;

    void Awake()
    {
        Instance = this;
    }

    public void MoveBack()
    {
        StartCoroutine(Move_Camera(5, Vector3.zero, false));
    }

    public IEnumerator Move_Camera(float Size, Vector3 position, bool InfoboxState)
    {
        int i = 1;
        float originalSize = MainCamera.orthographicSize;
        Vector3 originalPosition = MainCamera.transform.position;
        position.z = -10;

        if (!InfoboxState) InfoBox.SetActive(InfoboxState);

        for (; i <= 30; i++)
        {
            MainCamera.orthographicSize = Mathf.Lerp(MainCamera.orthographicSize, Size, 0.3f);
            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, position, 0.3f);

            yield return CoroDict.ContainsKey(0.0233f) ? CoroDict[0.0233f] : PushData(0.0233f, new WaitForSeconds(0.0233f));
        }

        MainCamera.orthographicSize = Size;
        MainCamera.transform.position = position;
        InfoBox.SetActive(InfoboxState);
    }
}
