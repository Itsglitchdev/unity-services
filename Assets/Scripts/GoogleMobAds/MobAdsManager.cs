using UnityEngine;
using GoogleMobileAds.Api;
using System;
using GoogleMobileAds.Common;


public class MobAdsManager : MonoBehaviour
{
    public static MobAdsManager Instance;
    private Action onUserRewardedCallback;

#if UNITY_ANDROID
    private string adUnitId = "ca-app-pub-9445194636847953~1496649538";
#elif UNITY_IOS
    string adUnitId = "";
#endif


#if UNITY_ANDROID
    string bannerAdUnitId = "ca-app-pub-9445194636847953/1713090198";
    string interstitialAdUnitId = "ca-app-pub-9445194636847953/7918571743";
    string rewardedAdUnitId = "ca-app-pub-9445194636847953/1142244557";
    string nativeAdUnitId = "ca-app-pub-9445194636847953/2627325429";
#elif UNITY_IOS
    string bannerAdUnitId = "";
    string interstitialAdUnitId = "";
    string rewardedAdUnitId = "";
    string nativeAdUnitId = "";    
#endif

    BannerView bannerView;
    InterstitialAd interstitial;
    RewardedAd rewardedAd;
    NativeOverlayAd nativeOverlayAd;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdsInitialization status: " + initStatus);
        });
    }

    #region Banner

    public void ShowBannerAd()
    {
        CreateBannerView();

        var request = new AdRequest();
        request.Keywords.Add("unity-admob-sample");
        Debug.Log("Loading banner ad.");
        bannerView.LoadAd(request);
    }


    private void CreateBannerView()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Top);

        // Subscribe to events immediately
        SubscribeToBannerEvents();
    }


    private void SubscribeToBannerEvents()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
         {
             Debug.Log("Banner view full screen content opened.");
         };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }


    public void HideBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
            Debug.Log("Banner ad destroyed.");
        }
        else
        {
            Debug.LogWarning("HideBannerAd() called but no banner exists.");
        }
    }


    #endregion

    #region Interstitial

    public void LoadInterstitialAd()
    {
        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(interstitialAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial failed to load: " + error);
                    return;
                }

                Debug.Log("Interstitial loaded successfully.");
                interstitial = ad;
                SubscribeToInterstitialEvents(interstitial);

            });
    }

    public void ShowInterstitialAd()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
        }
        else
        {
            Debug.LogWarning("Interstitial ad is not ready yet.");
        }

    }

    private void SubscribeToInterstitialEvents(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad opened.");
        };

        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad closed.");

            // Ad is one-time use; load another
            interstitial = null;
            LoadInterstitialAd();
        };

        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad impression recorded.");
        };

        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad clicked.");
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to show with error: " + error.GetMessage());
            interstitial = null;
            LoadInterstitialAd();
        };

        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Interstitial ad paid: {adValue.Value} {adValue.CurrencyCode}");
        };
    }

    #endregion

    #region Rewarded

    public void LoadRewardedAd()
    {
        AdRequest adRequest = new AdRequest();
        RewardedAd.Load(rewardedAdUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded successfully.");
                rewardedAd = ad;
                SubscribeToRewardedEvents(rewardedAd);
            });
    }

    public void ShowRewardedAd(Action onRewardedCallback)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
        {
            // TODO: Reward the user.
            onRewardedCallback.Invoke();
            onUserRewardedCallback = null;
            Debug.Log("User rewarded with " + reward.Amount + " " + reward.Type);
        });
        }
        else
        {
            Debug.LogWarning("Rewarded ad is not ready yet.");
        }
    }

    private void SubscribeToRewardedEvents(RewardedAd rewardedAd)
    {
        rewardedAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad opened.");
        };

        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad closed.");

            // Ad is one-time use; load another
            rewardedAd = null;
            LoadRewardedAd();
        };

        rewardedAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"Rewarded ad paid: {adValue.Value} {adValue.CurrencyCode}");
        };
    }

    #endregion

    #region NativeOverlay

    public void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (nativeOverlayAd != null)
        {
            DestroyAd();
        }

        Debug.Log("Loading native overlay ad.");

        var adRequest = new AdRequest();

        var options = new NativeAdOptions
        {
            AdChoicesPlacement = AdChoicesPlacement.BottomRightCorner,
            MediaAspectRatio = MediaAspectRatio.Any
        };

        // Send the request to load the ad.
        NativeOverlayAd.Load(nativeAdUnitId, adRequest, options,
            (NativeOverlayAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Native Overlay ad failed to load an ad " +
                               " with error: " + error);
                return;
            }

            // The ad should always be non-null if the error is null, but
            // double-check to avoid a crash.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Native Overlay ad load event " +
                               " fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            Debug.Log("Native Overlay ad loaded with response : " +
                       ad.GetResponseInfo());
            nativeOverlayAd = ad;


        });
    }

    public void ShowAd()
    {
        if (nativeOverlayAd != null)
        {
            Debug.Log("Showing Native Overlay ad.");
            nativeOverlayAd.Show();
        }
    }

    public void HideAd()
    {
        if (nativeOverlayAd != null)
        {
            Debug.Log("Hiding Native Overlay ad.");
            nativeOverlayAd.Hide();
        }
    }

    public void DestroyAd()
    {
        if (nativeOverlayAd != null)
        {
            Debug.Log("Destroying native overlay ad.");
            nativeOverlayAd.Destroy();
            nativeOverlayAd = null;
        }
    }

    #endregion


}
