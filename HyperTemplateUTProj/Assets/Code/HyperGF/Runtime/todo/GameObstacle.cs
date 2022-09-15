using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Options;

public abstract class GameObstacle : LevelObjectBehaviour
{
    protected internal override void OnAwake()
    {
        tr = transform;
    }

    Transform tr; 
    [SerializeField] bool useDelay = false;
    [SerializeField] float delayAmount = 1.5f;
    [SerializeField, DebugView] int hitCount = 0;
    public int HitCount { get { return hitCount; } }
    public bool Used { get { return used; } }
    public Transform _Transform { get { return tr; } }
    bool used = false;
    IEnumerator Start()
    {
        hitCount = 0;
        used = false;
        if (useDelay)
        {
            yield return new WaitForSeconds(delayAmount);
        }
        OnStartObstacle();
    }

    protected virtual void OnStartObstacle() { }
    protected virtual void OnStartDeath() { }
    protected abstract System.Type DefinePlayerScriptType();

    void OnTriggerEnter(Collider other)
    {
        if (used) { return; }
        var pl = other.GetComponentInParent(DefinePlayerScriptType());
        if (pl == null) { return; }

        used = true;
        OnStartDeath();
    }
}
