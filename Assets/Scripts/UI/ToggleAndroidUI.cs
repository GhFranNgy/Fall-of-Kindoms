using UnityEngine;

public class ToggleAndroidUI : MonoBehaviour
{
    
    public GameObject AndroidUI;
    private PlatformManager platformManager;
    void Start()
    {
        platformManager = GetComponent<PlatformManager>();
        
        if(!platformManager.isAndroid && platformManager.isPC)
        {
            AndroidUI.SetActive(false);
        }
        else
        {
            AndroidUI.SetActive(true);
        }
    }
}
