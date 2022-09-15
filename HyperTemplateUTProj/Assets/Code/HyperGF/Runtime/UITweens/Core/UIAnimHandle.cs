using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;
using System.Threading.Tasks;

public sealed class UIAnimHandle
{
    List<Coroutine> coroutineHandles = new List<Coroutine>();
    List<Tween> tweenHandles = new List<Tween>();
    public void AddHandle(Coroutine handle)
    {
        if (coroutineHandles == null) { coroutineHandles = new List<Coroutine>(); }
        coroutineHandles.Add(handle);
    }
    public void AddHandle(Tween tween)
    {
        if (tweenHandles == null) { tweenHandles = new List<Tween>(); }
        tweenHandles.Add(tween);
    }
    internal void Stop(UIAnimation script)
    {
        coroutineHandles.ExForEach((t) => { script.StopCoroutine(t); });
        tweenHandles.ExForEach((t) => { t.Kill(); });
    }
    internal async Task StopAsync(UIAnimation script)
    {
        coroutineHandles.ExForEach((t) => { script.StopCoroutine(t); });
        await tweenHandles.ExForEach(async (t) =>
        {
            await t.AsyncWaitForKill();
        });
    }
    internal void Flush() { coroutineHandles = null; tweenHandles = null; } 
}