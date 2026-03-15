/*
This Script is to Toggle UI On and Off

animationSpeed is the speed the UI Pops In and out

startScale is the size (scale) the UI pops out

fullScale is the size (scale) the UI has at the end of the Popout Animation

disabeAfter is a setting to set whether it should Popin after the button is not selected anymore 
    or stay until something else closes it

triggerButton is the Button it´s self (don´t touch it) in line 33 it gets the button component

currentRoutine is the Coroutine for the animation

isOpen checks whether the UI is Toggled On or Off
*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ToggleUI : MonoBehaviour
{
    public GameObject object_to_toggle;

    [Header("Animation")]
    public float animationSpeed = 25f;
    public Vector3 startScale = Vector3.zero;
    public Vector3 fullScale = Vector3.one;

    [Header("Behaviour")]
    public bool disableAfter = false;
    public bool showAnimation = true;

    private Button triggerButton;
    private Coroutine currentRoutine;
    private bool isOpen = false;

    // Store the original scale of the object so we don't overwrite it in Start
    private Vector3 originalScale;

    void Start()
    {
        triggerButton = GetComponent<Button>();

        if (object_to_toggle != null)
        {
            // Store the original scale but don't modify it
            originalScale = object_to_toggle.transform.localScale;
        }
    }

    void Update()
    {
        if (!disableAfter || !isOpen) return;

        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null || 
            (selected != triggerButton.gameObject &&
            !selected.transform.IsChildOf(object_to_toggle.transform)))
        {
            SetUIDisabled();
        }
    }

    public void SetUIEnabled()
    {
        if (object_to_toggle == null) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        // Make sure object is active
        object_to_toggle.SetActive(true);

        // Start from startScale if animation is enabled, else fullScale
        if (showAnimation)
        {
            object_to_toggle.transform.localScale = startScale;
            currentRoutine = StartCoroutine(ScaleUI(fullScale, false));
        }
        else
        {
            object_to_toggle.transform.localScale = fullScale;
        }

        isOpen = true;
    }

    public void SetUIDisabled()
    {
        if (object_to_toggle == null) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        isOpen = false;

        if (showAnimation)
            currentRoutine = StartCoroutine(ScaleUI(startScale, true));
        else
            object_to_toggle.transform.localScale = startScale;
    }

    private IEnumerator ScaleUI(Vector3 target, bool disableAfterAnim)
    {
        Transform t = object_to_toggle.transform;

        while (Vector3.Distance(t.localScale, target) > 0.005f)
        {
            t.localScale = Vector3.Lerp(t.localScale, target, Time.deltaTime * animationSpeed);
            yield return null;
        }

        t.localScale = target;

        if (disableAfterAnim)
            object_to_toggle.SetActive(false);

        currentRoutine = null;
    }
}