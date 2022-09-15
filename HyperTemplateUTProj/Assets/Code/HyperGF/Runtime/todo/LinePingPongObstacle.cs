using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinePingPongObstacle : GameObstacle
{
    [SerializeField] float speed = 4.5f;
    [SerializeField] float aheadDistance = 2.5f;
    [SerializeField] Color debugColor = Color.blue;
    [SerializeField] Animator anim;
    protected override void OnStartObstacle()
    {
        base.OnStartObstacle();
        StartCoroutine(COR());
        IEnumerator COR()
        {
            var tr = _Transform;
            var endPos = tr.position + tr.forward * aheadDistance;
            var initPos = tr.position;
            var targetPos = endPos;
            while (true)
            {
                var dir = targetPos - tr.position;
                if(dir.magnitude > 0.1f)
                {
                    var qot = Quaternion.LookRotation(dir);
                    tr.rotation = qot;
                }

                tr.position = Vector3.MoveTowards(tr.position, targetPos, speed * Time.deltaTime);
                var dist = Vector3.Distance(tr.position, targetPos);
                if (dist < 0.1f)
                {
                    targetPos = Vector3.Distance(tr.position, endPos) <
                    Vector3.Distance(tr.position, initPos) ? initPos : endPos;
                }

                yield return null;
            }
        }
    }

    protected override void OnDrawDebugObjects()
    {
        if (Application.isPlaying) { return; }
        base.OnDrawDebugObjects();
        Gizmos.color = debugColor;
        DrawArrow.ForGizmo(transform.position, transform.forward * aheadDistance, 1f);
    }
}
