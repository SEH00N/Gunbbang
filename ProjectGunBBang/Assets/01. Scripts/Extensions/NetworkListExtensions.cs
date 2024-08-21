using System;
using Unity.Netcode;
using Random = UnityEngine.Random;

namespace GB.Extensions
{
public static class NetworkListExtensions
{
	public static void ChangeData<T>(this NetworkList<T> source, Func<T, bool> predicator, Func<T, T> process) where T : unmanaged, IEquatable<T>
    {
        int index = source.Find(out T data, predicator);
        if (index == -1)
            return;

        T changedData = process.Invoke(data);
        source[index] = changedData;
        source.SetDirty(true);
    }

    public static void ChangeAllData<T>(this NetworkList<T> source, Func<T, T> process) where T : unmanaged, IEquatable<T>
    {
        source.ForEach((i, index) => source[index] = process.Invoke(i));
    }

    public static void ForEach<T>(this NetworkList<T> source, Action<T> callback) where T : unmanaged, IEquatable<T>
    {
        for (int i = 0; i < source.Count; ++i)
            callback.Invoke(source[i]);
    }

    public static void ForEach<T>(this NetworkList<T> source, Action<T, int> callback) where T : unmanaged, IEquatable<T>
    {
        for (int i = 0; i < source.Count; ++i)
            callback.Invoke(source[i], i);
    }
    
    public static int FindIndex<T>(this NetworkList<T> source, Func<T, bool> predicator) where T : unmanaged, IEquatable<T>
    {
        for(int i = 0; i < source.Count; ++i)
            if(predicator.Invoke(source[i]))
                return i;

        return -1;
    }

    public static int Find<T>(this NetworkList<T> source, out T found, Func<T, bool> predicator) where T : unmanaged, IEquatable<T>
    {
        found = default(T);
        for(int i = 0; i < source.Count; ++i)
        {
            if(predicator.Invoke(source[i]))
            {
                found = source[i];
                return i;
            }
        }

        return -1;
    }

    public static T PickRandom<T>(this NetworkList<T> source, Func<T, bool> condition = null) where T : unmanaged, IEquatable<T>
    {
        int randIndex = 0;
        do
            randIndex = Random.Range(0, source.Count);
        while (condition != null && condition?.Invoke(source[randIndex]) == false);
        
        return source[randIndex];
    }
}
}