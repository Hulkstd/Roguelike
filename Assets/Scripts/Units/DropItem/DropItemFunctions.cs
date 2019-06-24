using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GCManager;

public class DropItemFunctions : MonoBehaviour
{
    public static DropItemFunctions Instance;
    public static GameObject SkillPointPrefab = Resources.Load("aaa") as GameObject;
    public static GameObject EssencePrefab = Resources.Load("aaa") as GameObject;
    public static GameObject[] ArtifactPrefab = Resources.LoadAll<GameObject>("aaa");

    void Awake()
    {
        Instance = this;
    }

    public void DropSkillPoint(int Min, int Max, Vector2 pos)
    {
        int N = Random.Range(Min, Max);

        for (int i = 0; i < N; i++) 
        {
            GameObject obj = Instantiate(SkillPointPrefab);

            StartCoroutine(DropDown(obj, pos));
        }
    }

    public void DropEssence(int Min, int Max, Vector2 pos)
    {
        int N = Random.Range(Min, Max);

        for (int i = 0; i < N; i++)
        {
            GameObject obj = Instantiate(EssencePrefab);

            StartCoroutine(DropDown(obj, pos));
        }
    }

    public void DropArtifact(DropItemType type, Vector2 pos)
    {
        GameObject obj = Instantiate(ArtifactPrefab[(int)type - 3]);

        StartCoroutine(DropDown(obj, pos));
    }

    IEnumerator DropDown(GameObject gameObject, Vector2 pos)
    {
        int g = 0;
        Vector2 vec = Random.insideUnitCircle;
        float distance = Random.Range(0.3f, 1.5f);

        gameObject.transform.position = pos;
        vec *= distance;

        while(true)
        {
            gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, vec, g * 0.03333f);
            gameObject.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y + Mathf.Sin(g * 0.03333f), 0);
            g++;

            if(g * 0.03333f >= 1)
            {
                break;
            }

            yield return CoroDict.ContainsKey(0.02f) ? CoroDict[0.02f] : PushData(0.02f, new WaitForSeconds(0.02f));
        }
    }
}
