using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UnityMobileNotificationGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Button startButton;
    [SerializeField] Button healthDecreaseButton;
    [SerializeField] Button callInstantNotificationButton;
    [SerializeField] Button callDelayedNotificationButton;
    [SerializeField] Slider healthSlider;

    [Header("Game Panel")]
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject gamePanel;

    [Header("Notification References")]
    [SerializeField] NotificationManager notificationManager;

    [Header("Daily Notification Settings")]
    [SerializeField] int goodMorningHour = 8;
    [SerializeField] int goodMorningMinute = 0;

    private bool isGameStarted = false;
    private int health = 0;
    private int maxHealth = 100;
    private int healthDecreaseAmount = 10;
    private bool healthNotificationSent = false;

    void Start()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);

        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        EventListeners();

        ScheduleDailyGoodMorningNotification();
    }

    void Update()
    {
        if (isGameStarted && health == 0)
        {
            // Start recovery only if health is 0
            StartCoroutine(RecoverHealth());
        }
    }


    void EventListeners()
    {
        startButton.onClick.AddListener(StartGame);
        healthDecreaseButton.onClick.AddListener(DecreaseHealth);
        callInstantNotificationButton.onClick.AddListener(CallInstantNotification);
        callDelayedNotificationButton.onClick.AddListener(CallDelayedNotification);
    }

    void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        isGameStarted = true;
        health = maxHealth / 2;
        healthSlider.value = health;
        healthNotificationSent = false;
    }

    void DecreaseHealth()
    {
        if (isGameStarted)
        {
            health -= healthDecreaseAmount;

            if (health > 0 && health <= healthDecreaseAmount)
            {
                health = 0;
            }

            if (health < 0)
            {
                health = 0;
            }

            healthSlider.value = health;

            if (health < maxHealth)
            {
                healthNotificationSent = false;
            }

        }
    }

    private bool isRecovering = false;

    private IEnumerator RecoverHealth()
    {
        if (isRecovering) yield break; // Prevent multiple coroutines

        isRecovering = true;

        while (health < maxHealth)
        {
            health += 2;
            if (health > maxHealth) health = maxHealth;

            healthSlider.value = health;

            Debug.Log("Health increased by 2: " + health);

            yield return new WaitForSeconds(2f); // Wait before next increment
        }

        if (!healthNotificationSent)
        {
            notificationManager.SendInstantNotification("Full Health!", "Your health has reached 100%!");
            healthNotificationSent = true;
        }

        isRecovering = false;
    }




    void CallInstantNotification()
    {
        if (isGameStarted)
        {
            notificationManager.SendInstantNotification("Instant Alert", "This is an immediate notification!");
        }
    }

    void CallDelayedNotification()
    {
        if (isGameStarted)
        {
            notificationManager.SendDelayedNotification("Delayed Alert", "This notification was scheduled for later.", 0, 0, 30);
        }
    }

    // Schedule a daily good morning notification
    private void ScheduleDailyGoodMorningNotification()
    {
        // Calculate time until next morning notification
        DateTime now = DateTime.Now;
        DateTime scheduledTime = new DateTime(now.Year, now.Month, now.Day, goodMorningHour, goodMorningMinute, 0);

        // If the scheduled time has already passed today, set it for tomorrow
        if (scheduledTime <= now)
        {
            scheduledTime = scheduledTime.AddDays(1);
        }

        // Calculate time difference
        TimeSpan timeUntilNotification = scheduledTime - now;

        // Schedule the notification
#if UNITY_ANDROID || UNITY_IOS
        notificationManager.SendDelayedNotification(
            "Good Morning!",
            "Rise and shine! Don't forget to check your game today.",
            timeUntilNotification.Hours,
            timeUntilNotification.Minutes,
            timeUntilNotification.Seconds);

        // Also schedule it as a repeating notification for future days
#if UNITY_ANDROID
        int repeatIntervalHours = 24; // 24 hours = 1 day
        notificationManager.SendRepeatingNotification(
            "Good Morning!",
            "Rise and shine! Don't forget to check your game today.",
            repeatIntervalHours);
#endif

#if UNITY_IOS
        notificationManager.SendRepeatingNotification(
            "Good Morning!", 
            "Rise and shine! Don't forget to check your game today.", 
            24); // 24-hour interval
#endif

        Debug.Log($"Daily good morning notification scheduled for {scheduledTime}");
#endif
    }
}