using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.Rendering;

public class CameraRootHandler : LevelObjectBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool alwaysUseSmoothing = false;
    [SerializeField] float transitionSpeed = 0.06f;
    [SerializeField] float transitionSpeedMultiplier = 0.5f, withinMultiplierTime = 0.8f;
    [SerializeField] float smoothTime = 0.06f;
    [SerializeField, DebugView] bool inTransition = false;
    Transform nextTarget, tr;
    Vector3 velocity = Vector3.zero, transitionVelocity = Vector3.zero, tranPos;
    bool nowReady = false;
    float originalSpeed;
    TweenerCore<float, float, FloatOptions> tw = null;

    public static CameraRootHandler instance;

    protected internal override void OnAwake()
    {
        Application.targetFrameRate = 60;
        instance = this;
        stopCameraAtOnce = false;
    }

    private IEnumerator Start()
    {
        tr = transform;
        yield return null;
        nowReady = true;
        originalSpeed = transitionSpeed;
        tranPos = target.position;
    }

    public void Start_Revive()
    {
        tranPos = target.position;
    }
    
    public void FollowSmoothly(Transform nextTarget)
    {
        inTransition = true;
        this.nextTarget = nextTarget;

        tranPos = tr.position;

        var newTransitionSpeed = originalSpeed * transitionSpeedMultiplier;
        if (tw != null && tw.IsActive()) { tw.Kill(); tw = null; }
        tw = DOTween.To(() => transitionSpeed, x => transitionSpeed = x, newTransitionSpeed, withinMultiplierTime);
    }

    bool stopCameraAtOnce = false;
    public void StopCameraDueToDeathOrLevelEnd()
    {
        stopCameraAtOnce = true;
        if (tw != null && tw.IsActive()) { tw.Kill(); tw = null; }
    }

    public void StopCameraDueToDeath_Revive()
    {
        stopCameraAtOnce = false;
        inTransition = false;
    }

    private void LateUpdate()
    {
        if (stopCameraAtOnce) { return; }
        if (!nowReady) { return; }
        Vector3 tg;
        if (inTransition)
        {
            tranPos = Vector3.SmoothDamp(tranPos, nextTarget.position, ref transitionVelocity, transitionSpeed);
            if (Vector3.Distance(tranPos, nextTarget.position) < 5f)
            {
                inTransition = false;
                target = nextTarget;
                transitionSpeed = originalSpeed;
            }
            tg = tranPos;
        }
        else
        {
            tg = target.position;
        }

        if (inTransition || alwaysUseSmoothing)
        {
            tr.position = Vector3.SmoothDamp(tr.position, tg, ref velocity, smoothTime);
        }
        else
        {
            tr.position = tg;
        }
        //tr.rotation = Player.instance.PlayerRotationRoot.rotation;
    }
}
