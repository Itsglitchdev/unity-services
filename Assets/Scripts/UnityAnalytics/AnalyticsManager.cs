using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        LogGameStart();
    }

    #region Game Progression Events

    public void LogGameStart()
    {
        Analytics.CustomEvent("game_start");
        Debug.Log("Analytics: Game Start logged");
    }

    public void LogLevelUp(int newLevel)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "level", newLevel }
        };

        Analytics.CustomEvent("level_up", eventData);
        Debug.Log($"Analytics: Level Up to {newLevel} logged");
    }

    public void LogUpgradePerformed(float upgradeValue)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "upgrade_value", upgradeValue }
        };

        Analytics.CustomEvent("upgrade_performed", eventData);
        Debug.Log($"Analytics: Upgrade to {upgradeValue} logged");
    }

    public void LogColorChanged(Color color)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "color_r", color.r },
            { "color_g", color.g },
            { "color_b", color.b }
        };

        Analytics.CustomEvent("color_changed", eventData);
        Debug.Log("Analytics: Color Change logged");
    }

    #endregion

    #region Player Behavior Events

    public void LogButtonClick(string buttonName)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "button_name", buttonName }
        };

        Analytics.CustomEvent("button_clicked", eventData);
        Debug.Log($"Analytics: Button Click on {buttonName} logged");
    }

    public void LogSessionTime(float timeInSeconds)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "session_time", timeInSeconds }
        };

        Analytics.CustomEvent("session_time", eventData);
        Debug.Log($"Analytics: Session time of {timeInSeconds} seconds logged");
    }

    #endregion

     #region Custom Business Events

    public void LogAchievementUnlocked(string achievementId)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "achievement_id", achievementId }
        };

        Analytics.CustomEvent("achievement_unlocked", eventData);
        Debug.Log($"Analytics: Achievement {achievementId} unlocked logged");
    }

    public void LogInAppPurchase(string itemId, float price, string currency)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "item_id", itemId },
            { "price", price },
            { "currency", currency }
        };

        Analytics.CustomEvent("in_app_purchase", eventData);
        Debug.Log($"Analytics: In-app purchase of {itemId} for {price} {currency} logged");
    }
    
    #endregion

}