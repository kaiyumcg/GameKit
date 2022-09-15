using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class UICoin : MonoBehaviour
{
    Transform root, thisTr;
    Vector3 moveDir;
    Vector3 initPos;
    GameObject gObj;
    TweenerCore<Vector3, Vector3, VectorOptions> tw1 = null, tw2 = null;
    Coin2DParticle coin2DParticle = null;

    public void Init(Coin2DParticle coin2DParticle, Transform coin2DParticleTransform)
    {
        this.coin2DParticle = coin2DParticle;
        thisTr = transform;
        gObj = gameObject;
        initPos = thisTr.position;
        root = coin2DParticleTransform;
        moveDir = thisTr.position - root.position;
        moveDir = moveDir.normalized;
        thisTr.position = root.position;
        gObj.SetActive(false);
    }

    public void StartParticle(float within, float distance, Transform coinHud, float hudReachTime, System.Action OnComplete = null)
    {
        tw1.ExResetDT(); tw2.ExResetDT();
        thisTr.position = root.position;
        StopAllCoroutines();
        gObj.SetActive(true);
        StartCoroutine(StartParticleCOR(within, distance, coinHud, hudReachTime, OnComplete));
    }

    IEnumerator StartParticleCOR(float within, float distance, Transform coinHud, float hudReachTime, System.Action OnComplete)
    {
        var reached = false;
        var tg = root.position + moveDir.normalized * distance;
        if (coin2DParticle.UseScreenSpaceCamera)
        {
            tg = root.position + moveDir.normalized * distance * coin2DParticle.ScreenSpaceCameraModifier;
        }
        thisTr.position = root.position;
        tw1 = thisTr.DOMove(tg, within).OnComplete(() =>
        {
            reached = true;
        });

        while (reached == false) { yield return null; }

        tw2 = thisTr.DOMove(coinHud.position, hudReachTime).OnComplete(() =>
        {
            gObj.SetActive(false);
            thisTr.position = root.position;
            OnComplete?.Invoke();
        });
    }
}
