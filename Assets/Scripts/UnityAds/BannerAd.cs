using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
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

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_adId, options);
    }



    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = BannerShown,
            hideCallback = BannerHidden,
            clickCallback = BannerClicked
        };

        Advertisement.Banner.Show(_adId, options);
    }



    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    #region Callbacks
    private void OnBannerError(string message)
    {
        Debug.Log("Banner Error: " + message);
    }

    private void OnBannerLoaded()
    {
        Debug.Log("Banner Loaded");
    }

    private void BannerClicked()
    {
        Debug.Log("Banner Clicked");
    }

    private void BannerHidden()
    {
        Debug.Log("Banner Hidden");
    }

    private void BannerShown()
    {
        Debug.Log("Banner Shown");
    }
    #endregion
}
