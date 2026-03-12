using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PopupsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button targetButton;
    [SerializeField] private RectTransform uiPanel;

    [Header("Animation Settings")]
    [SerializeField] private float animationSpeed = 25f;
    [SerializeField] private Vector3 startScale = new Vector3(0, 0, 0);

    //[SerializeField] private bool ShowOnHighlight = false;

    private Vector3 fullScale = Vector3.one;
    private Coroutine currentRoutine;

    private void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.localScale = startScale;
            uiPanel.gameObject.SetActive(false);
        }

        if (targetButton != null)
            targetButton.onClick.AddListener(TriggerShow);
        }

    private void Update()
    {
        // Close UI if the button loses focus (selected mode ends)
        if (uiPanel.gameObject.activeSelf)
        {
            if (EventSystem.current.currentSelectedGameObject != targetButton.gameObject)
            {
            TriggerHide();
            }
        }
    }

    private void TriggerShow()
    {
        // 1. Stop any current shrinking/growing
        if (currentRoutine != null) StopCoroutine(currentRoutine);

        // 2. Reset scale to 'start' so it pops out from the beginning again
        uiPanel.localScale = startScale;
        uiPanel.gameObject.SetActive(true);

        // 3. Start the growth animation
        currentRoutine = StartCoroutine(ScaleUI(fullScale, false));
    }

    private void TriggerHide()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ScaleUI(startScale, true));
    }

    private IEnumerator ScaleUI(Vector3 target, bool disableAfter)
    {
        // Using a while loop with a small threshold for precision
        while (Vector3.Distance(uiPanel.localScale, target) > 0.005f)
        {
            uiPanel.localScale = Vector3.Lerp(uiPanel.localScale, target, Time.deltaTime * animationSpeed);
            yield return null;
        }

        uiPanel.localScale = target;

        if (disableAfter)
        uiPanel.gameObject.SetActive(false);

        currentRoutine = null;
    }
}