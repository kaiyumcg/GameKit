using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class GameStarter : MonoBehaviour, IPointerDownHandler//, IPointerClickHandler
{
    [SerializeField] TMP_Text tapToStartTxt;
    
    static GameStarter instance = null;
    [SerializeField, DebugView] bool gameStarted = false;
    TweenerCore<Color, Color, ColorOptions> tw = null;

    public static GameStarter Instance { get { return instance; } }
    public static bool GameStarted { get { return instance == null ? false : instance.gameStarted; } }

    void Awake()
    {
        tapToStartTxt.raycastTarget = false;
        instance = this;
        gameStarted = false;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    public void DisableStarterPanel()
    {
        GetComponent<Image>().raycastTarget = false;
    }

    void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "boot") { return; }
        GetComponent<Image>().raycastTarget = true;
        UIManager.Instance.OnRefreshGameStarter();
        UIManager.Instance.SetShowGameplayUI(false, useTween: false);
        gameStarted = false;
        valid = false;

        if (tw != null && tw.IsActive()) { tw.Kill(); }
        tapToStartTxt.gameObject.SetActive(true);
        tapToStartTxt.color = Color.white;
        tapToStartTxt.ExBlinkUntil(this, 0.4f, () => { return gameStarted; });

        StopAllCoroutines();
        StartCoroutine(COR());
        IEnumerator COR()
        {
            yield return null;
            valid = true;
        }
    }

    bool valid = false;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (gameStarted || valid == false) { return; }
        HideTapToStartTxt();
        gameStarted = true;
    }

    void HideTapToStartTxt()
    {
        tw.ExResetDT();
        tw = tapToStartTxt.DOFade(0.0f, 0.4f).OnComplete(() =>
        {
            tapToStartTxt.gameObject.SetActive(false);
        });
        UIManager.Instance.OnStartLevelGameplay();
        UIManager.Instance.SetShowGameplayUI(true, useTween : true);
    }
}