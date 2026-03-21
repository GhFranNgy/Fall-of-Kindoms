using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAnimatorBridge : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    ISelectHandler,
    IDeselectHandler
{
    [Header("Animator to Control")]
    public Animator targetAnimator;

    // Animator Parameters must equal the parameter names of Animator
    [Header("Animator Parameters")]
    public string normalParam = "Normal";
    public string hoverParam = "Highlighted";
    public string pressedParam = "Pressed";
    public string selectedParam = "Selected";
    public string disabledParam = "Disabled";

    // Set Button to normal Animation at Start
    private void Start()
    {
        targetAnimator.SetBool(normalParam, true);
    }
    // Set Hover Animation when MousePointer enters Button
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetAnimator.SetBool(normalParam, false);
        targetAnimator.SetBool(hoverParam, true);
    }
    // Set Hover Animation when MousePointer exits Button
    public void OnPointerExit(PointerEventData eventData) 
    {
        targetAnimator.SetBool(hoverParam, false);
        targetAnimator.SetBool(normalParam, true);
    }
    // Set Pressed Animation when MousePointer presses Button
    public void OnPointerDown(PointerEventData eventData)
    {
        targetAnimator.SetBool(hoverParam, false);
        targetAnimator.SetBool(pressedParam, true);
    }
    // return to highlighted Animation when MousePointer releases Button
    public void OnPointerUp(PointerEventData eventData)
    {
        targetAnimator.SetBool(pressedParam, false);
        targetAnimator.SetBool(hoverParam, true);
    }
    // Set selected Animation after MousePointer Leaves Button
    public void OnSelect(BaseEventData eventData)
    {
        targetAnimator.SetBool(hoverParam, false);
        targetAnimator.SetBool(selectedParam, true);
        
    }
    // Set normal Animation when Button is nolonger Selected
    public void OnDeselect(BaseEventData eventData)
    {
        targetAnimator.SetBool(selectedParam, false);
        targetAnimator.SetBool(normalParam, true);
    }
}