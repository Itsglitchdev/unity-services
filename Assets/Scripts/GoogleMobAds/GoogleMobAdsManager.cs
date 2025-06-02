using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoogleMobAdsManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gamePanel;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button addOneButton;
    [SerializeField] private Button minusOneButton;
    [SerializeField] private Button rewardButton;
    [SerializeField] private Button nativeOverLayButton;

    [Header("Text and Image")]
    [SerializeField] private TextMeshProUGUI coinText;
    // [SerializeField] private Image nativeImage;

    [Header("Game Settings")]
    [SerializeField] private int[] levelGoals = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

    // Game state variables
    private int currentScore = 0;
    private int currentLevel = 0;
    private int currentGoal = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IntializeGame();
        EventListners();
    }

    private void IntializeGame()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        MobAdsManager.Instance.ShowBannerAd();
    }

    private void EventListners()
    {
        startButton.onClick.AddListener(StartGame);
        addOneButton.onClick.AddListener(AddOneCoin);
        minusOneButton.onClick.AddListener(MinusOneCoin);
        rewardButton.onClick.AddListener(ShowRewardedAd);
        nativeOverLayButton.onClick.AddListener(ShowNativeAd);
    }

    private void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        currentScore = 0;
        currentLevel = 0;
        currentGoal = levelGoals[currentLevel];
        MobAdsManager.Instance.HideBannerAd();

        // Preloading interstitial ad
        MobAdsManager.Instance.LoadInterstitialAd();
        MobAdsManager.Instance.LoadRewardedAd();
        MobAdsManager.Instance.LoadAd();
    }

    private void AddOneCoin()
    {
        currentScore += 1;
        UpdateCoinText();
        CheckLevelComplete();
    }

    private void MinusOneCoin()
    {
        if (currentScore > 0)
        {
            currentScore -= 1;
            UpdateCoinText();
        }
    }

    private void CheckLevelComplete()
    {
        if (currentScore >= currentGoal)
        {
            currentLevel++;
            if (currentLevel >= levelGoals.Length)
            {
                // Game completed, reset to first level
                currentLevel = 0;
                ShowGameComplete();
            }
            else
            {
                // Show interstitial ad and proceed to next level
                ShowLevelComplete();
            }
        }
    }

    private void ShowLevelComplete()
    {
        // Show interstitial ad
        MobAdsManager.Instance.ShowInterstitialAd();

        // Reset score and update goal for next level
        currentScore = 0;
        currentGoal = levelGoals[currentLevel];
        UpdateCoinText();
    }

    private void ShowRewardedAd()
    {
        // Show rewarded ad
        MobAdsManager.Instance.ShowRewardedAd(OnUserReward);
    }

    private void ShowNativeAd()
    {
        MobAdsManager.Instance.ShowAd();
    }


    private void OnUserReward()
    {
        currentScore += 5;
        UpdateCoinText();
        CheckLevelComplete();
    }

    private void ShowGameComplete()
    {
        // Show interstitial ad
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateCoinText()
    {
        coinText.text = $"{currentScore}/{currentGoal}".ToString();
    }

}
