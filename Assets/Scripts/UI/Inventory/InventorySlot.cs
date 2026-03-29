using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryItem currentItem;
    public bool isFull => currentItem != null; // read-only property

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        InventorySlot sourceSlot = draggedItem.currentSlot;

        if (!isFull)
        {
            // Slot empty → normal placement
            AssignItem(draggedItem);
        }
        else
        {
            // Slot full → swap via method
            HandleFullSlot(draggedItem);
        }
    }

    private void HandleFullSlot(InventoryItem newItem)
    {
        InventoryItem oldItem = currentItem;
        InventorySlot sourceSlot = newItem.currentSlot;

        // Place new item in this slot
        AssignItem(newItem);

        // Return old item to original slot
        if (oldItem != null && sourceSlot != null)
        {
            sourceSlot.AssignItem(oldItem);
        }
    }

    public void AssignItem(InventoryItem item)
    {
        // Clear previous slot safely
        if (item.currentSlot != null)
        {
            item.currentSlot.currentItem = null;
        }

        currentItem = item;
        item.currentSlot = this;

        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
    }
}