using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] slots = new InventorySlot[6];
    public int selectedSlot = 0;

    void Start()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i] = new InventorySlot();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseItem(selectedSlot, FindAnyObjectByType<StressManager>());
        }
    }

    public void AddItem(ItemData item, int amount)
    {
        // STACK if same item
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && item.isStackable)
            {
                slots[i].quantity += amount;
                return;
            }
        }

        // FIND EMPTY SLOT
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                slots[i].quantity = amount;
                return;
            }
        }

        Debug.Log("Inventory Full");
        Debug.Log("Adding item: " + item);
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                if (slots[i].quantity >= amount)
                {
                    slots[i].quantity -= amount;

                    if (slots[i].quantity <= 0)
                    {
                        slots[i] = new InventorySlot();
                    }

                    return true;
                }
            }
        }

        return false;
    }

    public void UseItem(int index, StressManager stressManager)
    {
        var slot = slots[index];

        if (slot.item == null) return;

        slot.quantity--;

        if (slot.quantity <= 0)
        {
            slots[index] = new InventorySlot();
        }
    }

    public bool HasItem(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].quantity > 0)
            {
                return true;
            }
        }

        return false;
    }

    public int GetItemCount(ItemData item)
    {
        int count = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                count += slots[i].quantity;
            }
        }

        return count;
    }

    // ------------------------------------------------------------
    // NEW: SAFE SNAPSHOT FOR TASK RESERVATION SYSTEM
    // ------------------------------------------------------------
    public Dictionary<ItemData, int> GetSnapshot()
    {
        Dictionary<ItemData, int> snapshot = new Dictionary<ItemData, int>();

        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];

            if (slot.item == null || slot.quantity <= 0)
                continue;

            if (!snapshot.ContainsKey(slot.item))
                snapshot[slot.item] = 0;

            snapshot[slot.item] += slot.quantity;
        }

        return snapshot;
    }

    public void SelectSlot(int index)
    {
        selectedSlot = index;
    }
}