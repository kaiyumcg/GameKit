using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public static class GameExtension
{
    public static bool IsValid<T>(this T[] array) { return array != null && array.Length > 0; }
    public static bool IsValid<T>(this List<T> list) { return list != null && list.Count > 0; }

    public static bool HasCrossed(this Vector3 thisPt, Vector3 point, Vector3 moveDirection)
    {
        var toPointDirection = point - thisPt;
        return Vector3.Dot(toPointDirection.normalized, moveDirection.normalized) < 0.0f;
    }

    public static void OptimizeSkinnedRenderersInside(this Transform tr, bool shadow, bool occlusionCulling)
    {
        var rnds1 = tr.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (rnds1 != null && rnds1.Length > 0)
        {
            for (int j = 0; j < rnds1.Length; j++)
            {
                var rn = rnds1[j];
                if (rn == null) { continue; }
                rn.shadowCastingMode = shadow ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
                rn.receiveShadows = shadow;
                rn.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                rn.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                rn.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                rn.allowOcclusionWhenDynamic = occlusionCulling;
                rn.quality = SkinQuality.Bone1;
                rn.skinnedMotionVectors = false;
            }
        }
    }

    public static void OptimizeMeshRenderersInside(this Transform tr, bool shadow, bool occlusionCulling)
    {
        var rnds = tr.GetComponentsInChildren<MeshRenderer>();
        if (rnds != null && rnds.Length > 0)
        {
            for (int i = 0; i < rnds.Length; i++)
            {
                var rn = rnds[i];
                if (rn == null) { continue; }
                rn.shadowCastingMode = shadow ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
                rn.receiveShadows = shadow;
                rn.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                rn.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                rn.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                rn.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                rn.allowOcclusionWhenDynamic = occlusionCulling;
            }
        }
    }

    public static void Optimize(this SkinnedMeshRenderer renderer, bool shadow, bool occlusionCulling)
    {
        renderer.shadowCastingMode = shadow ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = shadow;
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        renderer.allowOcclusionWhenDynamic = occlusionCulling;
        renderer.quality = SkinQuality.Bone1;
        renderer.skinnedMotionVectors = false;
    }

    public static void Optimize(this MeshRenderer renderer, bool shadow, bool occlusionCulling)
    {
        renderer.shadowCastingMode = shadow ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = shadow;
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        renderer.allowOcclusionWhenDynamic = occlusionCulling;
    }

    public static bool IsBetween(this float a, float min, float max)
    {
        return a >= min && a < max;
    }

    public static bool IsAround(this float a, float b, float tolerence = 0.01f)
    {
        return a > b - tolerence && a < b + tolerence;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static void ResetDT(this TweenerCore<float, float, FloatOptions> dt)
    {
        if (dt != null && dt.IsActive()) { dt.Kill(); }
    }

    public static void ResetDT(this TweenerCore<Vector3, Vector3, VectorOptions> dt)
    {
        if (dt != null && dt.IsActive()) { dt.Kill(); }
    }

    public static void ResetDT(this TweenerCore<Color, Color, ColorOptions> dt)
    {
        if (dt != null && dt.IsActive()) { dt.Kill(); }
    }

    public static void ResetDT(this List<TweenerCore<float, float, FloatOptions>> dtList)
    {
        if (dtList != null && dtList.Count > 0)
        {
            for (int i = 0; i < dtList.Count; i++)
            {
                var dt = dtList[i];
                if (dt == null) { continue; }
                dt.ResetDT();
            }
        }
    }

    public static void ResetDT(this List<TweenerCore<Vector3, Vector3, VectorOptions>> dtList)
    {
        if (dtList != null && dtList.Count > 0)
        {
            for (int i = 0; i < dtList.Count; i++)
            {
                var dt = dtList[i];
                if (dt == null) { continue; }
                dt.ResetDT();
            }
        }
    }

    public static void ResetDT(this List<TweenerCore<Color, Color, ColorOptions>> dtList)
    {
        if (dtList != null && dtList.Count > 0)
        {
            for (int i = 0; i < dtList.Count; i++)
            {
                var dt = dtList[i];
                if (dt == null) { continue; }
                dt.ResetDT();
            }
        }
    }

    public static void SetActive(this List<MaskableGraphic> graphics, bool enable)
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

    public static void DoFade(this List<MaskableGraphic> graphics, float alpha, float duration,
        ref List<TweenerCore<Color, Color, ColorOptions>> dtList)
    {
        _DoFade(graphics, alpha, duration, ref dtList, null, null);
    }

    public static void DoFade(this List<MaskableGraphic> graphics, float alpha, float duration,
        ref List<TweenerCore<Color, Color, ColorOptions>> dtList, MonoBehaviour mono, System.Action OnComplete)
    {
        _DoFade(graphics, alpha, duration, ref dtList, OnComplete, mono);
    }

    static void _DoFade(List<MaskableGraphic> graphics, float alpha, float duration, 
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

        dtList.ResetDT();

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

    public static void DoFade(this List<MaskableGraphic> graphics, float alpha, float duration)
    {
        _DoFade(graphics, alpha, duration, null, null);
    }

    public static void DoFade(this List<MaskableGraphic> graphics, float alpha, float duration, MonoBehaviour mono, System.Action OnComplete)
    {
        _DoFade(graphics, alpha, duration, OnComplete, mono);
    }

    static void _DoFade(List<MaskableGraphic> graphics, float alpha, float duration, System.Action OnComplete, MonoBehaviour mono)
    {
        if (graphics == null || graphics.Count < 1) { return; }

        var completedCount = 0;
        for (int i = 0; i < graphics.Count; i++)
        {
            var graphic = graphics[i];
            if (graphic == null) { continue; }
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
                if (completedCount >= graphics.Count) { break; }
                yield return null;
            }
            OnComplete?.Invoke();
        }
    }

    public static void DoColor(this List<MaskableGraphic> graphics, Color endColor, float duration,
        ref List<TweenerCore<Color, Color, ColorOptions>> dtList)
    {
        _DoColor(graphics, endColor, duration, ref dtList, null, null);
    }

    public static void DoColor(this List<MaskableGraphic> graphics, Color endColor, float duration,
        ref List<TweenerCore<Color, Color, ColorOptions>> dtList, MonoBehaviour mono, System.Action OnComplete)
    {
        _DoColor(graphics, endColor, duration, ref dtList, OnComplete, mono);
    }

    static void _DoColor(List<MaskableGraphic> graphics, Color endColor, float duration,
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

        dtList.ResetDT();

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

    public static void DoColor(this List<MaskableGraphic> graphics, Color endColor, float duration)
    {
        _DoColor(graphics, endColor, duration, null, null);
    }

    public static void DoColor(this List<MaskableGraphic> graphics, Color endColor, float duration, MonoBehaviour mono, System.Action OnComplete)
    {
        _DoColor(graphics, endColor, duration, OnComplete, mono);
    }

    static void _DoColor(List<MaskableGraphic> graphics, Color endColor, float duration, System.Action OnComplete, MonoBehaviour mono)
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


    //Fill
    public static void DoFillAmount(this List<Image> images, float endAmount, float duration,
        ref List<TweenerCore<float, float, FloatOptions>> dtList)
    {
        _DoFill(images, endAmount, duration, ref dtList, null, null);
    }

    public static void DoFillAmount(this List<Image> images, float endAmount, float duration,
        ref List<TweenerCore<float, float, FloatOptions>> dtList, MonoBehaviour mono, System.Action OnComplete)
    {
        _DoFill(images, endAmount, duration, ref dtList, OnComplete, mono);
    }

    static void _DoFill(List<Image> images, float endAmount, float duration,
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

        dtList.ResetDT();

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

    public static void DoFillAmount(this List<Image> images, float endAmount, float duration)
    {
        _DoFill(images, endAmount, duration, null, null);
    }

    public static void DoFillAmount(this List<Image> images, float endAmount, float duration, MonoBehaviour mono, System.Action OnComplete)
    {
        _DoFill(images, endAmount, duration, OnComplete, mono);
    }

    static void _DoFill(List<Image> images, float endAmount, float duration, System.Action OnComplete, MonoBehaviour mono)
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

    public static Coroutine DoBlinkContinue(this MaskableGraphic graphic, MonoBehaviour scriptCaller, float cycleTime)
    {
        return _Blink(scriptCaller, graphic, null, cycleTime, -1f, null);
    }

    public static Coroutine DoBlinkUntil(this MaskableGraphic graphic, MonoBehaviour scriptCaller,
        float cycleTime, WhenToDoFunc stopperCondition, System.Action OnComplete = null)
    {
        return _Blink(scriptCaller, graphic, stopperCondition, cycleTime, -1f, OnComplete);
    }

    public static Coroutine DoBlinkUntil(this MaskableGraphic graphic, MonoBehaviour scriptCaller,
        float cycleTime, float maxTime, System.Action OnComplete = null)
    {
        return _Blink(scriptCaller, graphic, null, cycleTime, maxTime, OnComplete);
    }

    public static Coroutine DoBlinkUntilConditionOrTime(this MaskableGraphic graphic, MonoBehaviour scriptCaller,
        float cycleTime, float maxTime, WhenToDoFunc stopperCondition, System.Action OnComplete = null)
    {
        return _Blink(scriptCaller, graphic, stopperCondition, cycleTime, maxTime, OnComplete);
    }

    static Coroutine _Blink(MonoBehaviour mono, MaskableGraphic graphic, 
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
                    graphic.SetAlpha(0.0f);
                    OnComplete?.Invoke();
                    yield break;
                }

                {
                    var fadeIn = false;
                    graphic.SetAlpha(0.0f);
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
                    graphic.SetAlpha(1.0f);
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

    public static Coroutine DoScaleUpDownContinue(this Transform transform, MonoBehaviour scriptCaller, float from, float to, float cycleTime)
    {
        return _ScaleUpDown(transform, scriptCaller, null, cycleTime, from, to, -1f, null);
    }

    public static Coroutine DoScaleUpDownUntil(this Transform transform, MonoBehaviour scriptCaller,
         float from, float to, float cycleTime, WhenToDoFunc stopperCondition, System.Action OnComplete = null)
    {
        return _ScaleUpDown(transform, scriptCaller, stopperCondition, cycleTime, from, to, -1f, OnComplete);
    }

    public static Coroutine DoScaleUpDownUntil(this Transform transform, MonoBehaviour scriptCaller,
         float from, float to, float cycleTime, float maxTime, System.Action OnComplete = null)
    {
        return _ScaleUpDown(transform, scriptCaller, null, cycleTime, from, to, maxTime, OnComplete);
    }

    public static Coroutine DoScaleUpDownUntilConditionOrTime(this Transform transform, MonoBehaviour scriptCaller, float cycleTime, float from, float to,
        float maxTime, WhenToDoFunc stopperCondition, System.Action OnComplete = null)
    {
        return _ScaleUpDown(transform, scriptCaller, stopperCondition, cycleTime, from, to, maxTime, OnComplete);
    }

    static Coroutine _ScaleUpDown(Transform transform, MonoBehaviour scriptCaller, WhenToDoFunc stopperCondition,
        float cycleTime, float from, float to, float maxTime, System.Action OnComplete)
    {
        return scriptCaller.StartCoroutine(ScaleUpDown(transform, stopperCondition, cycleTime, from, to, maxTime, OnComplete));
        IEnumerator ScaleUpDown(Transform tr, WhenToDoFunc stopperCondition,
            float cycleTime, float from, float to, float maxTime, System.Action OnComplete)
        {
            tr.gameObject.SetActive(true);
            var originalScale = tr.localScale;
            var fromScale = originalScale * (from);
            var toScale = originalScale * (to);
            var timer = 0.0f;
            while (true)
            {
                if ((maxTime > 0.0f && timer > maxTime) || (stopperCondition != null && stopperCondition.Invoke()))
                {
                    tr.localScale = originalScale;
                    OnComplete?.Invoke();
                    yield break;
                }

                {
                    var fadeIn = false;
                    tr.DOScale(toScale, cycleTime).OnComplete(() => { fadeIn = true; });
                    while (!fadeIn)
                    {
                        if (stopperCondition != null && stopperCondition.Invoke()) { OnComplete?.Invoke(); yield break; }

                        timer += Time.deltaTime;
                        yield return null;
                    }
                }

                {
                    var fadeOut = false;
                    tr.DOScale(fromScale, cycleTime).OnComplete(() => { fadeOut = true; });
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

    public static void SetAlpha(this MaskableGraphic graphic, float alpha)
    {
        var col = graphic.color;
        col.a = alpha;
        graphic.color = col;
    }

    public static void SetAlpha(this List<MaskableGraphic> graphics, float alpha)
    {
        if (graphics != null && graphics.Count > 0)
        {
            for (int i = 0; i < graphics.Count; i++)
            {
                var graphic = graphics[i];
                if (graphic == null) { continue; }
                graphic.SetAlpha(alpha);
            }
        }
    }

    public static void SetActiveObjects(this List<GameObject> objectLists, bool active)
    {
        if (objectLists != null && objectLists.Count > 0)
        {
            for (int i = 0; i < objectLists.Count; i++)
            {
                var obj = objectLists[i];
                if (obj == null) { continue; }
                if (obj.activeInHierarchy == active) { continue; }
                obj.SetActive(active);
            }
        }
    }

    public static bool ContainsOptimized<T>(this List<T> lst, T t) where T : class
    {
        var contains = false;
        if(lst != null)
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

    public static Vector3 RandomNavmeshLocation(this Transform transform, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public static Vector3 SamplePositionOnNavMesh(this Transform agent, Vector3 direction)
    {
        Vector3 sourcePosition = direction;
        sourcePosition += agent.transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = agent.position + direction.normalized * 2f;
        if (NavMesh.SamplePosition(sourcePosition, out hit, 10f, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public static void PlayParticles(this ParticleSystem[] sysList)
    {
        if (sysList != null && sysList.Length > 0)
        {
            for (int i = 0; i < sysList.Length; i++)
            {
                var sys = sysList[i];
                if (sys == null) { continue; }
                if (sys.isPlaying)
                {
                    sys.Stop();
                }
                sys.Play();
            }
        }
    }

    public static void PlayParticles(this List<ParticleSystem> sysList)
    {
        if (sysList != null && sysList.Count > 0)
        {
            for (int i = 0; i < sysList.Count; i++)
            {
                var sys = sysList[i];
                if (sys == null) { continue; }
                if (sys.isPlaying)
                {
                    sys.Stop();
                }
                sys.Play();
            }
        }
    }

    public static void StopParticles(this ParticleSystem[] sysList)
    {
        if (sysList != null && sysList.Length > 0)
        {
            for (int i = 0; i < sysList.Length; i++)
            {
                var sys = sysList[i];
                if (sys == null) { continue; }
                if (sys.isPlaying)
                {
                    sys.Stop();
                }
            }
        }
    }

    public static void StopParticles(this List<ParticleSystem> sysList)
    {
        if (sysList != null && sysList.Count > 0)
        {
            for (int i = 0; i < sysList.Count; i++)
            {
                var sys = sysList[i];
                if (sys == null) { continue; }
                if (sys.isPlaying)
                {
                    sys.Stop();
                }
            }
        }
    }

    public static void InitParticles(this List<GameParticle> particles)
    {
        if (particles != null && particles.Count > 0)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                var eff = particles[i];
                if (eff == null) { continue; }
                eff.Init();
            }
        }
    }

    public static void InitParticles(this GameParticle[] particles)
    {
        if (particles != null && particles.Length > 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                var eff = particles[i];
                if (eff == null) { continue; }
                eff.Init();
            }
        }
    }

    public static void PlayParticles(this List<GameParticle> particles)
    {
        if (particles != null && particles.Count > 0)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                var eff = particles[i];
                if (eff == null) { continue; }
                eff.Play();
            }
        }
    }

    public static void PlayParticles(this GameParticle[] particles)
    {
        if (particles != null && particles.Length > 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                var eff = particles[i];
                if (eff == null) { continue; }
                eff.Play();
            }
        }
    }

    public static void StopParticles(this List<GameParticle> particles)
    {
        if (particles != null && particles.Count > 0)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                var eff = particles[i];
                if (eff == null) { continue; }
                eff.Stop();
            }
        }
    }

    public static void StopParticles(this GameParticle[] particles)
    {
        if (particles != null && particles.Length > 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                var eff = particles[i];
                if (eff == null) { continue; }
                eff.Stop();
            }
        }
    }

    public static void PlayParticlesSerially(this List<GameParticle> particles, MonoBehaviour handle, float interval)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            var waiter = new WaitForSeconds(interval);
            if (particles != null && particles.Count > 0)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    yield return waiter;
                }
            }
        }
    }

    public static void PlayParticlesSerially(this GameParticle[] particles, MonoBehaviour handle, float interval)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            var waiter = new WaitForSeconds(interval);
            if (particles != null && particles.Length > 0)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    yield return waiter;
                }
            }
        }
    }

    public static void StopParticlesSerially(this List<GameParticle> particles, MonoBehaviour handle, float interval)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            var waiter = new WaitForSeconds(interval);
            if (particles != null && particles.Count > 0)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Stop();
                    yield return waiter;
                }
            }
        }
    }

    public static void StopParticlesSerially(this GameParticle[] particles, MonoBehaviour handle, float interval)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            var waiter = new WaitForSeconds(interval);
            if (particles != null && particles.Length > 0)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Stop();
                    yield return waiter;
                }
            }
        }
    }

    public static void PlayParticlesSerially(this List<GameParticle> particles, MonoBehaviour handle, List<float> intervals)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            if (particles != null && particles.Count > 0)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    if (i == particles.Count - 1) { continue; }
                    yield return new WaitForSeconds(intervals[i]);
                }
            }
        }
    }

    public static void PlayParticlesSerially(this List<GameParticle> particles, MonoBehaviour handle, float[] intervals)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            if (particles != null && particles.Count > 0)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    if (i == particles.Count - 1) { continue; }
                    yield return new WaitForSeconds(intervals[i]);
                }
            }
        }
    }

    public static void PlayParticlesSerially(this GameParticle[] particles, MonoBehaviour handle, List<float> intervals)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            if (particles != null && particles.Length > 0)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    if (i == particles.Length - 1) { continue; }
                    yield return new WaitForSeconds(intervals[i]);
                }
            }
        }
    }

    public static void PlayParticlesSerially(this GameParticle[] particles, MonoBehaviour handle, float[] intervals)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            if (particles != null && particles.Length > 0)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    if (i == particles.Length - 1) { continue; }
                    yield return new WaitForSeconds(intervals[i]);
                }
            }
        }
    }

    public static void StopParticlesSerially(this List<GameParticle> particles, MonoBehaviour handle, List<float> intervals)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            if (particles != null && particles.Count > 0)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Stop();
                    if (i == particles.Count - 1) { continue; }
                    yield return new WaitForSeconds(intervals[i]);
                }
            }
        }
    }

    public static void StopParticlesSerially(this List<GameParticle> particles, MonoBehaviour handle, float[] intervals)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            if (particles != null && particles.Count > 0)
            {
                for (int i = 0; i < particles.Count; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    if (i == particles.Count - 1) { continue; }
                    yield return new WaitForSeconds(intervals[i]);
                }
            }
        }
    }

    public static void StopParticlesSerially(this GameParticle[] particles, MonoBehaviour handle, List<float> intervals)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            if (particles != null && particles.Length > 0)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    if (i == particles.Length - 1) { continue; }
                    yield return new WaitForSeconds(intervals[i]);
                }
            }
        }
    }

    public static void StopParticlesSerially(this GameParticle[] particles, MonoBehaviour handle, float[] intervals)
    {
        handle.StartCoroutine(COR());
        IEnumerator COR()
        {
            if (particles != null && particles.Length > 0)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    var eff = particles[i];
                    if (eff == null) { continue; }
                    eff.Play();
                    if (i == particles.Length - 1) { continue; }
                    yield return new WaitForSeconds(intervals[i]);
                }
            }
        }
    }
}