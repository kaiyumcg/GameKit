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
    [SerializeField] TMP_Text levelNo;
    [SerializeField] List<MaskableGraphic> gameplayUIGraphics;
    [SerializeField] MaskableGraphic[] levelEndKeepGraphics;

    [SerializeField] GameObject levelSuccessUI, levelFailedUI;
    [SerializeField] Button nextLevelBtn, reloadBtn;
    [SerializeField] GameObject tapToPlay;
    [SerializeField] TMP_Text coinNum;
    [SerializeField] Coin2DParticle coin2DEffect;
    public MaskableGraphic[] MandetoryLevelEndGameboardUI
    {
        get
        {
            return levelEndKeepGraphics;
        }
    }

    public static UIManager Instance { get { return instance; } }
    static UIManager instance;
    const float defaultTweenTime = 0.3f;
    PlayerStorage<int> playerMoney = null;
    TweenerCore<Vector3, Vector3, VectorOptions> levelSuccessTw = null, levelFailTw = null;

    void Awake()
    {
        playerMoney = new PlayerStorage<int>("DOLLAR_PLAYER", 0);
        coin2DEffect.Init();
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
        StartCoroutine(InitUIPerLevel(isBoot));
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    bool loadNextLevel = false;
    IEnumerator InitUIPerLevel(bool isBoot)
    {
        if (isBoot == false)
        {
            //Analytics-LevelStarted
        }

        coinNum.text = "" + playerMoney.Value;
        loadNextLevel = false;
        levelSuccessUI.SetActive(false);
        levelFailedUI.SetActive(false);
        levelSuccessTw.ExResetDT();
        levelFailTw.ExResetDT();
        levelSuccessUI.transform.localScale = Vector3.one;
        levelFailedUI.transform.localScale = Vector3.one;

        if (isBoot)
        {
            yield break;
        }

        yield return null;
    }

    public void OnRefreshGameStarter()
    {
        
    }

    public void OnStartLevelGameplay()
    {
        
    }

    public void SetShowGameplayUI(bool active, bool useTween, params MaskableGraphic[] exceptions)
    {
        levelNo.text = "Level " + LevelManager.CurLevelNum;
        if (useTween)
        {
            if (active)
            {
                gameplayUIGraphics.ExSetAlpha(0.0f, exceptions);
                gameplayUIGraphics.ExFade(1.0f, defaultTweenTime, exceptions);
            }
            else
            {
                gameplayUIGraphics.ExFade(0.0f, defaultTweenTime, exceptions);
            }
        }
        else
        {
            if (active)
            {
                gameplayUIGraphics.ExSetAlpha(1.0f, exceptions);
            }
            else
            {
                gameplayUIGraphics.ExSetAlpha(0.0f, exceptions);
            }
        }
    }

    public void ClaimMoneyAtEnd(System.Action OnComplete)
    {
        coin2DEffect.Claim(playerMoney.Value, LevelBehaviour.Instance.LevelEndMoney, (moneyNow) =>
        {
            playerMoney.Value = moneyNow;
            OnComplete?.Invoke();
        });
    }

    public void ClaimMoneyFromCenter(System.Action OnComplete)
    {
        var pos = new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 1.0f);
        coin2DEffect.SetEffectPosition(pos);
        coin2DEffect.Claim(playerMoney.Value, LevelBehaviour.Instance.LevelEndMoney, (moneyNow) =>
        {
            playerMoney.Value = moneyNow;
            OnComplete?.Invoke();
        });
    }

    public void ClaimMoneyFrom(Transform worldObjectFrom, int moneyCount, System.Action OnComplete)
    {
        var rootCam = CameraRootHandler.instance.GetComponentInChildren<Camera>();
        coin2DEffect.SetEffectPosition(worldObjectFrom.position, rootCam);
        coin2DEffect.Claim(playerMoney.Value, moneyCount, (moneyNow) =>
        {
            playerMoney.Value = moneyNow;
            OnComplete?.Invoke();
        });
    }

    public void ClaimMoneyFromCenter(int moneyCount, System.Action OnComplete)
    {
        var pos = new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 1.0f);
        coin2DEffect.SetEffectPosition(pos);
        coin2DEffect.Claim(playerMoney.Value, moneyCount, (moneyNow) =>
        {
            playerMoney.Value = moneyNow;
            OnComplete?.Invoke();
        });
    }

    public void ShowLevelSuccessUI()
    {
        levelSuccessTw.ExResetDT();
        levelSuccessUI.gameObject.SetActive(true);
        levelSuccessUI.transform.localScale = Vector3.zero;
        levelSuccessTw = levelSuccessUI.transform.DOScale(1.0f, 0.2f);

        GameStarter.Instance.DisableStarterPanel();
        //Analytics-LevelCompleted
        LevelManager.DoNextLevelCalculation();
        LevelManager.LoadNextLevel(() => { return loadNextLevel; }, null);

        StartCoroutine(COR());
        IEnumerator COR()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    loadNextLevel = true;
                    break;
                }
                yield return null;
            }
        }
    }

    public void ShowLevelFailUI()
    {
        levelFailTw.ExResetDT();
        levelFailedUI.gameObject.SetActive(true);
        levelFailedUI.transform.localScale = Vector3.zero;
        levelFailTw = levelFailedUI.transform.DOScale(1.0f, 0.2f);
    }
}