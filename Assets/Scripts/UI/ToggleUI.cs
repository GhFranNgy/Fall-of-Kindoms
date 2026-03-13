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

    [Header("Behaviour")]
    public bool disableAfter = false;

    private Button triggerButton;
    private Vector3 fullScale = Vector3.one;
    private Coroutine currentRoutine;
    private bool isOpen = false;

    void Start()
    {
        triggerButton = GetComponent<Button>();

        if (object_to_toggle != null)
        {
            object_to_toggle.transform.localScale = startScale;
            object_to_toggle.SetActive(false);
        }
    }

    void Update()
    {
        if (!disableAfter) return;
        if (!isOpen) return;

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
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        object_to_toggle.transform.localScale = startScale;
        object_to_toggle.SetActive(true);

        isOpen = true;
        currentRoutine = StartCoroutine(ScaleUI(fullScale, false));
    }

    public void SetUIDisabled()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        isOpen = false;
        currentRoutine = StartCoroutine(ScaleUI(startScale, true));
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