public static class UtilityClass
{
    public static float Remap(float value, float minValue, float maxValue, float minResult, float maxResult)
    {
        return minResult + (value - minValue) * (maxValue - minValue) / (maxResult - minResult);
    }
}

