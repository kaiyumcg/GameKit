using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTweener : MonoBehaviour
{
    [SerializeField] bool customScale = false;
    [SerializeField] float forceInitScaleAmount = 1.0f;
    TweenerCore<Vector3, Vector3, VectorOptions> scaleUpTw = null, scaleDownTw = null;

    public void StartEffect()
    {
        StartCoroutine(TapToStartTweenPlay());
    }

    public void StopEffect()
    {
        StopAllCoroutines();
        transform.localScale = customScale ? Vector3.one * forceInitScaleAmount : Vector3.one;
        if (scaleUpTw != null && scaleUpTw.IsActive()) { scaleUpTw.Kill(); scaleUpTw = null; }
        if (scaleDownTw != null && scaleDownTw.IsActive()) { scaleDownTw.Kill(); scaleDownTw = null; }
    }

    IEnumerator TapToStartTweenPlay()
    {
        var tr = transform;
        var tweenSize = 0.02f;
        var tweenDuration = 0.4f;
        while (true)
        {
            if (scaleUpTw != null && scaleUpTw.IsActive()) { scaleUpTw.Kill(); scaleUpTw = null; }
            if (scaleDownTw != null && scaleDownTw.IsActive()) { scaleDownTw.Kill(); scaleDownTw = null; }
            scaleUpTw = null;
            scaleDownTw = null;

            var scaleUp = false;
            var sc = customScale ? Vector3.one * forceInitScaleAmount : Vector3.one;
            scaleUpTw = tr.DOScale(sc * (1f + tweenSize), tweenDuration).OnComplete(() =>
            {
                scaleUp = true;
            }).SetEase(Ease.Linear);
            while (scaleUp == false)
            {
                yield return null;
            }
            var scaleDown = false;

            scaleDownTw = tr.DOScale(sc * (1f - tweenSize), tweenDuration).OnComplete(() =>
            {
                scaleDown = true;
            }).SetEase(Ease.Linear);
            while (scaleDown == false)
            {
                yield return null;
            }
            scaleUpTw = null;
            scaleDownTw = null;
            yield return null;
        }
    }

    private void OnDisable()
    {
        StopEffect();
    }
}
