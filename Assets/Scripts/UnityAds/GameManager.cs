using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("UI Reference")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button addButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button rewardButton;

    [Header("Game Settings")]
    [SerializeField] private int[] levelGoals = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };


    // Game state variables
    private int currentScore = 0;
    private int currentLevel = 0;
    private int currentGoal = 10;

    private void Awake()
    {
       
    }

    void Start()
    {
        EventListners();
        ShowMenuPanel();
        UpdateScoreText();
    }

    void EventListners()
    {
        startButton.onClick.AddListener(() => StartGame());
        addButton.onClick.AddListener(() => AddScore());
        minusButton.onClick.AddListener(() => MinusScore());
        rewardButton.onClick.AddListener(() => ShowRewardedAd());
    }

    private void ShowMenuPanel()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);

        // Show banner ad on menu
        StartCoroutine(DisplayBannerAd());
    }

    private IEnumerator DisplayBannerAd()
    { 
        yield return new WaitForSeconds(1f);
        AdsManager.Instance.bannerAd.ShowBannerAd();
    }

    private void ShowGamePanel()
    {
        AdsManager.Instance.bannerAd.HideBannerAd();
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    void StartGame()
    {
        ShowGamePanel();
        currentScore = 0;
        currentLevel = 0;
        currentGoal = levelGoals[currentLevel];
        UpdateScoreText();
    }

    private void AddScore()
    {
        currentScore++;
        UpdateScoreText();
        CheckLevelComplete();
    }
    
    private void MinusScore()
    {
        if (currentScore > 0)
        {
            currentScore--;
            UpdateScoreText();
        }
    }

    private void ShowRewardedAd()
    {
        // Show rewarded ad
        AdsManager.Instance.rewardedAds.ShowRewardedAd();
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
        AdsManager.Instance.interstitialAd.ShowInterstitialAd();
        
        // Reset score and update goal for next level
        currentScore = 0;
        currentGoal = levelGoals[currentLevel];
        UpdateScoreText();
    }

    public void OnRewardedAdCompleted()
    {
        // Add +2 to the score when rewarded ad is completed
        currentScore += 5;
        UpdateScoreText();
        CheckLevelComplete();
    }

    private void ShowGameComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{currentScore}/{currentGoal}";
    }
}
