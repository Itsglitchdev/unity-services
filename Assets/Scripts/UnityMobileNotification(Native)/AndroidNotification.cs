using UnityEngine;
using System;
#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif
public class AndroidNotification : MonoBehaviour
{
#if UNITY_ANDROID

    // Different notification channels for various purposes
    private const string DEFAULT_CHANNEL = "default";
    private const string REWARD_CHANNEL = "rewards";
    private const string REMINDER_CHANNEL = "reminders";


    // Request Authorize to send the notifications
    public void RequestAuthorization()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    // Register a channel
    public void RequestNotificationChannel()
    {
        // Default notification channel
        var defaultChannel = new AndroidNotificationChannel
        {
            Id = DEFAULT_CHANNEL,
            Name = "Default",
            Importance = Importance.Default,
            Description = "Generic notifications"
        };
        AndroidNotificationCenter.RegisterNotificationChannel(defaultChannel);

        // Reminder notification channel
        var reminderChannel = new AndroidNotificationChannel
        {
            Id = REMINDER_CHANNEL,
            Name = "Reminders",
            Importance = Importance.Low,
            Description = "Reminder notifications",
            EnableVibration = true
        };
        AndroidNotificationCenter.RegisterNotificationChannel(reminderChannel);
    }

    // Send notification with specific time delay
    public void SendNotification(string title, string message, int fireInHours, int fireInMinutes = 0, int fireInSeconds = 0)
    {
        var notification = new Unity.Notifications.Android.AndroidNotification
        {
            Title = title,
            Text = message,
            FireTime = DateTime.Now.AddHours(fireInHours).AddMinutes(fireInMinutes).AddSeconds(fireInSeconds),
            SmallIcon = "icon_small",
            LargeIcon = "icon_large"
        };

        AndroidNotificationCenter.SendNotification(notification, DEFAULT_CHANNEL);
    }

    // Send an instant notification
    public void SendInstantNotification(string title, string message)
    {
        var notification = new Unity.Notifications.Android.AndroidNotification
        {
            Title = title,
            Text = message,
            FireTime = DateTime.Now,
            SmallIcon = "icon_small",
            LargeIcon = "icon_large"
        };

        AndroidNotificationCenter.SendNotification(notification, DEFAULT_CHANNEL);
    }
    

    // Send a reminder notification with repeat
    public void SendRepeatNotification(string title, string message, TimeSpan repeatInterval)
    {
        var notification = new Unity.Notifications.Android.AndroidNotification
        {
            Title = title,
            Text = message,
            FireTime = DateTime.Now.Add(repeatInterval),
            RepeatInterval = repeatInterval,
            SmallIcon = "icon_small",
            LargeIcon = "icon_large"
        };

        AndroidNotificationCenter.SendNotification(notification, REMINDER_CHANNEL);
    }

    // Cancel all pending notifications
    public void CancelAllNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }

    // Cancel a specific notification by ID
    public void CancelNotification(int notificationId)
    {
        AndroidNotificationCenter.CancelNotification(notificationId);
    }

    // Check if app was opened via notification
    public bool CheckIfOpenedFromNotification(out Unity.Notifications.Android.AndroidNotification notification, out string channelId)
    {
        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null)
        {
            notification = notificationIntentData.Notification;
            channelId = notificationIntentData.Channel;
            return true;
        }
        
        notification = new Unity.Notifications.Android.AndroidNotification();
        channelId = "";
        return false;
    }
    
    #endif
}
