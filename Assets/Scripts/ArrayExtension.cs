public static class ArrayExtension
{
    public static T GetRandom<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }
}