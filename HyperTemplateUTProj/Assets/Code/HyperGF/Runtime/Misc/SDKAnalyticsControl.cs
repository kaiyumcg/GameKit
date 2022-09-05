using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-11000)]
public class SDKAnalyticsControl : MonoBehaviour
{
    [SerializeField] LevelManager man;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (man.OnStartLevel == null) { man.OnStartLevel = new UnityEngine.Events.UnityEvent<int>(); }
        if (man.OnCompleteLevel == null) { man.OnCompleteLevel = new UnityEngine.Events.UnityEvent<int>(); }
        if (man.OnReloadLevel == null) { man.OnReloadLevel = new UnityEngine.Events.UnityEvent<int>(); }
        man.OnStartLevel.AddListener((lvNum) =>
        {
            //todo API CALL
            //Debug.Log("<color='green'>Level started: " + lvNum + "</color>");
        });

        man.OnCompleteLevel.AddListener((lvNum) =>
        {
            //todo API CALL
            //Debug.Log("<color='green'>Level " + lvNum + " completed with score: " + score + "</color>");
        });

        man.OnReloadLevel.AddListener((lvNum) =>
        {
            //todo API CALL
            //Debug.Log("<color='green'>Level " + lvNum + " failed with score: " + score + "</color>");
        });
    }
}