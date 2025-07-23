# Google AdMob Unity Integration Guide

## ✅ Services Used
- **Google AdMob (Mobile Advertisement)**

## 📦 Step-by-Step Instructions

### 1. Download and Import AdMob Unity Package
- Download the latest Google Mobile Ads Unity plugin from:
  ```
  https://github.com/googleads/googleads-mobile-unity/releases/tag/v10.2.0
  ```
- Import the `.unitypackage` file into your Unity project
- **Accept all import prompts** when the package installation dialog appears

### 2. Force Resolve Dependencies
- After importing, go to:
  ```
  Assets > External Dependency Manager > Android Resolver > Force Resolve
  ```
- This will download and configure all necessary Android dependencies
- Wait for the resolution process to complete

### 3. Configure AdMob Settings in Unity
- Navigate to:
  ```
  Edit > Google AdMob > Settings
  ```
- Enter your **App IDs** for both Android and iOS platforms

### 4. Create AdMob Account and Get IDs
#### Create AdMob Account:
1. Go to [Google AdMob Console](https://admob.google.com/)
2. Sign in with your Google account
3. Create a new app or select existing app

#### Get Required IDs:
- **App ID**: Found in AdMob console under "App settings"
- **Ad Unit IDs**: Create different ad units for:
  - Banner Ads
  - Interstitial Ads  
  - Rewarded Ads
  - Native Ads (if needed)

### 5. Follow Official Documentation
📖 **Important**: Follow the official Google AdMob Unity documentation for detailed implementation:
- [AdMob Unity Plugin Documentation](https://developers.google.com/admob/unity/start)
- Study the integration patterns and best practices
- Implement ad loading, showing, and callback handling

### 6. Create AdMob Scripts
Create your advertisement scripts following the official examples:
- Initialize the Mobile Ads SDK
- Load and show Banner ads
- Load and show Interstitial ads
- Load and show Rewarded ads
- Handle ad events and callbacks

### 7. Testing Setup

#### Editor Testing:
- Ads work in Unity Editor for basic testing
- Use test ad unit IDs provided in documentation during development

#### Device Testing:
For testing on actual devices, you need to add test devices:

1. **Add Test Device in AdMob Console:**
   ```
   AdMob Console > Settings > Test devices > Add test device
   ```

2. **Get Device Advertising ID:**
   - Use the advertising ID from the phone{Settings>Google>Ads}
   - Or find your device ID through Android logcat when running the app

3. **Add Device ID in Code:**
   ```csharp
   using GoogleMobileAds.Api;
   
   // Add test device ID
   RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
       .SetTestDeviceIds(new List<string> { "DEVICE_ID" })
       .build();
   MobileAds.SetRequestConfiguration(requestConfiguration);
   ```

### 8. Important Notes

#### Development vs Production:
- **Always use test ad unit IDs during development**
- Switch to real ad unit IDs only when publishing
- Never click on your own ads in production

#### Device ID Collection:
- You can find device advertising ID through logcat:
  ```
  adb logcat | grep "advertising ID"
  ```
- Or check Unity console logs when initializing AdMob

#### Build Settings:
- Ensure proper Android permissions are set
- Configure proper build settings for ad networks
- Test thoroughly on different devices before release

---

## 📋 Quick Checklist
- [ ] Downloaded and imported AdMob Unity package v10.2.0
- [ ] Forced resolve Android dependencies  
- [ ] Created AdMob account and app
- [ ] Obtained App ID and Ad Unit IDs
- [ ] Configured AdMob settings in Unity Editor
- [ ] Added test device IDs for testing
- [ ] Implemented ad scripts following official documentation
- [ ] Tested ads in editor and on device

---

✅ **You're now ready to implement and monetize with Google AdMob ads in your Unity project!**

## 🔗 Useful Links
- [Google AdMob Console](https://admob.google.com/)
- [AdMob Unity Documentation](https://developers.google.com/admob/unity/start)
- [AdMob Unity GitHub](https://github.com/googleads/googleads-mobile-unity)
