using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-9000)]
public class ReviewPopupControl : MonoBehaviour
{
    [SerializeField] bool featureEnabled = false;
    static ReviewPopupControl instance;
    public static ReviewPopupControl Instance { get { return instance; } }
    private void Awake()
    {
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
    }

    #region ReviewRating
    static bool ShowReview = false;
    public void TryShowReviewPanel()
    {
        if (featureEnabled == false) { return; }
        if (ShowReview)
        {

            ShowReview = false;

#if UNITY_EDITOR
            Debug.Log("Showing Review");
#endif

#if UNITY_IOS
            UnityEngine.iOS.Device.RequestStoreReview();
#endif

#if UNITY_ANDROID
            //todo
#endif

        }
    }

    const string reviewkey = "G_STORE_REVIEW";
    public void IncrementReviewRelatedData()
    {
        if (featureEnabled == false) { return; }
        int gamesPlayed = PlayerPrefs.GetInt(reviewkey) + 1;
        PlayerPrefs.SetInt(reviewkey, gamesPlayed);

        if (gamesPlayed < 10 && gamesPlayed % 5 == 0) ShowReview = true;
        else if (gamesPlayed % 10 == 0) ShowReview = true;
    }
    #endregion
}
