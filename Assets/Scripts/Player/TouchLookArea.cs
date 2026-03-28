using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLookArea : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private Vector2 inputDelta;

    public float sensitivity = 0.2f;

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