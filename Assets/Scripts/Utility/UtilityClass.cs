using static GCManager;

public static class UtilityClass
{
    public static float Remap(float value, float minValue, float maxValue, float minResult, float maxResult)
    {
        return minResult + (value - minValue) * (maxValue - minValue) / (maxResult - minResult);
    }

    public static System.Collections.IEnumerator Disable(UnityEngine.GameObject gameObject, float t)
    {
        yield return CoroDict.ContainsKey(t) ? CoroDict[t] : PushData(t, new UnityEngine.WaitForSeconds(t));

        gameObject.SetActive(false);
    }
}

