using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLookArea : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public UserSettings userSettings;
    private float sensitivity;
    private Vector2 inputDelta;

    void Start() 
    {
        sensitivity = userSettings.touchSensitivity;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        inputDelta = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        inputDelta = eventData.delta * sensitivity;
    }

    public Vector2 GetInput()
    {
        return inputDelta;
    }

    void LateUpdate()
    {
        // reset every frame so it doesn't accumulate
        inputDelta = Vector2.zero;
    }
}