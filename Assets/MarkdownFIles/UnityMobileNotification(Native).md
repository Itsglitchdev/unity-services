# Unity Mobile Notifications - Simple Integration Guide

## ✅ Components Used
- **Unity Mobile Notifications Package**

## 📦 Step-by-Step Instructions

### 1. Install Mobile Notifications Package
- Go to **Window > Package Manager**
- Click the **+** button
- Select **Add package by name**
- Enter `com.unity.mobile.notifications`
- Click **Add**

### 2. Configure Project Settings
- Go to **Edit > Project Settings**
- Find **Mobile Notifications** in the left panel
- For Android:
  - ✅ Enable **Reschedule on Device Restart**
  - Set **Schedule at exact time** to **Everything**
  - Add notification icons (name them `icon_small` and `icon_large`)

### 3. Create These Three Simple Scripts

#### AndroidNotification.cs
```csharp
using UnityEngine;
using System;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class AndroidNotification : MonoBehaviour
{
#if UNITY_ANDROID
    private const string CHANNEL_ID = "game_channel";
    
    void Start()
    {
        // Create the notification channel
        var channel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_ID,
            Name = "Game Notifications",
            Importance = Importance.Default,
            Description = "Notifications for game events"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    
    public void SendInstantNotification(string title, string message)
    {
        var notification = new Unity.Notifications.Android.AndroidNotification
        {
            Title = title,
            Text = message,
            SmallIcon = "icon_small",
            LargeIcon = "icon_large",
            FireTime = DateTime.Now
        };
        
        AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
        Debug.Log("Android notification sent: " + title);
    }
#endif
}
```

#### IOSNotification.cs
```csharp
using UnityEngine;
using System.Collections;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class IOSNotification : MonoBehaviour
{
#if UNITY_IOS
    void Start()
    {
        RequestAuthorization();
    }
    
    void RequestAuthorization()
    {
        using (var authRequest = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound, true))
        {
            while (!authRequest.IsFinished) { }
            
            if (authRequest.Granted)
                Debug.Log("iOS Authorization Request Granted");
            else
                Debug.Log("iOS Authorization Request Denied");
        }
    }
    
    public void SendInstantNotification(string title, string message)
    {
        var notification = new iOSNotification
        {
            Identifier = "_notification",
            Title = title,
            Body = message,
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
        };
        
        iOSNotificationCenter.ScheduleNotification(notification);
        Debug.Log("iOS notification sent: " + title);
    }
#endif
}
```

#### NotificationManager.cs
```csharp
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private AndroidNotification androidNotification;
    [SerializeField] private IOSNotification iosNotification;
    
    public void SendNotification(string title, string message)
    {
#if UNITY_ANDROID
        androidNotification.SendInstantNotification(title, message);
#elif UNITY_IOS
        iosNotification.SendInstantNotification(title, message);
#else
        Debug.Log("Platform not supported for notifications");
#endif
    }
}
```

### 4. Simple Button Setup

#### Create a Basic UI
1. Create a Canvas: **GameObject > UI > Canvas**
2. Add a Button: **GameObject > UI > Button**
3. Position the button where you want it on screen

#### NotificationButton.cs
```csharp
using UnityEngine;
using UnityEngine.UI;

public class NotificationButton : MonoBehaviour
{
    [SerializeField] private NotificationManager notificationManager;
    [SerializeField] private Button notificationButton;
    
    void Start()
    {
        notificationButton.onClick.AddListener(OnButtonClick);
    }
    
    void OnButtonClick()
    {
        notificationManager.SendNotification("Hello Player!", "This is a test notification from our game!");
    }
}
```

### 5. Set Up Scene
1. Create an empty GameObject named "NotificationSystem"
2. Add the three scripts to it:
   - AndroidNotification.cs
   - IOSNotification.cs  
   - NotificationManager.cs
3. In the Inspector for NotificationManager:
   - Drag the AndroidNotification component to the "Android Notification" field
   - Drag the IOSNotification component to the "IOS Notification" field
4. Add NotificationButton script to your button
5. Drag NotificationManager to the "Notification Manager" field in the NotificationButton component

### 6. Build and Test
- Build for Android or iOS
- Press the button in your game
- You should see the notification appear immediately

## ✅ That's it!

This is the most straightforward way to implement mobile notifications. The notification will appear as soon as you tap the button in your game.

## 🔍 Troubleshooting
- If notifications don't work on Android 13+, check that notification permissions are granted in device settings
- For iOS, make sure to allow notifications when prompted during first app launch