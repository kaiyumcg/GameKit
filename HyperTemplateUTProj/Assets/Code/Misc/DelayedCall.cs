using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-7000)]
public class DelayedCall : MonoBehaviour
{
    static DelayedCall instance;
    public static DelayedCall Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
    }

    public void Wait(float seconds, System.Action OnComplete)
    {
        StartCoroutine(WaitCOR());
        IEnumerator WaitCOR()
        {
            yield return new WaitForSeconds(seconds);
            OnComplete?.Invoke();
        }
    }
}