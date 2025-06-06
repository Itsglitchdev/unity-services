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

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                statusText.text = "✅ Firebase is ready!";
            }
            else
            {
                statusText.text = $"❌ Firebase error: {status}";
                Debug.LogError("Could not resolve Firebase dependencies: " + status);
            }
        });
    }
}
