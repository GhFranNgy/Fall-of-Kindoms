using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickHandle;
    private Vector2 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        pos.x = (pos.x / (transform as RectTransform).sizeDelta.x);
        pos.y = (pos.y / (transform as RectTransform).sizeDelta.y);

        inputVector = new Vector2(pos.x * 2, pos.y * 2);
        inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        joystickHandle.anchoredPosition = new Vector2(
            inputVector.x * ((transform as RectTransform).sizeDelta.x / 2),
            inputVector.y * ((transform as RectTransform).sizeDelta.y / 2)
        );
    }

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal() => inputVector.x;
    public float Vertical() => inputVector.y;
}