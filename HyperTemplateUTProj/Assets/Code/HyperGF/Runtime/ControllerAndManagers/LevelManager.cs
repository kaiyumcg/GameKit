using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public delegate bool WhenLoaderFunc();
[DefaultExecutionOrder(-3000)]
public class LevelManager : MonoBehaviour
{
    [SerializeField] List<AssetReference> scenes = new List<AssetReference>();
    
    [SerializeField] ShaderVariantCollection shaderCollection;
    [SerializeField] int levelStartNumAfterAllDone = 2;
    [SerializeField] bool loadRandomlyAfterDone = false;
    
    static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    int curLevelNum = -1;
    int nextLevelNum = -1;
    int curRealLevelNum = -1;
    int nextRealLevelNum = -1;

    public static bool IsOk() { return instance != null; }
    public static int CurLevelNum { get { return instance.curLevelNum; } }
    public static int NextLevelNum { get { return instance.nextLevelNum; } }
    public static int CurRealLevelNum { get { return instance.curRealLevelNum; } }
    public static int NextRealLevelNum { get { return instance.nextRealLevelNum; } }

    [HideInInspector]
    public UnityEvent<int> OnStartLevel, OnReloadLevel, OnCompleteLevel;

    public static int TotalLevelNum
    {
        get
        {
            if (instance == null) { return -1; }
            else
            {
                var totalUnityLevelNum = instance.scenes.Count;
                return totalUnityLevelNum - 1;
            }
        }
    }

    [SerializeField] bool manualSceneLoadForTesting = false;
    [SerializeField] int manualLevelNo = 5;
    static string manualSceneName = "";
    static CallBootObj manualObject = null;
    public static void SetAsManualLoad()
    {
        var isEditor = false;
#if UNITY_EDITOR
        isEditor = true;
#endif
        if (!isEditor)
        {
            Debug.Log("platform name: " + Application.platform);
            throw new Exception("Manual scene load exception for nonEditor platform!");
        }
        manualSceneName = SceneManager.GetActiveScene().name;
        manualObject = new CallBootObj();
    }

    static int levelToLoadImmediately = -1;
    AsyncOperationHandle<SceneInstance> sceneLoadHandle;
    AssetReference lastLoadedLevel = null;
    
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

