using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-5000)]
public class LoaderUIControl : MonoBehaviour
{
    [SerializeField] GameObject loaderUI;
    [SerializeField] Image loaderFill;
    static LoaderUIControl instance;
    public static LoaderUIControl Instance { get { return instance; } }
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

    public void StartLoaderUI()
    {
        loaderUI.SetActive(true);
        loaderUICORHandle = StartCoroutine(LoaderUICOR());
    }

    public void After90PercentSceneLoad()
    {
        loaderFill.fillAmount = 1.0f;
        resourceLoadedForUI = true;
        if (loaderTw != null && loaderTw.IsActive()) { loaderTw.Kill(); loaderTw = null; }
        if (loaderUICORHandle != null) { StopCoroutine(loaderUICORHandle); }
        if (loaderUITweenCORHandle != null) { StopCoroutine(loaderUITweenCORHandle); }
    }

    public void OnCompleteLoaderUI()
    {
        loaderUI.SetActive(false);
        Destroy(loaderUI);
    }

    Coroutine loaderUICORHandle = null, loaderUITweenCORHandle = null;
    TweenerCore<float, float, FloatOptions> loaderTw = null;
    float tw_fAmount = 0.0f;
    bool resourceLoadedForUI = false;
    IEnumerator LoaderUICOR()
    {
        resourceLoadedForUI = false;
        loaderUITweenCORHandle = StartCoroutine(CustomLoaderUICOR());
        yield return null;
    }

    IEnumerator CustomLoaderUICOR()
    {
        yield return null;
        var done = false;
        loaderTw = DOTween.To(() =>
        { return tw_fAmount; },
       x =>
       {
           tw_fAmount = x;
           loaderFill.fillAmount = tw_fAmount;
       }, 0.1f, 0.3f).OnComplete(() => { done = true; });
        if (resourceLoadedForUI) { yield break; }
        while (!done) { if (resourceLoadedForUI) { yield break; } yield return null; }
        yield return new WaitForSeconds(0.2f);
        if (resourceLoadedForUI) { yield break; }

        done = false;
        loaderTw = DOTween.To(() =>
        { return tw_fAmount; },
        x =>
        {
            tw_fAmount = x;
            loaderFill.fillAmount = tw_fAmount;
        }, 0.7f, 0.4f).OnComplete(() => { done = true; });
        if (resourceLoadedForUI) { yield break; }
        while (!done) { if (resourceLoadedForUI) { yield break; } yield return null; }
        yield return new WaitForSeconds(0.2f);
        if (resourceLoadedForUI) { yield break; }

        done = false;
        loaderTw = DOTween.To(() =>
        { return tw_fAmount; },
        x =>
        {
            tw_fAmount = x;
            loaderFill.fillAmount = tw_fAmount;
        }, 0.9f, 0.4f).OnComplete(() => { done = true; });
        if (resourceLoadedForUI) { yield break; }
        while (!done) { if (resourceLoadedForUI) { yield break; } yield return null; }
    }
}
