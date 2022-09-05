using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class KLog
{
    public const bool Enabled = true;

    public static void Print(string msg, Color color = default)
    {
        if (KLog.Enabled == false) { return; }
        string ltxt = msg;
        if (color != default)
        {
            ltxt = string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(color.r * 255f),
                (byte)(color.g * 255f), (byte)(color.b * 255f), msg);
            Debug.Log(ltxt);
        }
        else
        {
            Debug.Log(ltxt);
        }
    }

    public static void PrintError(string msg)
    {
        if (KLog.Enabled == false) { return; }
        Debug.LogError(msg);
    }

    public static void PrintWarning(string msg)
    {
        if (KLog.Enabled == false) { return; }
        Debug.LogWarning(msg);
    }

    public static void PrintException(Exception exception)
    {
        if (KLog.Enabled == false) { return; }
        Debug.LogException(exception);
    }

    public static void PrintException(Exception exception, UnityEngine.Object objectContext)
    {
        if (KLog.Enabled == false) { return; }
        Debug.LogException(exception, objectContext);
    }

    public static void Check(System.Action OnCheckCode)
    {
        if (KLog.Enabled == false) { OnCheckCode?.Invoke(); return; }
        try
        {
            OnCheckCode?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}