using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public InventorySlot currentSlot;

    private Vector2 originalPosition;
    private InventorySlot lastNearestSlot;
    private InventorySlot originalSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // Assign to the slot it starts on
        InventorySlot slot = FindNearestSlot();
        if (slot != null && slot.IsEmpty())
        {
            SnapToSlot(slot);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;

        // Bring on top
        transform.SetAsLastSibling();

        // Allow raycasts to pass through
        canvasGroup.blocksRaycasts = false;

        // Store original slot BEFORE clearing it
        originalSlot = currentSlot;

        if (currentSlot != null)
        {
            currentSlot.Clear();
        }

        lastNearestSlot = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        InventorySlot nearestSlot = FindNearestSlot();
        if (nearestSlot != lastNearestSlot)
        {
            if (lastNearestSlot != null)
                lastNearestSlot.SetHighlight(false);

            if (nearestSlot != null)
                nearestSlot.SetHighlight(true);

            lastNearestSlot = nearestSlot;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (lastNearestSlot != null)
            lastNearestSlot.SetHighlight(false);

        InventorySlot nearestSlot = FindNearestSlot();

        if (nearestSlot == null)
        {
            // Return to original position
            rectTransform.anchoredPosition = originalPosition;
            if (originalSlot != null)
            {
                currentSlot = originalSlot;
                originalSlot.SetItem(this);
            }
            return;
        }

        if (nearestSlot.IsEmpty())
        {
            SnapToSlot(nearestSlot);
        }
        else
        {
            Swap(nearestSlot);
        }
    }

    InventorySlot FindNearestSlot()
    {
        InventorySlot[] slots = FindObjectsOfType<InventorySlot>();
        float minDistance = float.MaxValue;
        InventorySlot nearest = null;

        foreach (var slot in slots)
        {
            float dist = Vector2.Distance(rectTransform.position, slot.rectTransform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = slot;
            }
        }

        return nearest;
    }

    void SnapToSlot(InventorySlot slot)
    {
        rectTransform.position = slot.rectTransform.position;
        currentSlot = slot;
        slot.SetItem(this);
    }

    void Swap(InventorySlot targetSlot)
    {
        InventoryItem otherItem = targetSlot.currentItem;

        if (otherItem == null || originalSlot == null)
        {
            SnapToSlot(targetSlot);
            return;
        }

        // Move other item back to original slot
        otherItem.rectTransform.position = originalSlot.rectTransform.position;
        otherItem.currentSlot = originalSlot;
        originalSlot.SetItem(otherItem);

        // Move this item to target slot
        rectTransform.position = targetSlot.rectTransform.position;
        currentSlot = targetSlot;
        targetSlot.SetItem(this);
    }
}