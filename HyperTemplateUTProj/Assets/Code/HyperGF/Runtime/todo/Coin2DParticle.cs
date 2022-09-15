using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Core;

public class Coin2DParticle : MonoBehaviour
{
    [Header("Initial Spread")]
    [SerializeField] float spreadMin = 0.15f;
    [SerializeField] float spreadMax = 0.2f;
    [SerializeField] float particleReachWithMin = 0.4f, particleReachWithMax = 1f;
    [SerializeField] bool useScreenSpaceCamera = false;
    [SerializeField] float screenSpaceCameraModifier = 0.0027f;

    [Header("HUD Reach Setting")]
    [SerializeField] Transform coinHud;
    [SerializeField] TMP_Text coinNumTxt;
    [SerializeField] float hudReachTimeMin = 0.8f, hudReachTimeMax = 1f;
   
    List<UICoin> coin2Ds = new List<UICoin>();
    Transform coin2DRoot;
    System.Action<int> _CB;
    List<Tweener> twList;
    bool startedToEnter = false;
    int coinNumHUDNow = 0;
    TweenerCore<int, int, NoOptions> coinHUDTw = null;

    public UnityEvent onCoinStartedToEnterHUD;
    public UnityEvent<int, int> onCoinReachHUD;
    public bool UseScreenSpaceCamera { get { return useScreenSpaceCamera; } }
    public float ScreenSpaceCameraModifier { get { return screenSpaceCameraModifier; } }

    public void Init()
    {
        coin2DRoot = transform;
        coin2Ds = new List<UICoin>();
        if (coin2DRoot != null && coin2DRoot.childCount > 0)
        {
            for (int i = 0; i < coin2DRoot.childCount; i++)
            {
                var c = coin2DRoot.GetChild(i).GetComponent<UICoin>();
                if (c == null) { continue; }
                coin2Ds.Add(c);
                c.Init(this, coin2DRoot);
            }
        }
        startedToEnter = false;
        _CB = null;
    }

    public void SetEffectPosition(Vector3 screenSpacePosition)
    {
        coin2DRoot.position = screenSpacePosition;
    }

    public void SetEffectPosition(Vector3 worldPosition, Camera camera)
    {
        coin2DRoot.position = camera.WorldToScreenPoint(worldPosition);
    }

    public void Claim(int coinNumTotalNow, int coinToGive,
        System.Action<int> OnComplete, float coinHUDUpdateWithin = 0.5f,
        string coinHUDAdditionalText = "")
    {
        gameObject.SetActive(true);
        startedToEnter = false;
        twList.ExResetDT();
        coinHUDTw.ExResetDT();

        int toCoinNum = coinNumTotalNow + coinToGive;

        if (_CB != null) { _CB?.Invoke(toCoinNum); _CB = null; }
        _CB = OnComplete;
        coinNumTxt.text = coinNumTotalNow + coinHUDAdditionalText;
        coinNumHUDNow = coinNumTotalNow;
        StopAllCoroutines();
        ClaimInternal(toCoinNum, coinHUDUpdateWithin, coinHUDAdditionalText);
    }
    
    void ClaimInternal(int toCoinNum, float coinHUDUpdateWithin, string coinHUDAdditionalText)
    {
        StartCoroutine(ClaimCOR());
        IEnumerator ClaimCOR()
        {
            var completedNum = 0;
            var totalNum = 0;
            if (coin2Ds != null && coin2Ds.Count > 0)
            {
                for (int i = 0; i < coin2Ds.Count; i++)
                {
                    var c = coin2Ds[i];
                    if (c == null) { continue; }
                    totalNum++;
                }
            }

            var oriScale = coinHud.localScale;
            var oriRot = coinHud.localRotation;

            twList = new List<Tweener>();
            if (coin2Ds != null && coin2Ds.Count > 0)
            {
                for (int i = 0; i < coin2Ds.Count; i++)
                {
                    var c = coin2Ds[i];
                    if (c == null) { continue; }
                    var reachTime = UnityEngine.Random.Range(particleReachWithMin, particleReachWithMax);
                    var spreadDistMin = Screen.width * spreadMin;
                    var spreadDistMax = Screen.width * spreadMax;

                    var distance = UnityEngine.Random.Range(spreadDistMin, spreadDistMax);
                    var hudReachTime = UnityEngine.Random.Range(hudReachTimeMin, hudReachTimeMax);
                    c.StartParticle(reachTime, distance, coinHud, hudReachTime, () =>
                    {
                        var twShake = coinHud.DOShakeScale(0.3f, 0.1f).OnComplete(() => {  });
                        var twRotator = coinHud.DOShakeRotation(0.2f, 0.1f).OnComplete(() => {  });
                        twList.Add(twShake);
                        twList.Add(twRotator);
                        completedNum++;
                        onCoinReachHUD?.Invoke(completedNum, totalNum);

                        if (startedToEnter == false)
                        {
                            startedToEnter = true;
                            onCoinStartedToEnterHUD?.Invoke();
                            coinHUDTw = DOTween.To(() => coinNumHUDNow, x =>
                            {
                                coinNumHUDNow = x;
                                coinNumTxt.text = coinNumHUDNow + coinHUDAdditionalText;

                            }, toCoinNum, coinHUDUpdateWithin);
                        }
                    });
                }
            }

            while (completedNum != totalNum) { yield return null; }
            _CB?.Invoke(toCoinNum);
            _CB = null;
            twList.ExResetDT();
            coinHud.localScale = oriScale;
            coinHud.localRotation = oriRot;
            gameObject.SetActive(false);
        }
    }
}