# Firebase Cloud Messaging (FCM) Unity Integration Guide

## 🔥 What is Firebase Cloud Messaging?
Firebase Cloud Messaging is a cross-platform messaging solution that lets you reliably send messages at no cost. Unlike local notifications, FCM allows you to send push notifications from a server to your app, even when the app isn't running.

## 📋 Prerequisites
- Unity 2019.4 LTS or later
- Android SDK with API level 16 or higher
- Google account for Firebase Console access

## 📦 Step-by-Step Setup

### 1. Create Firebase Project
1. Go to [Firebase Console](https://console.firebase.google.com/)
2. Click **"Create a project"** or **"Add project"**
3. Enter your project name (e.g., "MyUnityGame")
4. Choose whether to enable Google Analytics (recommended)
5. Click **"Create project"**

### 2. Register Your Android App
1. In Firebase Console, click **"Add app"** and select the **Android icon**
2. **IMPORTANT**: Enter your package name
   - For testing: Use `com.google.FirebaseUnityMessagingTestApp.dev`
   - For production: Use your actual package name (e.g., `com.yourcompany.yourgame`)
3. Enter app nickname (optional)
4. Click **"Register app"**

### 3. Generate SHA-1 Key (Critical Step)

#### Why SHA-1 is Required:
Android apps must be signed by a key, and Firebase needs this key's signature to verify your app's authenticity.

#### Steps to Generate SHA-1:
1. **Set Keystore in Unity:**
   - Go to **Edit > Project Settings**
   - Navigate to **Player > Publishing Settings**
   - **Create New Keystore:**
     - Click **"Create New Keystore"**
     - Choose location and password
   - **Create New Key:**
     - Click **"Create a new key"**
     - Fill in alias name and password
     - Remember these details!

2. **Generate SHA-1 Command:**
   ```bash
   keytool -list -v -keystore <path_to_keystore> -alias <key_name>
   ```
   
   **Example:**
   ```bash
   keytool -list -v -keystore "C:\MyProject\mykeystore.keystore" -alias mygamekey
   ```

3. **Copy SHA-1 from Output:**
   Look for line: `SHA1: XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX:XX`

4. **Add to Firebase:**
   - In Firebase Console, go to **Project Settings**
   - Scroll to **"Your apps"** section
   - Click **"Add fingerprint"**
   - Paste your SHA-1 key

### 4. Download Configuration Files
1. In Firebase Console, click **"Download google-services.json"**
2. **IMPORTANT**: Place this file in your Unity project's **Assets** folder
   - You can put it anywhere under Assets (e.g., `Assets/StreamingAssets/`)
   - Do NOT rename this file

### 5. Download Firebase Unity SDK
1. Go to [Firebase Unity SDK](https://firebase.google.com/download/unity)
2. Download the latest version
3. Extract the ZIP file to a convenient location

### 6. Import Firebase Messaging Package
1. In Unity, go to **Assets > Import Package > Custom Package**
2. Navigate to your extracted Firebase SDK folder
3. Select **FirebaseMessaging.unitypackage**
4. Click **Import All**
5. **Optional**: Also import **FirebaseAnalytics.unitypackage** for analytics

### 7. Configure Android Manifest

#### Why Manifest is Important:
The AndroidManifest.xml tells Android how to handle Firebase messages and which activity to use.

#### Create/Update AndroidManifest.xml:
**Location**: `Assets/Plugins/Android/AndroidManifest.xml`

**Note**: Please provide your specific AndroidManifest.xml file to replace this template.

**Important Notes:**
- Replace `com.yourcompany.yourgame` with your actual package name
- The `Theme.AppCompat.Light.NoActionBar` theme is required
- If missing AppCompat, add it via **Window > Package Manager > Android Support**

### 8. Create Firebase Scripts

#### FirebaseInit.cs (Initialize Firebase)
```csharp
using UnityEngine;
using TMPro; 
using Firebase;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;

    void Start()
    {
        statusText.text = "Checking Firebase dependencies...";
        
        // Check if Firebase is available on this device
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                // Firebase is ready to use
                FirebaseApp app = FirebaseApp.DefaultInstance;
                statusText.text = "✅ Firebase is ready!";
                Debug.Log("Firebase initialized successfully");
            }
            else
            {
                statusText.text = $"❌ Firebase error: {status}";
                Debug.LogError($"Could not resolve Firebase dependencies: {status}");
            }
        });
    }
}
```

#### FirebaseMessagingHandler.cs (Handle Push Notifications)
```csharp
using UnityEngine;
using Firebase.Messaging;

public class FirebaseMessagingHandler : MonoBehaviour
{
    void Start()
    {
        // Subscribe to token and message events
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
        
        // Request notification permissions (Android 13+)
        RequestNotificationPermission();
    }
    
    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        FirebaseMessaging.TokenReceived -= OnTokenReceived;
        FirebaseMessaging.MessageReceived -= OnMessageReceived;
    }

    public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log($"📱 FCM Registration Token: {token.Token}");
        // Store this token on your server to send notifications to this device
        // You can send this token to your backend server
    }

    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log($"📨 Message received from: {e.Message.From}");
        Debug.Log($"📝 Message body: {e.Message.Data}");
        
        // Handle the message (show in-game notification, update UI, etc.)
        HandleIncomingMessage(e.Message);
    }
    
    private void HandleIncomingMessage(FirebaseMessage message)
    {
        // Example: Show a simple in-game message
        if (message.Data != null && message.Data.Count > 0)
        {
            foreach (var data in message.Data)
            {
                Debug.Log($"Key: {data.Key}, Value: {data.Value}");
            }
        }
        
        // You can trigger in-game events, show popups, etc.
        ShowInGameNotification(message.Data.ContainsKey("title") ? message.Data["title"] : "New Message");
    }
    
    private void ShowInGameNotification(string message)
    {
        // Implement your in-game notification system here
        Debug.Log($"🎮 In-game notification: {message}");
    }
    
    private void RequestNotificationPermission()
    {
        // For Android 13+ (API level 33+), request POST_NOTIFICATIONS permission
        FirebaseMessaging.GetTokenAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("✅ Notification permission granted or not required");
            }
            else
            {
                Debug.LogWarning("⚠️ Failed to get FCM token - check permissions");
            }
        });
    }
}
```

### 9. Scene Setup
1. **Create UI Canvas:**
   - **GameObject > UI > Canvas**
   - Add **TextMeshPro - Text** for status display

2. **Create Firebase Manager:**
   - Create empty GameObject named "FirebaseManager"
   - Add both scripts: `FirebaseInit` and `FirebaseMessagingHandler`
   - Link the status text to FirebaseInit script

3. **Update Package Name:**
   - **File > Build Settings > Player Settings**
   - Set **Package Name** to match Firebase configuration
   - Example: `com.yourcompany.yourgame`

### 10. Build and Test

#### Build Settings:
1. **File > Build Settings**
2. Select **Android** platform
3. Click **Switch Platform**
4. **Build and Run**

#### Testing Push Notifications:
1. **Build and install** your app
2. **Launch the app** - check logs for FCM token
3. **Copy the FCM token** from the logs
4. **Test via Firebase Console:**
   - Go to **Firebase Console > Messaging**
   - Click **"Send your first message"**
   - Enter notification title and body
   - Select your app
   - **Advanced options > Additional options**
   - Paste your FCM token in **"FCM registration token"**
   - Send the notification

## 🔧 Troubleshooting

### Common Issues and Solutions:

#### 1. **App Doesn't Receive Notifications:**
```bash
# Check logs using ADB (Windows)
adb logcat | findstr /i firebase

# Check logs using ADB (Mac/Linux)
adb logcat | grep -i firebase
```
- Verify SHA-1 key is correct
- Check package name matches Firebase configuration
- Ensure google-services.json is in Assets folder

#### 2. **"AppCompat Theme" Error:**
- Install **Android Support Library** via Package Manager
- Use `Theme.AppCompat.Light.NoActionBar` in manifest

#### 3. **Build Errors:**
- **File > Build Settings > Player Settings**
- **Android > Minimum API Level**: Set to 21 or higher
- **Target API Level**: 33 or higher for Android 13+ support

#### 4. **Token Not Generated:**
- Check notification permissions in device settings
- Verify internet connectivity
- Ensure Firebase dependencies are resolved

### Testing Commands:
```bash
# View all logs
adb logcat

# Filter Firebase logs only
adb logcat | grep -i firebase

# Clear logs and start fresh
adb logcat -c && adb logcat
```

## 🎯 Key Differences from Local Notifications

| Feature | Local Notifications | Firebase FCM |
|---------|-------------------|--------------|
| **Source** | Generated locally by app | Sent from server/Firebase Console |
| **Internet** | Not required | Required |
| **Targeting** | Only current device | Any device with your app |
| **Rich Media** | Limited | Images, sounds, actions |
| **Analytics** | None | Built-in Firebase Analytics |
| **Scheduling** | App must be installed | Server-side scheduling |

## 🚀 Next Steps
- **Server Integration**: Build backend to send targeted notifications
- **User Segmentation**: Use Firebase Analytics for targeted messaging  
- **A/B Testing**: Test different notification strategies
- **Rich Notifications**: Add images, actions, and custom sounds

## 📚 Additional Resources
- [Firebase Unity Documentation](https://firebase.google.com/docs/unity/setup)
- [FCM Unity Quickstart](https://github.com/firebase/quickstart-unity/tree/master/messaging)
- [Firebase Console](https://console.firebase.google.com/)

---

**✅ Success Checklist:**
- [ ] Firebase project created
- [ ] Android app registered with correct package name
- [ ] SHA-1 key generated and added to Firebase
- [ ] google-services.json downloaded and placed in Assets
- [ ] Firebase SDK imported
- [ ] AndroidManifest.xml configured
- [ ] Scripts added and configured
- [ ] App built and FCM token received
- [ ] Test notification sent successfully