using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBootObj { }
public abstract class LevelObjectBehaviour : MonoBehaviour
{
    protected internal abstract void OnAwake();
    static CallBootObj calledBoot = null;
    [HideInInspector] protected bool isBehaviourValid = false;
    [SerializeField] protected bool behaviourEnabled = true;
    private void Awake()
    {
        isBehaviourValid = false;
        if (LevelManager.Instance == null && calledBoot == null)
        {
            calledBoot = new CallBootObj();
            LevelManager.SetAsManualLoad();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }

        if (LevelManager.Instance == null) { return; }

        isBehaviourValid = true;
        OnAwake();
    }

    protected virtual void OnDrawDebugObjects() { }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        OnDrawDebugObjects();
    }
#endif
}