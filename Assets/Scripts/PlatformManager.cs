using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public bool isPC { get; private set; }
    public bool isAndroid { get; private set; }

    void Awake()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
            isPC = true;
            isAndroid = false;
        #elif UNITY_ANDROID
            isPC = false;
            isAndroid = true;
        #else
            isPC = false;
            isAndroid = false;
            Debug.LogWarning("Platform not recognized!");
        #endif

        Debug.Log("Platform detected: " + (isPC ? "PC" : isAndroid ? "Android" : "Unknown"));
    }
}