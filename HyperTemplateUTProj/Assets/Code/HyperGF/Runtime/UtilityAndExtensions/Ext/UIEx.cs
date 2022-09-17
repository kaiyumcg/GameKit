using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIEx
{
    //todo filter by exception list of maskable graphics so that effects will not be included for them

    #region Alpha
    public static void ExSetAlpha(this MaskableGraphic graphic, float alpha)
    {
        var col = graphic.color;
        col.a = alpha;
        graphic.color = col;
    }
    public static void ExSetAlpha(this List<MaskableGraphic> graphics, float alpha, params MaskableGraphic[] exceptions)
    {
        if (graphics != null && graphics.Count > 0)
        {
            for (int i = 0; i < graphics.Count; i++)
            {
                var graphic = graphics[i];
                if (graphic == null) { continue; }
                if (exceptions != null)
                {
                    if (exceptions.ExContainsInArray(graphic)) { continue; }
                }
                graphic.ExSetAlpha(alpha);
            }
        }
    }
    #endregion

    #region Activation
    public static void ExSetActive(this List<MaskableGraphic> graphics, bool enable)
    {
        if (graphics != null && graphics.Count > 0)
        {
            for (int i = 0; i < graphics.Count; i++)
            {
                var graphic = graphics[i];
                if (graphic == null) { continue; }
                graphic.enabled = enable;
            }
        }
    }
    public static void ExSetActive(this List<MaskableGraphic> graphics, bool enable, params MaskableGraphic[] exceptions)
    {
        if (graphics != null && graphics.Count > 0)
        {
            for (int i = 0; i < graphics.Count; i++)
            {
                var graphic = graphics[i];
                if (graphic == null) { continue; }
                if (exceptions != null)
                {
                    if (exceptions.ExContainsInArray(graphic)) { continue; }
                }
                graphic.enabled = enable;
            }
        }
    }
    #endregion

    //todo we do not need two private method for to support exceptioned graphics, one is enough
    #region Fade
    public static void ExFade(this List<MaskableGraphic> graphics, float alpha, float duration,
        ref List<Tween> dtList)
    {
        _ExFade(graphics, alpha, duration, ref dtList, null, null);
    }
    public static void ExFade(this List<MaskableGraphic> graphics, float alpha, float duration,
        ref List<Tween> dtList, MonoBehaviour mono, System.Action OnComplete)
    {
        _ExFade(graphics, alpha, duration, ref dtList, OnComplete, mono);
    }
    static void _ExFade(List<MaskableGraphic> graphics, float alpha, float duration,
        ref List<Tween> dtList, System.Action OnComplete, MonoBehaviour mono)
    {
        if (graphics == null || graphics.Count < 1) { return; }

        var invalidList = false;
        for (int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            if (graphic == null)
            {
                invalidList = true;
                break;
            }
        }
        if (invalidList) { return; }

        if (dtList == null || dtList.Count != graphics.Count)
        {
            dtList = new List<Tween>();
            for (int i = 0; i < graphics.Count; i++)
            {
                dtList.Add(null);
            }
        }

        dtList.ExResetDT();

        var completedCount = 0;
        for (int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            dtList[i] = graphic.DOFade(alpha, duration).OnComplete(() =>
            {
                completedCount++;
            });
        }

        if (mono != null)
        {
            mono.StartCoroutine(COR());
        }
        IEnumerator COR()
        {
            while (true)
            {
                if (completedCount >= graphics.Count) { break; }
                yield return null;
            }
            OnComplete?.Invoke();
        }
    }
    public static void ExFade(this List<MaskableGraphic> graphics, float alpha, float duration, params MaskableGraphic[] exceptions)
    {
        _ExFade(graphics, alpha, duration, null, null, exceptions);
    }
    public static void ExFade(this List<MaskableGraphic> graphics, float alpha, float duration, MonoBehaviour mono,
        System.Action OnComplete, params MaskableGraphic[] exceptions)
    {
        _ExFade(graphics, alpha, duration, OnComplete, mono, exceptions);
    }
    static void _ExFade(List<MaskableGraphic> graphics, float alpha, float duration, System.Action OnComplete, MonoBehaviour mono, MaskableGraphic[] exceptions)
    {
        if (graphics == null || graphics.Count < 1) { return; }
        var validExceptionCount = 0;
        if (exceptions != null && exceptions.Length > 0)
        {
            for (int i = 0; i < exceptions.Length; i++)
            {
                if (exceptions[i] != null) { validExceptionCount++; }
            }
        }

        var completedCount = 0;
        for (int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            if (graphic == null) { continue; }
            if (exceptions != null)
            {
                if (exceptions.ExContainsInArray(graphic)) { continue; }
            }
            graphic.DOFade(alpha, duration).OnComplete(() =>
            {
                completedCount++;
            });
        }

        if (mono != null)
        {
            mono.StartCoroutine(COR());
        }
        IEnumerator COR()
        {
            while (true)
            {
                if (completedCount >= (graphics.Count - validExceptionCount)) { break; }
                yield return null;
            }
            OnComplete?.Invoke();
        }
    }
    #endregion

    #region Color
    public static void ExColor(this List<MaskableGraphic> graphics, Color endColor, float duration,
        ref List<TweenerCore<Color, Color, ColorOptions>> dtList)
    {
        _ExColor(graphics, endColor, duration, ref dtList, null, null);
    }
    public static void ExColor(this List<MaskableGraphic> graphics, Color endColor, float duration,
        ref List<TweenerCore<Color, Color, ColorOptions>> dtList, MonoBehaviour mono, System.Action OnComplete)
    {
        _ExColor(graphics, endColor, duration, ref dtList, OnComplete, mono);
    }
    static void _ExColor(List<MaskableGraphic> graphics, Color endColor, float duration,
        ref List<TweenerCore<Color, Color, ColorOptions>> dtList, System.Action OnComplete, MonoBehaviour mono)
    {
        if (graphics == null || graphics.Count < 1) { return; }

        var invalidList = false;
        for (int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            if (graphic == null)
            {
                invalidList = true;
                break;
            }
        }
        if (invalidList) { return; }


        if (dtList == null || dtList.Count != graphics.Count)
        {
            dtList = new List<TweenerCore<Color, Color, ColorOptions>>();
            for (int i = 0; i < graphics.Count; i++)
            {
                dtList.Add(null);
            }
        }

        dtList.ExResetDT();

        var completedCount = 0;
        for (int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            dtList[i] = graphic.DOColor(endColor, duration).OnComplete(() =>
            {
                completedCount++;
            });
        }

        if (mono != null)
        {
            mono.StartCoroutine(COR());
        }
        IEnumerator COR()
        {
            while (true)
            {
                if (completedCount >= graphics.Count) { break; }
                yield return null;
            }
            OnComplete?.Invoke();
        }
    }
    public static void ExColor(this List<MaskableGraphic> graphics, Color endColor, float duration)
    {
        _ExColor(graphics, endColor, duration, null, null);
    }
    public static void ExColor(this List<MaskableGraphic> graphics, Color endColor, float duration, MonoBehaviour mono, System.Action OnComplete)
    {
        _ExColor(graphics, endColor, duration, OnComplete, mono);
    }
    static void _ExColor(List<MaskableGraphic> graphics, Color endColor, float duration, System.Action OnComplete, MonoBehaviour mono)
    {
        if (graphics == null || graphics.Count < 1) { return; }

        var invalidList = false;
        for (int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            if (graphic == null)
            {
                invalidList = true;
                break;
            }
        }
        if (invalidList) { return; }

        var completedCount = 0;
        for (int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            if (graphic == null) { continue; }
            graphic.DOColor(endColor, duration).OnComplete(() =>
            {
                completedCount++;
            });
        }

        if (mono != null)
        {
            mono.StartCoroutine(COR());
        }
        IEnumerator COR()
        {
            while (true)
            {
                if (completedCount >= graphics.Count) { break; }
                yield return null;
            }
            OnComplete?.Invoke();
        }
    }
    #endregion

    #region FillAmount
    public static void ExFillAmount(this List<Image> images, float endAmount, float duration,
        ref List<TweenerCore<float, float, FloatOptions>> dtList)
    {
        _ExFill(images, endAmount, duration, ref dtList, null, null);
    }
    public static void ExFillAmount(this List<Image> images, float endAmount, float duration,
        ref List<TweenerCore<float, float, FloatOptions>> dtList, MonoBehaviour mono, System.Action OnComplete)
    {
        _ExFill(images, endAmount, duration, ref dtList, OnComplete, mono);
    }
    static void _ExFill(List<Image> images, float endAmount, float duration,
        ref List<TweenerCore<float, float, FloatOptions>> dtList, System.Action OnComplete, MonoBehaviour mono)
    {
        if (images == null || images.Count < 1) { return; }

        var invalidList = false;
        for (int i = 0; i < images.Count; i++)
        {
            var graphic = images[i];
            if (graphic == null)
            {
                invalidList = true;
                break;
            }
        }
        if (invalidList) { return; }


        if (dtList == null || dtList.Count != images.Count)
        {
            dtList = new List<TweenerCore<float, float, FloatOptions>>();
            for (int i = 0; i < images.Count; i++)
            {
                dtList.Add(null);
            }
        }

        dtList.ExResetDT();

        var completedCount = 0;
        for (int i = 0; i < images.Count; i++)
        {
            var img = images[i];
            dtList[i] = img.DOFillAmount(endAmount, duration).OnComplete(() =>
            {
                completedCount++;
            });
        }

        if (mono != null)
        {
            mono.StartCoroutine(COR());
        }
        IEnumerator COR()
        {
            while (true)
            {
                if (completedCount >= images.Count) { break; }
                yield return null;
            }
            OnComplete?.Invoke();
        }
    }
    public static void ExFillAmount(this List<Image> images, float endAmount, float duration)
    {
        _ExFill(images, endAmount, duration, null, null);
    }
    public static void ExFillAmount(this List<Image> images, float endAmount, float duration, MonoBehaviour mono, System.Action OnComplete)
    {
        _ExFill(images, endAmount, duration, OnComplete, mono);
    }
    static void _ExFill(List<Image> images, float endAmount, float duration, System.Action OnComplete, MonoBehaviour mono)
    {
        if (images == null || images.Count < 1) { return; }

        var invalidList = false;
        for (int i = 0; i < images.Count; i++)
        {
            var graphic = images[i];
            if (graphic == null)
            {
                invalidList = true;
                break;
            }
        }
        if (invalidList) { return; }

        var completedCount = 0;
        for (int i = 0; i < images.Count; i++)
        {
            var img = images[i];
            if (img == null) { continue; }
            img.DOFillAmount(endAmount, duration).OnComplete(() =>
            {
                completedCount++;
            });
        }

        if (mono != null)
        {
            mono.StartCoroutine(COR());
        }
        IEnumerator COR()
        {
            while (true)
            {
                if (completedCount >= images.Count) { break; }
                yield return null;
            }
            OnComplete?.Invoke();
        }
    }
    #endregion

    #region Blink
    public static Coroutine ExBlinkContinue(this MaskableGraphic graphic, MonoBehaviour scriptCaller, float cycleTime)
    {
        return _ExBlink(scriptCaller, graphic, null, cycleTime, -1f, null);
    }
    public static Coroutine ExBlinkUntil(this MaskableGraphic graphic, MonoBehaviour scriptCaller,
        float cycleTime, WhenToDoFunc stopperCondition, System.Action OnComplete = null)
    {
        return _ExBlink(scriptCaller, graphic, stopperCondition, cycleTime, -1f, OnComplete);
    }
    public static Coroutine ExBlinkUntil(this MaskableGraphic graphic, MonoBehaviour scriptCaller,
        float cycleTime, float maxTime, System.Action OnComplete = null)
    {
        return _ExBlink(scriptCaller, graphic, null, cycleTime, maxTime, OnComplete);
    }
    public static Coroutine ExBlinkUntilConditionOrTime(this MaskableGraphic graphic, MonoBehaviour scriptCaller,
        float cycleTime, float maxTime, WhenToDoFunc stopperCondition, System.Action OnComplete = null)
    {
        return _ExBlink(scriptCaller, graphic, stopperCondition, cycleTime, maxTime, OnComplete);
    }
    static Coroutine _ExBlink(MonoBehaviour mono, MaskableGraphic graphic,
        WhenToDoFunc stopperCondition, float cycleTime, float maxTime, System.Action OnComplete)
    {
        return mono.StartCoroutine(TapToStartBlink(graphic, stopperCondition, cycleTime, maxTime, OnComplete));
        IEnumerator TapToStartBlink(MaskableGraphic graphic, WhenToDoFunc stopperCondition,
            float cycleTime, float maxTime, System.Action OnComplete)
        {
            graphic.gameObject.SetActive(true);
            var timer = 0.0f;
            while (true)
            {
                if ((maxTime > 0.0f && timer > maxTime) || (stopperCondition != null && stopperCondition.Invoke()))
                {
                    graphic.ExSetAlpha(0.0f);
                    OnComplete?.Invoke();
                    yield break;
                }

                {
                    var fadeIn = false;
                    graphic.ExSetAlpha(0.0f);
                    graphic.DOFade(1.0f, cycleTime).OnComplete(() => { fadeIn = true; });
                    while (!fadeIn)
                    {
                        if (stopperCondition != null && stopperCondition.Invoke()) { OnComplete?.Invoke(); yield break; }

                        timer += Time.deltaTime;
                        yield return null;
                    }
                }

                {
                    var fadeOut = false;
                    graphic.ExSetAlpha(1.0f);
                    graphic.DOFade(0.0f, cycleTime).OnComplete(() => { fadeOut = true; });
                    while (!fadeOut)
                    {
                        if (stopperCondition != null && stopperCondition.Invoke()) { OnComplete?.Invoke(); yield break; }

                        timer += Time.deltaTime;
                        yield return null;
                    }
                }
                yield return null;
            }
        }
    }
    #endregion
}