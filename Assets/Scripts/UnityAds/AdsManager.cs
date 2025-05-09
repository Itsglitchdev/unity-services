using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public AdsInitializer adsInitializer;
    public InterstitialAd interstitialAd;
    public RewardedAds rewardedAds;
    public BannerAd bannerAd;

    public static AdsManager Instance { get; private set; }

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

        bannerAd.LoadBannerAd();
        interstitialAd.LoadInterstitialAd();
        rewardedAds.LoadRewardedAd();

    }

}
