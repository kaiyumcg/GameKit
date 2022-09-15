using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class LevelBehaviour : LevelObjectBehaviour, ILevelMarker
{
    [SerializeField] int levelEndMoney = 50;
    [SerializeField] LevelEndController endController;
    bool gameOver = false;
    static LevelBehaviour instance;
    public bool GameOver { get { return gameOver; } }
    public static LevelBehaviour Instance { get { return instance; } }
    public int LevelEndMoney { get { return levelEndMoney; } }

    protected internal override void OnAwake()
    {
        instance = this;
        gameOver = false;
    }

    void ILevelMarker.LevelComplete()
    {
        if (gameOver) { return; }
        gameOver = true;
        endController.StartEndSequence();
    }

    void ILevelMarker.LevelFail()
    {
        if (gameOver) { return; }
        gameOver = true;
        endController.StartLevelFailSequence();
    }
}