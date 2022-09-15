using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;

public class UpdownTween : LevelObjectBehaviour
{
    [SerializeField] float upwardAmount = 1.2f;
    [SerializeField] float cycleDuration = 0.6f;
    Transform tr;
    Vector3 initLocalPos;
    protected internal override void OnAwake()
    {
        tr = transform;
        StartCoroutine(COR());
        IEnumerator COR()
        {
            initLocalPos = tr.localPosition;
            while (true)
            {
                if (behaviourEnabled == false) { yield return null; }
                var upDone = false;
                var upTarget = initLocalPos + tr.up * upwardAmount;
                tr.DOLocalMove(upTarget, cycleDuration).OnComplete(() =>
                {
                    upDone = true;
                });
                while (!upDone) { yield return null; }

                var normalDone = false;
                tr.DOLocalMove(initLocalPos, cycleDuration).OnComplete(() =>
                {
                    normalDone = true;
                });
                while (!normalDone) { yield return null; }
                yield return null;
            }
        }
    }
}
