using UnityEngine;
using System.Collections;
using static GCManager;

public static class DropItemFunctions
{
    public static GameObject SkillPointPrefab = Resources.Load("aaa") as GameObject;
    public static GameObject EssencePrefab = Resources.Load("aaa") as GameObject;
    public static GameObject[] ArtifactPrefab = Resources.LoadAll<GameObject>("aaa");

    public static void DropSkillPoint(int Min, int Max, Vector2 pos)
    {
        int N = Random.Range(Min, Max);

        for (int i = 0; i < N; i++) 
        {
            GameObject obj = MonoBehaviour.Instantiate(SkillPointPrefab);

            StaticClassCoroutineManager.Instance.Perform(DropDown(obj, pos));
        }
    }

    public static void DropEssence(int Min, int Max, Vector2 pos)
    {
        int N = Random.Range(Min, Max);
        Debug.Log(N);

        for (int i = 0; i < N; i++)
        {
            GameObject obj = MonoBehaviour.Instantiate(EssencePrefab);

            StaticClassCoroutineManager.Instance.Perform(DropDown(obj, pos));
        }
    }

    public static void DropArtifact(DropItemType type, Vector2 pos)
    {
        GameObject obj = MonoBehaviour.Instantiate(ArtifactPrefab[(int)type - 3]);

        StaticClassCoroutineManager.Instance.Perform(DropDown(obj, pos));
    }

    static IEnumerator DropDown(GameObject gameObject, Vector2 pos)
    {
        int g = 0;
        Vector2 vec = Random.insideUnitCircle;
        float distance = Random.Range(1.5f, 3.0f);
        float height = Random.Range(1.2f, 2.5f);

        gameObject.transform.position = pos;
        vec *= distance;

        while(true)
        {
            gameObject.transform.position = Vector2.Lerp(pos, vec, g * 0.03333f);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, pos.y + Mathf.Sin(Remap(g, 0, 31, 0, Mathf.PI)) * height, 0);
            g++;

            if(g * 0.03333f >= 1)
            {
                break;
            }

            yield return CoroDict.ContainsKey(0.02f) ? CoroDict[0.02f] : PushData(0.02f, new WaitForSeconds(0.02f));
        }
    }

    private static float Remap(float value, float minValue, float maxValue, float minResult, float maxResult)
    {
        return minResult + (value - minValue) * (maxResult - minResult) / (maxValue - minValue);
    }
}
