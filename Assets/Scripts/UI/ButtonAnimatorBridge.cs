using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAnimatorBridge : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler/*,
    ISelectHandler,
    IDeselectHandler*/
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

    private bool isPointerOn;

    // Set Button to normal Animation at Start
    private void Start()
    {
        targetAnimator.SetBool(normalParam, true);
    }

    private void ResetBools()
    {
        targetAnimator.SetBool(normalParam, false);
        targetAnimator.SetBool(hoverParam, false);
        targetAnimator.SetBool(pressedParam, false);
        targetAnimator.SetBool(selectedParam, false);
        targetAnimator.SetBool(disabledParam, false);
    }

    // Set Hover Animation when MousePointer enters Button
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOn = true;

        ResetBools();
        targetAnimator.SetBool(hoverParam, true);
    }

    // Set Hover Animation when MousePointer exits Button
    public void OnPointerExit(PointerEventData eventData) 
    {
        isPointerOn = false;

        ResetBools();
        targetAnimator.SetBool(normalParam, true);
    }

    // Set Pressed Animation when MousePointer presses Button
    public void OnPointerDown(PointerEventData eventData)
    {   
        ResetBools();
        targetAnimator.SetBool(pressedParam, true);
    }

    // return to highlighted Animation when MousePointer releases Button
    public void OnPointerUp(PointerEventData eventData)
    {
        ResetBools();

        // Check if MousePointer is still on button before releasing
        if(isPointerOn)
        {   
            // Back to hover Animation if true
            targetAnimator.SetBool(hoverParam, true);            
        }
        else
        {
            // cancel Animation set to normal Animation
            targetAnimator.SetBool(normalParam, true);  
        }
    }

    /*// Set Selected Animation after MousePointer release
    public void OnSelect(BaseEventData eventData)
    {
        ResetBools();
        targetAnimator.SetBool(selectedParam, true);
    }
    // Set normal Animation when Button is nolonger Selected
    public void OnDeselect(BaseEventData eventData)
    {
        ResetBools();
        targetAnimator.SetBool(normalParam, true);
    }*/
}