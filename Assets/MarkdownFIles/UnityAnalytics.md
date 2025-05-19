# Unity Analytics - Simple Integration Guide

## ✅ Components Used
- **Unity Analytics Service**

## 📦 Step-by-Step Instructions

### 1. Enable Unity Analytics
- Go to **Project Settings > Services**
- Sign in to your Unity account
- Enable **Analytics** for your project
- Link your project to your organization

### 2. Create a Simple Analytics Manager Script

#### AnalyticsManager.cs
```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    private static AnalyticsManager _instance;
    public static AnalyticsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AnalyticsManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("AnalyticsManager");
                    _instance = go.AddComponent<AnalyticsManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    void Start()
    {
        // Log game start event
        LogGameStart();
    }

    public void LogGameStart()
    {
        Analytics.CustomEvent("game_start");
        Debug.Log("Analytics: Game Start logged");
    }

    public void LogButtonClick(string buttonName)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "button_name", buttonName }
        };
        
        Analytics.CustomEvent("button_clicked", eventData);
        Debug.Log($"Analytics: Button Click on {buttonName} logged");
    }

    public void LogLevelComplete(int levelNumber, float timeSpent)
    {
        Dictionary<string, object> eventData = new Dictionary<string, object>
        {
            { "level_number", levelNumber },
            { "time_spent", timeSpent }
        };
        
        Analytics.CustomEvent("level_complete", eventData);
        Debug.Log($"Analytics: Level {levelNumber} Complete logged");
    }
}
```

### 3. Create a Simple Button Handler Script

#### AnalyticsButton.cs
```csharp
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsButton : MonoBehaviour
{
    [SerializeField] private AnalyticsManager analyticsManager;
    [SerializeField] private Button testButton;
    
    private int currentLevel = 1;
    
    void Start()
    {
        // Set up the button listener
        testButton.onClick.AddListener(OnButtonClick);
        
        // Get analytics manager if not assigned
        if (analyticsManager == null)
        {
            analyticsManager = AnalyticsManager.Instance;
        }
    }
    
    void OnButtonClick()
    {
        // Log the button click
        analyticsManager.LogButtonClick("test_button");
        
        // Simulate level completion
        analyticsManager.LogLevelComplete(currentLevel, 60f);
        
        // Increment level for next click
        currentLevel++;
    }
}
```

### 4. Set Up Your Scene
1. Create an empty GameObject named "AnalyticsManager"
2. Add the `AnalyticsManager.cs` script to it
3. Create a Canvas with a Button for testing
4. Add the `AnalyticsButton.cs` script to the Button
5. Drag the AnalyticsManager to the "analyticsManager" field in the AnalyticsButton component

### 5. Build and Test
- Create a development build
- Run the build and click the button
- Events will be sent to Unity Analytics
- Check the Unity dashboard after 24-48 hours to see your events

## 📊 Viewing Your Analytics Data
1. Go to [Unity Dashboard](https://dashboard.unity3d.com)
2. Sign in with your Unity account
3. Navigate to **Analytics > Events**
4. Your custom events will appear in the list:
   - `game_start`
   - `button_clicked`
   - `level_complete`

## 🔍 Troubleshooting
- If events don't appear in 48 hours, check internet connectivity in your build
- Use Debug.Log statements to verify events are being sent
- Enable Test Mode in Project Settings for faster data processing during development

## ✅ That's it!

You've successfully implemented basic analytics tracking in your Unity game. This simple setup will track game starts, button clicks, and level completions.
