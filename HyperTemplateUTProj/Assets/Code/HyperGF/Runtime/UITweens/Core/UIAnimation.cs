using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class UIAnimation : MonoBehaviour
{
    [SerializeField] bool startDefaultAutomatically = false;
    [SerializeField] bool useDelayForDefaultAnimation = false;
    [SerializeField] float delayForDefaultAnimation = 0.5f;
    [SerializeField] UIAnimationData defaultAnimation;
    [SerializeField] List<UIAnimationSet> sets;

    //[EasyButtons.Button("PlayDefault")]
    void PlayDefaultEd()
    {

    }

    public void PlayDefault(Action OnComplete, bool loop)
    {
        //if (loop)
        //{
        //    //todo should stop previously played, user jodi intednd kore je bar bar ektu por por play hobe?
        //    if (defaultHandle != null) { StopCoroutine(defaultHandle); }
        //    StartCoroutine(COR());
        //}
        //else
        //{
        //    defaultAnimation.Play(OnComplete);
        //}

        //IEnumerator COR()
        //{
        //    while (true)
        //    {
        //        var done = false;
        //        defaultAnimation.Play(() =>
        //        {
        //            done = true;
        //            OnComplete?.Invoke();
        //        });

        //        while (!done) { yield return null; }
        //        yield return null;
        //    }
        //}
    }

    //todo play single or default animation
    //todo play anyone with matched name
    //todo play a set sequentially one after another or all at once
    //todo stop single or default animation
    //todo stop anyone with matched name
    //todo stop a set sequentially one after another or all at once
    //todo play/stop default in editor with inspector button

    //todo asset e tags name sense make kore or should it not be in the inspector data?
    //todo set name should not be a single one, right? can be multi tag?
    public void PlaySetWithName(Action OnComplete, bool loop, bool oneByOne, params string[] names)
    {

    }   
}