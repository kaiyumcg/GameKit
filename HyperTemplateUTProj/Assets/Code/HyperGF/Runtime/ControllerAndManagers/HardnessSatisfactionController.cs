using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the easy-hard stuffs of an entire level
/// </summary>
public class HardnessSatisfactionController : LevelObjectBehaviour
{
    static HardnessSatisfactionController instance;
    public static HardnessSatisfactionController Instance { get { return instance; } }
    protected internal override void OnAwake()
    {
        instance = this;
    }
}