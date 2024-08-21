using System.Collections.Generic;
using Random = UnityEngine.Random;
using System;

namespace GB.Extensions
{
    public static class CollectionExtensions
    {
        public static T PickRandom<T>(this T[] source)
        {
            int randomIndex = Random.Range(0, source.Length);
            if (randomIndex >= source.Length)
                return default(T);

            return source[randomIndex];
        }

        public static T PickRandom<T>(this List<T> source)
        {
            int randomIndex = Random.Range(0, source.Count);
            if (randomIndex >= source.Count)
                return default(T);

            return source[randomIndex];
        }

        public static List<T> Shuffle<T>(this List<T> source)
        {
            List<T> result = new List<T>(source);

            for(int i = 0; i < result.Count; i++)
            {
                int index = Random.Range(i, source.Count);
                T temp = result[i];

                result[i] = result[index];
                result[index] = temp;
            }

            return result;
        }

        public static Dictionary<Type, T> GetDictionaryByType<T>(this T[] source)
        {
            Dictionary<Type, T> dictionary = new Dictionary<Type, T>();
            for (int i = 0; i < source.Length; ++i)
            {
                Type type = source[i].GetType();
                if (dictionary.ContainsKey(type))
                    continue;

                dictionary.Add(type, source[i]);
            }

            return dictionary;
        }

        public static void ForEach<T>(this T[] source, Action<T> callback)
        {
            for (int i = 0; i < source.Length; ++i)
                callback?.Invoke(source[i]);
        }

        public static void ForEach<T>(this T[] source, Action<T, int> callback)
        {
            for (int i = 0; i < source.Length; ++i)
                callback?.Invoke(source[i], i);
        }
    }
}