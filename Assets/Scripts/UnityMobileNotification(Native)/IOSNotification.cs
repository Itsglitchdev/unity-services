using UnityEngine;
using System.Collections;
using System;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class IOSNotification : MonoBehaviour
{
#if UNITY_IOS
    // Category identifiers for different notification types
    private const string DEFAULT_CATEGORY = "default";
    private const string REWARD_CATEGORY = "reward";
    private const string REMINDER_CATEGORY = "reminder";

    void Awake()
    {
        // Setup notification categories when the app starts
        SetupNotificationCategories();
    }

    // Setup notification categories with different actions
    void SetupNotificationCategories()
    {
        // Default category with simple actions
        var defaultCategory = new iOSNotificationCategory()
            .SetCategoryIdentifier(DEFAULT_CATEGORY)
            .SetAllowInCarPlay(true)
            .AddAction(new iOSNotificationAction()
                .SetIdentifier("open")
                .SetTitle("Open")
                .SetOptions(iOSNotificationActionOptions.Foreground))
            .AddAction(new iOSNotificationAction()
                .SetIdentifier("dismiss")
                .SetTitle("Dismiss")
                .SetOptions(iOSNotificationActionOptions.Destructive));

        // Reward category with claim action
        var rewardCategory = new iOSNotificationCategory()
            .SetCategoryIdentifier(REWARD_CATEGORY)
            .SetAllowInCarPlay(true)
            .AddAction(new iOSNotificationAction()
                .SetIdentifier("claim")
                .SetTitle("Claim")
                .SetOptions(iOSNotificationActionOptions.Foreground))
            .AddAction(new iOSNotificationAction()
                .SetIdentifier("later")
                .SetTitle("Later")
                .SetOptions(iOSNotificationActionOptions.None));

        // Reminder category
        var reminderCategory = new iOSNotificationCategory()
            .SetCategoryIdentifier(REMINDER_CATEGORY)
            .SetAllowInCarPlay(true)
            .AddAction(new iOSNotificationAction()
                .SetIdentifier("remind")
                .SetTitle("Remind Later")
                .SetOptions(iOSNotificationActionOptions.None));

        // Register all categories
        iOSNotificationCenter.SetNotificationCategories(new iOSNotificationCategory[]
        {
            defaultCategory,
            rewardCategory,
            reminderCategory
        });
    }

    // Request authorization to send notifications
    public IEnumerator RequestAuthorization()
    {
        using var request = new AuthorizationRequest(
            AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound,
            true
        );
        
        while (!request.IsFinished)
        {
            yield return null;
        }
        
        // Check if permission was granted
        if (request.Granted)
        {
            Debug.Log("iOS notification permission granted");
        }
        else
        {
            Debug.Log("iOS notification permission denied");
        }
    }

    // Send a notification with specified delay in hours
    public void SendNotification(string title, string body, string subtitle, int fireInHours, int fireInMinutes = 0, int fireInSeconds = 0)
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(fireInHours, fireInMinutes, fireInSeconds),
            Repeats = false
        };
       
        var notification = new iOSNotification()
        {
            Identifier = System.Guid.NewGuid().ToString(),
            Title = title,
            Subtitle = subtitle,
            Body = body,
            Trigger = timeTrigger,
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge | PresentationOption.Sound,
            CategoryIdentifier = DEFAULT_CATEGORY,
            ThreadIdentifier = "default",
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

    // Send an instant notification
    public void SendInstantNotification(string title, string body, string subtitle = "")
    {
        var notification = new iOSNotification()
        {
            Identifier = System.Guid.NewGuid().ToString(),
            Title = title,
            Subtitle = subtitle,
            Body = body,
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge | PresentationOption.Sound,
            CategoryIdentifier = DEFAULT_CATEGORY,
            ThreadIdentifier = "default",
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

    // Send a repeating notification
    public void SendRepeatingNotification(string title, string body, string subtitle, int intervalHours)
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(intervalHours, 0, 0),
            Repeats = true
        };
       
        var notification = new iOSNotification()
        {
            Identifier = "repeat_" + System.Guid.NewGuid().ToString(),
            Title = title,
            Subtitle = subtitle,
            Body = body,
            Trigger = timeTrigger,
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge | PresentationOption.Sound,
            CategoryIdentifier = REMINDER_CATEGORY,
            ThreadIdentifier = "reminders",
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

    // Send a calendar notification
    public void SendCalendarNotification(string title, string body, string subtitle, int year, int month, int day, int hour, int minute)
    {
        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            Year = year,
            Month = month, 
            Day = day,
            Hour = hour,
            Minute = minute,
            Repeats = false
        };
       
        var notification = new iOSNotification()
        {
            Identifier = "calendar_" + System.Guid.NewGuid().ToString(),
            Title = title,
            Subtitle = subtitle,
            Body = body,
            Trigger = calendarTrigger,
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge | PresentationOption.Sound,
            CategoryIdentifier = DEFAULT_CATEGORY,
            ThreadIdentifier = "calendar",
        };

        iOSNotificationCenter.ScheduleNotification(notification);
    }

    // Cancel all scheduled notifications
    public void CancelAllNotifications()
    {
        iOSNotificationCenter.RemoveAllScheduledNotifications();
    }

    // Cancel a specific notification by ID
    public void CancelNotification(string identifier)
    {
        iOSNotificationCenter.RemoveScheduledNotification(identifier);
    }

    // Check if app was opened via notification
    public bool CheckIfOpenedFromNotification(out iOSNotification notification)
    {
        notification = iOSNotificationCenter.GetLastRespondedNotification();
        return notification != null;
    }

    // Set the app badge number
    public void SetBadgeNumber(int number)
    {
        iOSNotificationCenter.ApplicationBadge = number;
    }
#endif
}