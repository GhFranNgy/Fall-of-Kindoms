using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    // REMOVE WHEN DONE WITH DEBUGGING
    public ItemData Goldmine;
    public ItemData Gold;

    public GameObject hotbarObj;
    public GameObject inventorySlotParent;

    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private List<InventorySlot> hotbarSlots = new List<InventorySlot>();
    private List<InventorySlot> allSlots = new List<InventorySlot>();

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<InventorySlot>());
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<InventorySlot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
    }

    public void Update()
    {

        // REMOVE WHEN DONE WITH DEBUGGING(This is Shop Logic later)
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddItem(Gold, 10000);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddItem(Goldmine, 1);
        }
    }

    public void AddItem(ItemData itemToAdd, int amount)
    {
        int remaining = amount;

        foreach(InventorySlot slot in allSlots)
        {
            if(slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStack;

                if(currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= amountToAdd;

                    if (remaining <= 0) 
                        return;
                }
            }
        }

        foreach(InventorySlot slot in allSlots)
        {
            if(!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStack, remaining);
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;

                if(remaining <= 0) 
                    return;
            }
        }

        if (remaining > 0) 
            Debug.Log("Inventory is Full, Could not add" + remaining + itemToAdd.itemName);
    }
}
