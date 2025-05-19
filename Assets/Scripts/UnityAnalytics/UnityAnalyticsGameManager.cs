using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnityAnalyticsGameManager : MonoBehaviour
{

    [SerializeField] private AnalyticsManager analyticsManager;

    [Header("Buttons")]
    [SerializeField] private Button levelButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button colorButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI upgradeText;

    private Color currentColor;
    private int currentLevel;
    private float currentUpgrade;
    private Camera mainCamera;
    private float sessionStartTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        currentLevel = 1;
        currentUpgrade = 0.1f;
        sessionStartTime = Time.time;

        UpdateUI();
        EventListners();
    }

     private void OnApplicationQuit()
    {
        // Log session time when the game is closed
        float sessionLength = Time.time - sessionStartTime;
        analyticsManager.LogSessionTime(sessionLength);
    }

    private void EventListners()
    {
        levelButton.onClick.AddListener(OnClickLevelButton);
        upgradeButton.onClick.AddListener(OnClickUpgradeButton);
        colorButton.onClick.AddListener(OnClickColorButton);
    }

    private void OnClickLevelButton()
    {
        // Log button click
        analyticsManager.LogButtonClick("level_button");

        currentLevel++;
        UpdateUI();
        
        // Log level up event
        analyticsManager.LogLevelUp(currentLevel);
        
        // If we reach certain milestone levels, log achievements
        if (currentLevel == 5)
        {
            analyticsManager.LogAchievementUnlocked("reach_level_5");
        }
        else if (currentLevel == 10)
        {
            analyticsManager.LogAchievementUnlocked("reach_level_10");
        }
    }

    private void OnClickUpgradeButton()
    {
        // Log button click
        analyticsManager.LogButtonClick("upgrade_button");

        currentUpgrade += 0.1f;
        UpdateUI();

        // Log upgrade event
        analyticsManager.LogUpgradePerformed(currentUpgrade);
    }

    private void OnClickColorButton()
    {
        // Log button click
        analyticsManager.LogButtonClick("color_button");

        currentColor = Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        // mainCamera.backgroundColor = currentColor;
         // Apply the color and immediately force a repaint
        mainCamera.backgroundColor = currentColor;

        // Log color change event
        analyticsManager.LogColorChanged(currentColor);
    }

    private void UpdateUI()
    {
        levelText.text = "Level " + currentLevel.ToString();
        upgradeText.text = "Upgrade " + currentUpgrade.ToString();
    }
}
