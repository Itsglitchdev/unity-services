using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdId;
    [SerializeField] string _iOSAdId;
    
    // Reference to GameManager to call reward method
    private UnityAdGameManager gameManager;

    private string _adId;

    private void Awake()
    {
#if UNITY_IOS
        _adId = _iOSAdId;
#elif UNITY_ANDROID
        _adId = _androidAdId;
#endif
        
        // Get reference to GameManager
        gameManager = FindFirstObjectByType<UnityAdGameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
        }
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(_adId, this);
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(_adId, this);
    }

    #region Rewarded LoadAds Callbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Rewarded Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load rewarded ad {placementId}: {error.ToString()} - {message}");
        
        // Try loading again
        LoadRewardedAd();
    }
    #endregion

    #region Rewarded ShowAds Callbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Rewarded Ad Failed");
        
        // Try loading again
        LoadRewardedAd();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Rewarded Ad Started");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Rewarded Ad Clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == _adId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Rewarded Ad Completed");
            
            // Award the player with +2 points
            if (gameManager != null)
            {
                gameManager.OnRewardedAdCompleted();
            }
            
            // Load another rewarded ad
            LoadRewardedAd();
        }
    }
    #endregion
}