using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class LevelBehaviour : LevelObjectBehaviour
{
    [SerializeField] LevelEndController endController;
    bool gameOver = false;
    static LevelBehaviour instance;
    public bool GameOver { get { return gameOver; } }
    public static LevelBehaviour Instance { get { return instance; } }

    protected internal override void OnAwake()
    {
        instance = this;
        gameOver = false;
    }

    void LevelComplete()
    {
        if (gameOver) { return; }
        gameOver = true;
        endController.StartEndSequence();
        UIManager.Instance.SetShowGameplayUI(false, useTween : false);
    }

    void LevelFail()
    {
        if (gameOver) { return; }
        gameOver = true;
        endController.StartLevelFailSequence();
    }
}