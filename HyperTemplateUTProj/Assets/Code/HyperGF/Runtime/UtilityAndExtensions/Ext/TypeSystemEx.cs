using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TypeSystemEx
{
    public static List<T> ExConvertTo<T>(this List<MonoBehaviour> scripts) where T : class
    {
        var result = new List<T>();
        if (scripts != null && scripts.Count > 0)
        {
            for (int i = 0; i < scripts.Count; i++)
            {
                var nt = scripts[i];
                if (nt == null) { continue; }
                var it = nt as T;
                if (it == null) { continue; }
                result.Add(it);
            }
        }
        return result;
    }
    public static List<T> ExConvertTo<T>(this MonoBehaviour[] scripts) where T : class
    {
        var result = new List<T>();
        if (scripts != null && scripts.Length > 0)
        {
            for (int i = 0; i < scripts.Length; i++)
            {
                var nt = scripts[i];
                if (nt == null) { continue; }
                var it = nt as T;
                if (it == null) { continue; }
                result.Add(it);
            }
        }
        return result;
    }
}