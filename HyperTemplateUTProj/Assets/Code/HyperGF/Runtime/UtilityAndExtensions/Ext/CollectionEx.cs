using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public static class CollectionEx
{
    #region Foreach
    public static void ExForEach<T>(this List<T> list, OnDoAnything<T> Code)
    {
        if (list.ExIsValid() == false) { return; }
        for (int i = 0; i < list.Count; i++)
        {
            var l = list[i];
            Code?.Invoke(l);
        }
    }
    public static async Task ExForEach<T>(this List<T> list, Func<T, Task> Code)
    {
        if (list.ExIsValid() == false) { return; }
        for (int i = 0; i < list.Count; i++)
        {
            var l = list[i];
            await Code?.Invoke(l);
        }
    }
    public static void ExForEach_NoCheck<T>(this List<T> list, OnDoAnything<T> Code)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var l = list[i];
            Code.Invoke(l);
        }
    }
    public static async Task ExForEach_NoCheck<T>(this List<T> list, Func<T, Task> Code)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var l = list[i];
            await Code.Invoke(l);
        }
    }
    public static void ExForEach_NoCheck<T>(this List<T> list, int len, OnDoAnything<T> Code)
    {
        for (int i = 0; i < len; i++)
        {
            var l = list[i];
            Code.Invoke(l);
        }
    }
    public static async Task ExForEach_NoCheck<T>(this List<T> list, int len, Func<T, Task> Code)
    {
        for (int i = 0; i < len; i++)
        {
            var l = list[i];
            await Code.Invoke(l);
        }
    }
    public static void ExForEach<T>(this T[] list, OnDoAnything<T> Code)
    {
        if (list.ExIsValid() == false) { return; }
        for (int i = 0; i < list.Length; i++)
        {
            var l = list[i];
            Code?.Invoke(l);
        }
    }
    public static async Task ExForEach<T>(this T[] list, Func<T, Task> Code)
    {
        if (list.ExIsValid() == false) { return; }
        for (int i = 0; i < list.Length; i++)
        {
            var l = list[i];
            await Code?.Invoke(l);
        }
    }
    public static void ExForEach_NoCheck<T>(this T[] list, OnDoAnything<T> Code)
    {
        for (int i = 0; i < list.Length; i++)
        {
            var l = list[i];
            Code.Invoke(l);
        }
    }
    public static async Task ExForEach_NoCheck<T>(this T[] list, Func<T, Task> Code)
    {
        for (int i = 0; i < list.Length; i++)
        {
            var l = list[i];
            await Code.Invoke(l);
        }
    }
    public static void ExForEach_NoCheck<T>(this T[] list, int len, OnDoAnything<T> Code)
    {
        for (int i = 0; i < len; i++)
        {
            var l = list[i];
            Code.Invoke(l);
        }
    }
    public static async Task ExForEach_NoCheck<T>(this T[] list, int len, Func<T, Task> Code)
    {
        for (int i = 0; i < len; i++)
        {
            var l = list[i];
            await Code.Invoke(l);
        }
    }
    #endregion


    #region Foreach With ID
    public static void ExForEach<T>(this List<T> list, OnDoAnything<T, int> Code)
    {
        if (list.ExIsValid() == false) { return; }
        for (int i = 0; i < list.Count; i++)
        {
            var l = list[i];
            Code?.Invoke(l, i);
        }
    }
    public static async Task ExForEach<T>(this List<T> list, Func<T, int, Task> Code)
    {
        if (list.ExIsValid() == false) { return; }
        for (int i = 0; i < list.Count; i++)
        {
            var l = list[i];
            await Code?.Invoke(l, i);
        }
    }
    public static void ExForEach_NoCheck<T>(this List<T> list, OnDoAnything<T, int> Code)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var l = list[i];
            Code.Invoke(l, i);
        }
    }
    public static async Task ExForEach_NoCheck<T>(this List<T> list, Func<T, int, Task> Code)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var l = list[i];
            await Code.Invoke(l, i);
        }
    }
    public static void ExForEach_NoCheck<T>(this List<T> list, int len, OnDoAnything<T, int> Code)
    {
        for (int i = 0; i < len; i++)
        {
            var l = list[i];
            Code.Invoke(l, i);
        }
    }
    public static async Task ExForEach_NoCheck<T>(this List<T> list, int len, Func<T, int, Task> Code)
    {
        for (int i = 0; i < len; i++)
        {
            var l = list[i];
            await Code.Invoke(l, i);
        }
    }
    public static void ExForEach<T>(this T[] list, OnDoAnything<T, int> Code)
    {
        if (list.ExIsValid() == false) { return; }
        for (int i = 0; i < list.Length; i++)
        {
            var l = list[i];
            Code?.Invoke(l, i);
        }
    }
    public static async Task ExForEach<T>(this T[] list, Func<T, int, Task> Code)
    {
        if (list.ExIsValid() == false) { return; }
        for (int i = 0; i < list.Length; i++)
        {
            var l = list[i];
            await Code?.Invoke(l, i);
        }
    }
    public static void ExForEach_NoCheck<T>(this T[] list, OnDoAnything<T, int> Code)
    {
        for (int i = 0; i < list.Length; i++)
        {
            var l = list[i];
            Code.Invoke(l, i);
        }
    }
    public static async Task ExForEach_NoCheck<T>(this T[] list, Func<T, int, Task> Code)
    {
        for (int i = 0; i < list.Length; i++)
        {
            var l = list[i];
            await Code.Invoke(l, i);
        }
    }
    public static void ExForEach_NoCheck<T>(this T[] list, int len, OnDoAnything<T, int> Code)
    {
        for (int i = 0; i < len; i++)
        {
            var l = list[i];
            Code.Invoke(l, i);
        }
    }
    public static async Task ExForEach_NoCheck<T>(this T[] list, int len, Func<T, int, Task> Code)
    {
        for (int i = 0; i < len; i++)
        {
            var l = list[i];
            await Code.Invoke(l, i);
        }
    }
    #endregion

    //todo this method should be "this List<T>" argumented and also returns to avoid writing CollectionEx
    public static List<T> GetListWithCount<T>(int count)
    {
        var result = new List<T>();
        for (int i = 0; i < count; i++)
        {
            result.Add(default);
        }
        return result;
    }

    //todo HasAnyNull
    //todo Get NotNullCount
    //todo ForEach from reverse direction of all method support


    public static bool ExIsValid<T>(this T[] array) { return array != null && array.Length > 0; }
    public static bool ExIsValid<T>(this List<T> list) { return list != null && list.Count > 0; }
    public static List<T> ExRemoveNulls<T>(this List<T> list) where T : class
    {
        var result = new List<T>();
        if (list != null && list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var l = list[i];
                if (l == null) { continue; }
                result.Add(l);
            }
        }
        return result;
    }
    public static bool ExContainsInArray<T>(this T[] array, T item) where T : class
    {
        bool contains = false;
        if (array != null && array.Length > 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                var it = array[i];
                if (it == item)
                {
                    contains = true;
                    break;
                }
            }
        }
        return contains;
    }
    public static bool ExContainsOptimized<T>(this List<T> lst, T t) where T : class
    {
        var contains = false;
        if (lst != null)
        {
            var len = lst.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    var it = lst[i];
                    if (ReferenceEquals(it, t))
                    {
                        contains = true;
                        break;
                    }
                }
            }
        }
        return contains;
    }
}