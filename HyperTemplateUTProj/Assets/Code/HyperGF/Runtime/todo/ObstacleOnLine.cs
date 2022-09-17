using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;

[System.Serializable]
public class LineObstacle
{
    [SerializeField] internal Transform root;
    [SerializeField] internal float leftAmount, rightAmount;
    [SerializeField] internal float speed = 4f;
    [SerializeField] internal bool flipped = false;
    [SerializeField] internal bool useStartDelay = false;
    [SerializeField] internal float startDelay = 1.5f;
    [SerializeField] internal Color debugColor = Color.black;
    
    internal List<Vector3> endPositions, startPositions;
    internal List<Vector3> targetPositions;
    internal List<Transform> targets;
}

public abstract class ObstacleOnLine : GameObstacle
{
    [SerializeField] List<LineObstacle> data;
    protected override void OnStartObstacle()
    {
        base.OnStartObstacle();
        StartCoroutine(Platformer());
        IEnumerator Platformer()
        {
            data.ExForEach((d) => { StartCoroutine(RunObstacle(d)); });
            yield return null;
        }

        IEnumerator RunObstacle(LineObstacle desc)
        {
            desc.targets = desc.root.ExGetImmediateChilds();
            desc.targetPositions = CollectionEx.GetListWithCount<Vector3>(desc.targets.Count);
            desc.startPositions = CollectionEx.GetListWithCount<Vector3>(desc.targets.Count);
            desc.endPositions = CollectionEx.GetListWithCount<Vector3>(desc.targets.Count);

            var targets = desc.targets;
            targets.ExForEach((t, i) =>
            {
                desc.startPositions[i] = desc.root.right * desc.rightAmount + desc.targets[i].position;
                desc.endPositions[i] = desc.root.right * -1f * desc.leftAmount + desc.targets[i].position;
                desc.targetPositions[i] = desc.flipped ? desc.startPositions[i] : desc.endPositions[i];
            });
            while (true)
            {
                targets.ExForEach((t, i) =>
                {
                    t.position = Vector3.MoveTowards(t.position, desc.targetPositions[i], desc.speed * Time.deltaTime);
                    var dist = Vector3.Distance(t.position, desc.targetPositions[i]);
                    if(dist < 0.1f)
                    {
                        var lc = t.position;
                        desc.targetPositions[i] = Vector3.Distance(lc, desc.endPositions[i]) <
                        Vector3.Distance(lc, desc.startPositions[i]) ? desc.endPositions[i] : desc.startPositions[i];
                        t.position = Vector3.Distance(lc, desc.endPositions[i]) <
                        Vector3.Distance(lc, desc.startPositions[i]) ? desc.startPositions[i] : desc.endPositions[i];
                    }
                });
                yield return null;
            }
        }
    }

    protected override void OnDrawDebugObjects()
    {
        base.OnDrawDebugObjects();
        data.ExForEach((d) =>
        {
            if (d.root != null)
            {
                var startPosition = d.root.right * d.rightAmount + d.root.position;
                var endPosition = d.root.right * -1f * d.leftAmount + d.root.position;

                Gizmos.color = d.debugColor;
                DrawArrow.ForGizmo(startPosition + Vector3.up * 5f, (endPosition - startPosition), 4f);
            }
        });
    }

    protected override void OnStartDeath()
    {
        base.OnStartDeath();
        StopAllCoroutines();
    }
}