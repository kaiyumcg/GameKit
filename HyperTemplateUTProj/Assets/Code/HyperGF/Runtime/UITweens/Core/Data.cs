using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIAnimationData
{
    [SerializeField] UIAnimationAsset data;
    UIAnimHandle handle;
    
}

[System.Serializable]
internal class UIAnimationSet
{
    [SerializeField] string setName = "";
    internal string SetName { get { return setName; } }
    [SerializeField] List<UIAnimationData> animations;
    internal List<UIAnimationData> Animations { get { return animations; } }
}