using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject gameplayUI;
    [SerializeField] TMP_Text levelNo;
    [SerializeField] List<MaskableGraphic> gameplayUIGraphics;

    [SerializeField] GameObject levelSuccessUI, levelFailedUI;
    [SerializeField] Button nextLevelBtn, reloadBtn;
    [SerializeField] GameObject tapToPlay;

    public static UIManager Instance { get { return instance; } }
    static UIManager instance;
    const float defaultTweenTime = 0.3f;

    TweenerCore<Vector3, Vector3, VectorOptions> levelSuccessTw = null, levelFailTw = null;

    void Awake()
    {
        tapToPlay.SetActive(false);
        SetShowGameplayUI(false, false);
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
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        nextLevelBtn.onClick.RemoveAllListeners();
        nextLevelBtn.onClick.AddListener(() =>
        {
            //todo
            loadNextLevel = true;
        });

        reloadBtn.onClick.RemoveAllListeners();
        reloadBtn.onClick.AddListener(() =>
        {
            //Analytics-LevelFailed
            LevelManager.ReloadThisLevel();
        });
    }

    void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var isBoot = scene.name == "boot";
        if (isBoot == false) { tapToPlay.SetActive(true); }
        StopAllCoroutines();
        InitUIPerLevel(isBoot);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    bool loadNextLevel = false;
    public void InitUIPerLevel(bool isBoot)
    {
        if (isBoot == false)
        {
            //Analytics-LevelStarted
        }
        loadNextLevel = false;
        levelSuccessUI.SetActive(false);
        levelFailedUI.SetActive(false);
        levelSuccessTw.ResetDT();
        levelFailTw.ResetDT();
        levelSuccessUI.transform.localScale = Vector3.one;
        levelFailedUI.transform.localScale = Vector3.one;

        if (isBoot)
        {
            return;
        }
    }

    public void OnRefreshGameStarter()
    {
        
    }

    public void OnStartLevelGameplay()
    {
        
    }

    public void SetShowGameplayUI(bool active, bool useTween)
    {
        levelNo.text = "Level " + LevelManager.CurLevelNum;
        gameplayUI.SetActive(active);
        if (useTween)
        {
            if (active)
            {
                gameplayUIGraphics.SetAlpha(0.0f);
                gameplayUIGraphics.DoFade(1.0f, defaultTweenTime);
            }
            else
            {
                gameplayUIGraphics.DoFade(0.0f, defaultTweenTime);
            }
        }
        else
        {
            if (active)
            {
                gameplayUIGraphics.SetAlpha(1.0f);
            }
            else
            {
                gameplayUIGraphics.SetAlpha(0.0f);
            }
        }
    }

    public void ShowLevelSuccessUI()
    {
        levelSuccessTw.ResetDT();
        levelSuccessUI.gameObject.SetActive(true);
        levelSuccessUI.transform.localScale = Vector3.zero;
        levelSuccessTw = levelSuccessUI.transform.DOScale(1.0f, 0.2f);

        //Analytics-LevelCompleted
        LevelManager.DoNextLevelCalculation();
        LevelManager.LoadNextLevel(() => { return loadNextLevel; }, null);
    }

    public void ShowLevelFailUI()
    {
        levelFailTw.ResetDT();
        levelFailedUI.gameObject.SetActive(true);
        levelFailedUI.transform.localScale = Vector3.zero;
        levelFailTw = levelFailedUI.transform.DOScale(1.0f, 0.2f);
    }
}