        PreloadHandler.Instance.AllDependencyLoaded = false;
        LoaderUIControl.Instance.StartLoaderUI();
        shaderCollection.WarmUp();
        AdjustFPS();
        StartCoroutine(InitSys());
    }

    void AdjustFPS()
    {
        Application.targetFrameRate = 60;
#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#endif
    }

    IEnumerator InitSys()
    {
        yield return null;
        yield return null;

        curLevelNum = PlayerPrefs.GetInt("curLevelNum", 1);
        var defaultRealLevelNum = 1;
        curRealLevelNum = PlayerPrefs.GetInt("curRealLevelNum", defaultRealLevelNum);
        if (curRealLevelNum < 1)
        {
            curRealLevelNum = 1;
            PlayerPrefs.SetInt("curRealLevelNum", curRealLevelNum);
        }
        nextLevelNum = PlayerPrefs.GetInt("nextLevelNum", 2);
        nextRealLevelNum = PlayerPrefs.GetInt("nextRealLevelNum", defaultRealLevelNum + 1);
        nextRealLevelNum = curRealLevelNum + 1;
        CheckLevelOverFlow(curRealLevelNum - 1);//curScene.buildIndex
        PlayerPrefs.SetInt("curRealLevelNum", curRealLevelNum);
        levelToLoadImmediately = curRealLevelNum - 1;

#if UNITY_EDITOR
        if (manualSceneLoadForTesting)
        {
            levelToLoadImmediately = manualLevelNo;
        }

        if (manualObject != null)
        {
            if (scenes != null && scenes.Count > 0)
            {
                for (int i = 0; i < scenes.Count; i++)
                {
                    var sc = scenes[i];
                    if (sc == null || sc.editorAsset.name != manualSceneName) { continue; }
                    levelToLoadImmediately = i;
                    Debug.Log("i holo: " + i);
                    break;
                }
            }
        }
#endif

        var done = false;
        shouldFirstPlay = false;
        DelayedCall.Instance.Wait(0.3f, () => { shouldFirstPlay = true; });
        yield return StartCoroutine(PreloadHandler.Instance.LoadAdditionalDependencies());
        LoadKLevel(() => { return shouldFirstPlay && PreloadHandler.Instance.AllDependencyLoaded; }, () =>
        {
            done = true;
        });

        while (true)
        {
            if (sceneLoadHandle.IsValid() && sceneLoadHandle.PercentComplete > 0.9f)
            {
                break;
            }
            yield return null;
        }

        LoaderUIControl.Instance.After90PercentSceneLoad();
        while (!done) { yield return null; }
        OnStartLevel?.Invoke(curLevelNum);
        LoaderUIControl.Instance.OnCompleteLoaderUI();
    }

    bool shouldFirstPlay = false;

    void CheckLevelOverFlow(int currentLevelIndex)
    {
        if (nextRealLevelNum > scenes.Count)
        {
            //so we have either completed all levels for the first time or next or next-next etc time...
            List<int> randLvNums = new List<int>();
            for (int i = 0; i < scenes.Count; i++)
            {
                if (i == 0 || i == currentLevelIndex) { continue; }
                randLvNums.Add(i);
            }
            var randID = UnityEngine.Random.Range(0, randLvNums.Count);
            var lvIndex = randLvNums.Count == 0 ? 1 : randLvNums[randID];
            nextRealLevelNum = loadRandomlyAfterDone ? lvIndex + 1 : levelStartNumAfterAllDone;
        }
    }

    public static void LoadNextLevel(WhenLoaderFunc whenToLoad, System.Action OnComplete)
    {
        instance.LoadKLevel(whenToLoad, OnComplete);
    }

    public static void DoNextLevelCalculation()
    {
        var curSceneIdx = instance.curRealLevelNum;
        instance.OnCompleteLevel?.Invoke(instance.curLevelNum);
        var targetSceneID = instance.nextRealLevelNum - 1;
        instance.curRealLevelNum = targetSceneID + 1;
        instance.curLevelNum++;

        instance.OnStartLevel?.Invoke(instance.curLevelNum);

        instance.nextLevelNum = instance.curLevelNum + 1;
        instance.nextRealLevelNum = instance.curRealLevelNum + 1;
        instance.CheckLevelOverFlow(curSceneIdx - 1);

        PlayerPrefs.SetInt("curLevelNum", instance.curLevelNum);
        PlayerPrefs.SetInt("curRealLevelNum", instance.curRealLevelNum);
        PlayerPrefs.SetInt("nextLevelNum", instance.nextLevelNum);
        PlayerPrefs.SetInt("nextRealLevelNum", instance.nextRealLevelNum);
        levelToLoadImmediately = targetSceneID;
    }

    public static void ReloadThisLevel()
    {
        instance.StartCoroutine(instance.Reloader());
    }

    IEnumerator Reloader()
    {
        ReviewPopupControl.Instance.IncrementReviewRelatedData();
        instance.OnReloadLevel?.Invoke(instance.curLevelNum);
        levelToLoadImmediately = curRealLevelNum - 1;
        DOTween.KillAll();
        Addressables.UnloadScene(sceneLoadHandle.Result);
        sceneLoadHandle = Addressables.LoadScene(scenes[levelToLoadImmediately]);
        yield return null;
        ReviewPopupControl.Instance.TryShowReviewPanel();
    }

    void LoadKLevel(WhenLoaderFunc whenToLoad, System.Action OnComplete)
    {
        StartCoroutine(SceneLoaderAsync());
        IEnumerator SceneLoaderAsync()
        {
            ReviewPopupControl.Instance.IncrementReviewRelatedData();
            //Not allowing scene activation immediately
            lastLoadedLevel = scenes[levelToLoadImmediately];
            Debug.Log("last loaded level index: " + levelToLoadImmediately);

            sceneLoadHandle = lastLoadedLevel.LoadSceneAsync(LoadSceneMode.Single, false);
            yield return sceneLoadHandle;

            while (true)
            {
                if (whenToLoad == null) { break; }
                else
                {
                    var loadIt = whenToLoad.Invoke();
                    if (loadIt) { break; }
                }
                yield return null;
            }

            DOTween.KillAll();
            //One way to handle manual scene activation.
            if (sceneLoadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                yield return sceneLoadHandle.Result.ActivateAsync();
            }
            else
            {
                throw new Exception("Scene load error on LevelManager");
            }
            OnComplete?.Invoke();
            ReviewPopupControl.Instance.TryShowReviewPanel();
            Application.targetFrameRate = 60;
            yield return new WaitForSeconds(2f);
            Application.targetFrameRate = 60;
        }
    }
}