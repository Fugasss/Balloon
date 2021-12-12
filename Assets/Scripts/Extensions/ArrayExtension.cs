using UnityEngine;

namespace Extensions
{
    public static class ArrayExtension
    {
        public static T GetRandom<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }
    }
}