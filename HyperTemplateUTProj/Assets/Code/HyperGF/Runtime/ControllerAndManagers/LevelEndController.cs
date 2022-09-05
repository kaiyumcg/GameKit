using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelEndController : LevelObjectBehaviour
{
    public abstract void StartEndSequence();
    public abstract void StartLevelFailSequence();
}
