using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;

public abstract class SpeedorBooster : LevelObjectBehaviour
{
    [Header("Visual Tween")]
    [SerializeField] Transform visualTarget;
    [SerializeField] float tweenHeight = 2f;
    [SerializeField] float tweenWithin = 1.4f;

    [SerializeField] float tweenScale = 2f;
    [SerializeField] float tweenScaleWithin = 1.4f;

    [SerializeField] bool useScaleTween = true, useUpdownTween = false;

    [Header("Speed Setting")]
    [SerializeField] float modifier = 1.5f;
    [SerializeField] float raiseDuration = 0.4f, duration = 4f, normalizeDuration = 0.5f;

    TweenerCore<float, float, FloatOptions> speedModRaiseTween = null, speedModNormalizeTween = null;
    float factor = 1.0f;
    TweenerCore<Vector3, Vector3, VectorOptions> tween = null, scaleTween = null;
    bool done = false, speedActive = false;
    protected internal override void OnAwake()
    {
        factor = 1.0f;
    }
    protected virtual void OnStartComponent() { }
    protected virtual void OnEnterSpeedBooster() { }
    protected virtual void OnTickSpeedBooster() { }
    protected virtual void OnDestroyComponent() { }
    protected abstract Type DefinePlayerScriptType();
    protected abstract bool IsPlayerDeadOrDeathStarted();
    protected abstract void SetPlayerSpeedFactor(float speedFactor);

    IEnumerator Start()
    {
        yield return null;
        OnStartComponent();
        speedActive = false;
        done = false;
        if (useScaleTween)
        {
            StartCoroutine(ScaleTween());
        }
        if (useUpdownTween)
        {
            StartCoroutine(UpdownTween());
        }

        IEnumerator ScaleTween()
        {
            var initScale = visualTarget.localScale;
            var isScaleUp = false;
            while (true)
            {
                var tScale = isScaleUp ? (initScale * tweenScale) : initScale;
                var done = false;
                if (scaleTween != null && scaleTween.IsActive()) { scaleTween.Kill(); scaleTween = null; }
                scaleTween = visualTarget.DOScale(tScale, tweenScaleWithin).OnComplete(() =>
                {
                    done = true;
                    isScaleUp = !isScaleUp;
                });
                while (!done) { yield return null; }
                yield return null;
            }
        }

        IEnumerator UpdownTween()
        {
            var initPos = visualTarget.position;
            var isGoingUp = false;
            while (true)
            {
                var tPos = isGoingUp ? (initPos + visualTarget.up * tweenHeight) : initPos;
                var done = false;
                if (tween != null && tween.IsActive()) { tween.Kill(); tween = null; }
                tween = visualTarget.DOMove(tPos, tweenWithin).OnComplete(() =>
                {
                    done = true;
                    isGoingUp = !isGoingUp;
                });
                while (!done) { yield return null; }
                yield return null;
            }
        }
    }

    void OnDestroy()
    {
        OnDestroyComponent();
    }
    
    void OnTriggerEnter(Collider other)
    {
        var pl = other.GetComponentInParent(DefinePlayerScriptType());
        if (pl == null) { return; }
        if (done) { return; }
        done = true;
        StopAllCoroutines();
        StartCoroutine(Starter(true));

        IEnumerator Starter(bool hapticOn)
        {
            speedActive = true;
            OnEnterSpeedBooster();
            OnEnterSpeedModifier(modifier, raiseDuration, duration, normalizeDuration, () =>
            {
                speedActive = false;
                //hide particle
            });
            yield return null;

            while (speedActive && IsPlayerDeadOrDeathStarted() == false)
            {
                OnTickSpeedBooster();
                yield return null;
            }
        }

        void OnEnterSpeedModifier(float modifier, float raiseWithin, float duration, float normalizeWithin, System.Action OnDone)
        {
            if (IsPlayerDeadOrDeathStarted()) { return; }
            if (speedModRaiseTween != null && speedModRaiseTween.IsActive()) { speedModRaiseTween.Kill(); speedModRaiseTween = null; }
            if (speedModNormalizeTween != null && speedModNormalizeTween.IsActive()) { speedModNormalizeTween.Kill(); speedModNormalizeTween = null; }

            StartCoroutine(SpeedorCOR());
            IEnumerator SpeedorCOR()
            {
                var doneSpeedUp = false;
                speedModRaiseTween = DOTween.To(() => factor, x =>
                {
                    factor = x;
                    SetPlayerSpeedFactor(x);
                }, modifier, raiseWithin);
                speedModRaiseTween.OnComplete(() =>
                {
                    doneSpeedUp = true;

                });
                while (!doneSpeedUp) { yield return null; }

                yield return new WaitForSeconds(duration);

                var doneSpeedNormal = false;
                if (speedModNormalizeTween != null && speedModNormalizeTween.IsActive()) { speedModNormalizeTween.Kill(); speedModNormalizeTween = null; }
                speedModNormalizeTween = DOTween.To(() => factor, x =>
                {
                    factor = x;
                    SetPlayerSpeedFactor(x);
                }, 1.0f, normalizeWithin);
                speedModNormalizeTween.OnComplete(() =>
                {
                    doneSpeedNormal = true;
                });
                while (!doneSpeedNormal) { yield return null; }
                OnDone?.Invoke();
            }
        }
    }
}