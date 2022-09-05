using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


[System.Serializable]
public class BatcherData
{
    [SerializeField] Transform root;
    [SerializeField] bool shadow = true, occlusionCulling = false;
    public Transform Root { get { return root; } }
    public bool Shadow { get { return shadow; } }
    public bool OcclusionCulling { get { return occlusionCulling; } }
}

[System.Serializable]
public class RelativeTransform
{
    [SerializeField] float backward, upward, side;
    [SerializeField] RangedVector3 rotationOffset;
    public float Backward { get { return backward; } }
    public float Upward { get { return upward; } }
    public float Side { get { return side; } }
    public RangedVector3 RotationOffset { get { return rotationOffset; } }

    public Vector3 GetRelativePosition(Transform from)
    {
        return from.position + from.forward * -backward + from.up * upward + from.right * side;
    }

    public bool GetLookAtRotation(Transform thisTransform, Transform target, ref Quaternion result)
    {
        var toTarget = target.position - thisTransform.position;
        toTarget = toTarget.normalized + rotationOffset.Get();
        if (Mathf.Approximately(toTarget.magnitude, 0.0f) == false)
        {
            result = Quaternion.LookRotation(toTarget, Vector3.up);
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public class RangedVector3
{
    [SerializeField, Range(-1.0f, 1.0f)] float x, y, z;
    public Vector3 Get()
    {
        return new Vector3(x, y, z);
    }
}

public delegate void OnDoAnythingFunc();
public delegate void OnDoAnythingFunc2(int level);
public delegate void OnDoAnythingFunc3(int level, float raise, float totalNow);
public delegate void OnDoAnythingFunc4(int level, float raise, float totalNow, float totalPrev);
public delegate bool WhenToDoFunc();