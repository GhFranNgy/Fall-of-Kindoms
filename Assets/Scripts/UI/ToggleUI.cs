using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject object_to_toggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetUIEnabled()
    {
        object_to_toggle.SetActive(true);
    }
    public void SetUIDisabled()
    {
        object_to_toggle.SetActive(false);
    }
}
