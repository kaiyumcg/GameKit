using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorEx
{
    public static Vector3 ExMidPoint(this Vector3 pt, Vector3 target)
    {
        var toTarget = target - pt;
        var midDist = toTarget.magnitude * 0.5f;
        return pt + toTarget.normalized * midDist;
    }
    public static bool ExHasCrossed(this Vector3 thisPt, Vector3 point, Vector3 moveDirection)
    {
        var toPointDirection = point - thisPt;
        return Vector3.Dot(toPointDirection.normalized, moveDirection.normalized) < 0.0f;
    }
    public static Vector3 ExGetPointAtNormalizedDistance(this Vector3 pt, float normalizedDistance, Vector3 target)
    {
        var toTarget = target - pt;
        var dist = toTarget.magnitude * normalizedDistance;
        return pt + toTarget.normalized * dist;
    }
}