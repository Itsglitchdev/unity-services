using UnityEngine;
using System;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class NotificationManager : MonoBehaviour
{
    [SerializeField] IOSNotification iosNotification;
    [SerializeField] AndroidNotification androidNotification;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeNotifications();
        CheckForNotificationLaunch();
    }

    private void InitializeNotifications()
    {
        #if UNITY_ANDROID
        androidNotification.RequestAuthorization();
        androidNotification.RequestNotificationChannel();
        #endif
        
        #if UNITY_IOS
        StartCoroutine(iosNotification.RequestAuthorization());
        #endif
        
        Debug.Log("Notification system initialized");
    }

    // Check if the app was launched from a notification
    private void CheckForNotificationLaunch()
    {
        #if UNITY_ANDROID
        Unity.Notifications.Android.AndroidNotification notification;
        string channelId;
        
        if (androidNotification.CheckIfOpenedFromNotification(out notification, out channelId))
        {
            Debug.Log($"App opened from Android notification: {notification.Title}");
            ProcessNotificationData(notification.IntentData);
        }
        #endif
        
        #if UNITY_IOS
        iOSNotification notification;
        
        if (iosNotification.CheckIfOpenedFromNotification(out notification))
        {
            Debug.Log($"App opened from iOS notification: {notification.Title}");
            ProcessNotificationData(notification.Data);
        }
        #endif
    }

    // Process any data included in the notification
    private void ProcessNotificationData(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            Debug.Log($"Notification data: {data}");
            // TODO: Add code to process notification data
            // For example, parse JSON and award rewards, etc.
        }
    }

    // Handle app focus changes
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {   
            // Cancel notifications when app comes to foreground
            CancelAllNotifications();
        }
        else
        {
            // Optionally schedule notifications when app goes to background
            // For example, to remind the player to come back
            SendComeBackNotification("We miss you!", "Come back and continue your game!", 3);
        }
    }

    #region Cross-Platform Notification Methods

    // Send an instant notification
    public void SendInstantNotification(string title, string message)
    {
        #if UNITY_ANDROID
        androidNotification.SendInstantNotification(title, message);
        #endif
        
        #if UNITY_IOS
        iosNotification.SendInstantNotification(title, message);
        #endif
        
        Debug.Log($"Instant notification sent: {title}");
    }

    // Send a delayed notification
    public void SendDelayedNotification(string title, string message, int hours, int minutes = 0, int seconds = 0)
    {
        #if UNITY_ANDROID
        androidNotification.SendNotification(title, message, hours, minutes, seconds);
        #endif
        
        #if UNITY_IOS
        iosNotification.SendNotification(title, message, "Game Alert", hours, minutes, seconds);
        #endif
        
        Debug.Log($"Delayed notification scheduled: {title} - Time: {hours}h {minutes}m {seconds}s");
    }

    // Send a "come back" notification
    public void SendComeBackNotification(string title, string message, int hours)
    {
        #if UNITY_ANDROID
        androidNotification.SendNotification(title, message, hours);
        #endif
        
        #if UNITY_IOS
        iosNotification.SendNotification(title, message, "Come Back!", hours);
        #endif
        
        Debug.Log($"Come back notification scheduled: {title} - Time: {hours}h");
    }

    // Send a repeating notification
    public void SendRepeatingNotification(string title, string message, int intervalHours)
    {
        #if UNITY_ANDROID
        TimeSpan interval = TimeSpan.FromHours(intervalHours);
        androidNotification.SendRepeatNotification(title, message, interval);
        #endif
        
        #if UNITY_IOS
        iosNotification.SendRepeatingNotification(title, message, "Reminder", intervalHours);
        #endif
        
        Debug.Log($"Repeating notification set: {title} - Interval: {intervalHours}h");
    }

    // Cancel all pending notifications
    public void CancelAllNotifications()
    {
        #if UNITY_ANDROID
        androidNotification.CancelAllNotifications();
        #endif
        
        #if UNITY_IOS
        iosNotification.CancelAllNotifications();
        #endif
        
        Debug.Log("All notifications cancelled");
    }
    
    // Method to set badge number (primarily for iOS)
    public void SetBadgeNumber(int number)
    {
        #if UNITY_IOS
        iosNotification.SetBadgeNumber(number);
        #endif
        
        Debug.Log($"Badge number set to: {number}");
    }
    
    #endregion
}