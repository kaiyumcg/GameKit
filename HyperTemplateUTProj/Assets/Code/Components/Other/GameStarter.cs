using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour, IPointerDownHandler
{
    public static GameStarter Instance { get { return instance; } }
    static GameStarter instance = null;
    bool _clicked = false;
    bool gameStarted = false;
    public static bool Clicked { get { return instance == null ? false : instance._clicked; } }
    public static bool GameStarted { get { return instance == null ? false : instance.gameStarted; } }
    bool valid = false;

    void Awake()
    {
        instance = this;
        gameStarted = false;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "boot") { return; }
        _clicked = false;
        valid = false;
        StopAllCoroutines();
        StartCoroutine(StartCOR());
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    IEnumerator StartCOR()
    {
        valid = false;
        _clicked = false;
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (valid)
                {
                    gameStarted = true;
                    _clicked = true;
                }
                
            });
        }
        yield return new WaitForSeconds(0.1f);
        valid = true;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (valid)
        {
            gameStarted = true;
        }
    }
}