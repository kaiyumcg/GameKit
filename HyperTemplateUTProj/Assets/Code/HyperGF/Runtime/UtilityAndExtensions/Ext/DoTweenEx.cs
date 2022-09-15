using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DoTweenEx
{
    //todo other type implement?
    public static void ExResetDT(this TweenerCore<float, float, FloatOptions> dt)
    {
        if (dt != null && dt.IsActive()) { dt.Kill(); }
    }

    public static void ExResetDT(this Tween dt)
    {
        if (dt != null && dt.IsActive()) { dt.Kill(); }
    }

    public static void ExResetDT(this TweenerCore<Vector3, Vector3, VectorOptions> dt)
    {
        if (dt != null && dt.IsActive()) { dt.Kill(); }
    }

    public static void ExResetDT(this TweenerCore<Color, Color, ColorOptions> dt)
    {
        if (dt != null && dt.IsActive()) { dt.Kill(); }
    }

    public static bool ExIsValidDT(this TweenerCore<float, float, FloatOptions> dt)
    {
        return dt != null && dt.IsActive();
    }

    public static bool ExIsValidDT(this Tween dt)
    {
        return dt != null && dt.IsActive();
    }

    public static bool ExIsValidDT(this TweenerCore<Vector3, Vector3, VectorOptions> dt)
    {
        return dt != null && dt.IsActive();
    }

    public static bool ExIsValidDT(this TweenerCore<Color, Color, ColorOptions> dt)
    {
        return dt != null && dt.IsActive();
    }

    public static void ExResetDT(this List<TweenerCore<float, float, FloatOptions>> dtList)
    {
        if (dtList != null && dtList.Count > 0)
        {
            for (int i = 0; i < dtList.Count; i++)
            {
                var dt = dtList[i];
                if (dt == null) { continue; }
                dt.ExResetDT();
            }
        }
    }

    public static void ExResetDT(this List<Tween> dtList)
    {
        if (dtList != null && dtList.Count > 0)
        {
            for (int i = 0; i < dtList.Count; i++)
            {
                var dt = dtList[i];
                if (dt == null) { continue; }
                dt.ExResetDT();
            }
        }
    }

    public static void ExResetDT(this List<TweenerCore<Vector3, Vector3, VectorOptions>> dtList)
    {
        if (dtList != null && dtList.Count > 0)
        {
            for (int i = 0; i < dtList.Count; i++)
            {
                var dt = dtList[i];
                if (dt == null) { continue; }
                dt.ExResetDT();
            }
        }
    }

    public static void ExResetDT(this List<TweenerCore<Color, Color, ColorOptions>> dtList)
    {
        if (dtList != null && dtList.Count > 0)
        {
            for (int i = 0; i < dtList.Count; i++)
            {
                var dt = dtList[i];
                if (dt == null) { continue; }
                dt.ExResetDT();
            }
        }
    }
    public static void ExResetDT(this List<Tweener> twList)
    {
        if (twList != null && twList.Count > 0)
        {
            for (int i = 0; i < twList.Count; i++)
            {
                var tw = twList[i];
                if (tw == null) { continue; }
                tw.ExResetDT();
            }
        }
    }
    public static void ExResetDT(this Tweener dt)
    {
        if (dt != null && dt.IsActive()) { dt.Kill(); }
    }
}