using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public abstract class UIAnimationAsset : ScriptableObject
{
    [SerializeField] List<string> tags;
    internal List<string> Tags { get { return tags; } }
    protected internal abstract void Play(UIAnimation script, ref UIAnimHandle handle, Action OnComplete);
    internal void Stop(UIAnimation script, UIAnimHandle handle)
    {
        handle.Stop(script);
        handle.Flush();
    }
    internal async Task StopAsync(UIAnimation script, UIAnimHandle handle, Action OnComplete)
    {
        await handle.StopAsync(script);
        handle.Flush();
        OnComplete?.Invoke();
    }
}