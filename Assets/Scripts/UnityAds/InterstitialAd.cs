using UnityEngine;
using UnityEngine.Advertisements;
public class InterstitialAdd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] string _androidAdId;
    [SerializeField] string _iOSAdId;

    private string _adId;

    private void Awake()
    {
#if UNITY_IOS
        _adId = _iOSAdId;
#elif UNITY_ANDROID
        _adId = _androidAdId;
#endif
    }

    public void LoadInterstitialAd()
    {
        Advertisement.Load(_adId, this);
    }

    public void ShowInterstitialAd()
    {   
    
        Advertisement.Show(_adId, this);
        LoadInterstitialAd();
    }



    #region Interstitial LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad {placementId}: {error.ToString()} - {message}");
    }
    #endregion
    
    #region Interstitial ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
       Debug.LogError($"Failed to show ad {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Ad Started: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Ad Clicked: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Ad Completed: {placementId}");
    }
    #endregion
    
}
