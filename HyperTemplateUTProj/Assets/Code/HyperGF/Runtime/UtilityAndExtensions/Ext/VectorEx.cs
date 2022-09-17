using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorEx
{
    /// <summary>
    /// Gets the coordinates of the intersection point of two lines.
    /// </summary>
    /// <param name="A1">A point on the first line.</param>
    /// <param name="A2">Another point on the first line.</param>
    /// <param name="B1">A point on the second line.</param>
    /// <param name="B2">Another point on the second line.</param>
    /// <param name="found">Is set to false of there are no solution. true otherwise.</param>
    /// <returns>The intersection point coordinates. Returns Vector2.zero if there is no solution.</returns>
    public static Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found)
    {
        float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

        if (tmp == 0)
        {
            // No solution!
            found = false;
            return Vector2.zero;
        }

        float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

        found = true;

        return new Vector2(
            B1.x + (B2.x - B1.x) * mu,
            B1.y + (B2.y - B1.y) * mu
        );
    }

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