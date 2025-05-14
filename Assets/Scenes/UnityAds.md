# Unity Services Setup Guide

## ✅ Services Used
- **Unity Ads (Legacy Advertisement)**

## 📦 Step-by-Step Instructions

### 1. Open Your Unity Project
- Create or open your Unity project.

### 2. Install Unity Ads (Legacy)
- Go to **Window > Package Manager**
- Install **Advertisement (Legacy)** from the Unity Registry
- **If a pop-up appears asking to import the Android Resolver, click "No"** at this stage. We will handle it separately.

### 3. Set Up Unity Cloud
- Enable Unity Services (top-right corner of the Editor)
- Link your project to a Unity Dashboard project
- Enable **Ads** from the Unity Dashboard if not already active

### 4. Read Unity Ads Setup Documentation
Refer to the official Unity documentation to understand how to configure ads:
📖 [Unity Ads Integration Guide](https://docs.unity.com/grow/en-us/ads/unity-sdk)

### 5. Install External Dependency Manager (EDM4U)
We will now install the Android Resolver manually.
- Install EDM4U from the GitHub URL:
  ```
  https://github.com/googlesamples/unity-jar-resolver.git?path=upm
  ```

#### How:
- Open **Edit > Project Settings > Package Manager** (if needed)
- Then open **Window > Package Manager**
- Click the **+** button → **Add package from Git URL**
- Paste the URL above and confirm

### 6. Resolve Android Dependencies
- After installation, go to:
  ```
  Assets > External Dependency Manager > Android Resolver > Resolve
  ```
- Run **Resolve** once to fetch necessary dependencies

### 7. Android Resolver Settings
- Use the **default settings** in Android Resolver
- No need to change anything unless required by another SDK
- These defaults are safe and stable for building

---

✅ You're now ready to implement and run Unity Ads using the Legacy Advertisement package!
