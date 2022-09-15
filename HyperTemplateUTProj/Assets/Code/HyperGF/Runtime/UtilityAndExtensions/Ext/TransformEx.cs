using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class TransformEx
{
    //set global scale
    //foreach on each of parent chain
    

    public static List<Transform> GetImmediateChilds(this Transform transform)
    {
        var result = new List<Transform>();
        var count = transform.childCount;
        if (count == 0) { return result; }
        for (int i = 0; i < count; i++)
        {
            result.Add(transform.GetChild(i));
        }
        return result;
    }

    #region Foreach
    public static void ExForEachImmediateChilds(this Transform transform, OnDoAnything<Transform> Code)
    {
        var count = transform.childCount;
        if (count == 0) { return; }
        for (int i = 0; i < count; i++)
        {
            Code?.Invoke(transform.GetChild(i));
        }
    }
    public static async Task ExForEachImmediateChilds<T>(this Transform transform, Func<Transform, Task> Code)
    {
        var count = transform.childCount;
        if (count == 0) { return; }
        for (int i = 0; i < count; i++)
        {
            await Code?.Invoke(transform.GetChild(i));
        }
    }
    #endregion


    #region Foreach With ID
    public static void ExForEachImmediateChilds<T>(this Transform transform, OnDoAnything<Transform, int> Code)
    {
        var count = transform.childCount;
        if (count == 0) { return; }
        for (int i = 0; i < count; i++)
        {
            Code?.Invoke(transform.GetChild(i), i);
        }
    }
    public static async Task ExForEachImmediateChilds<T>(this Transform transform, Func<Transform, int, Task> Code)
    {
        var count = transform.childCount;
        if (count == 0) { return; }
        for (int i = 0; i < count; i++)
        {
            await Code?.Invoke(transform.GetChild(i), i);
        }
    }
    #endregion
}
