using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubInventory
{
    public int width { get; private set; }
    public int height { get; private set; }

    public Slot[] inventory;

    public SubInventory(int x, int y)
    {
        width = x;
        height = y;
        inventory = new Slot[x * y];
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new Slot();
        }
    }

    public bool CanAddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (!BoundsCheck(item, i)) { continue; }
            if (!OccupiedSlotsCheck(item, i)) { continue; }
            return true;
        }
        return false;
    }
    public bool CanAddItem(Item item, int i)
    {
        return BoundsCheck(item, i)
            && OccupiedSlotsCheck(item, i);
    }
    public bool CanAddItem(Item item, int x, int y)
    {
        return BoundsCheck(item, x, y)
            && OccupiedSlotsCheck(item, x, y);
    }

    public bool TryAddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            int[] realitiveItemSlotIndexOffsets = GetItemsRelativeSlotIndexes(item);
            if (!CanAddItem(item, i)) { continue; }
            foreach (int realitiveItemSlotIndexOffset in realitiveItemSlotIndexOffsets)
            {
                inventory[i + realitiveItemSlotIndexOffset].item = item;
            }
            return true;
        }
        return false;
    }
    public bool TryAddItem(Item item, int i)
    {
        int[] realitiveItemSlotIndexOffsets = GetItemsRelativeSlotIndexes(item);
        if (!CanAddItem(item, i)) { return false; }
        foreach (int realitiveItemSlotIndexOffset in realitiveItemSlotIndexOffsets)
        {
            inventory[i + realitiveItemSlotIndexOffset].item = item;
        }
        return true;
    }
    public bool TryAddItem(Item item, int x, int y)
    {
        Vector2Int[] realitiveItemSlotOffsets = GetItemsRelativeSlots(item);
        if (!CanAddItem(item, x, y)) { return false; }
        foreach (Vector2Int realitiveItemSlotOffset in realitiveItemSlotOffsets)
        {
            inventory[x + width * y + realitiveItemSlotOffset.x + width * realitiveItemSlotOffset.y].item = item;
        }
        return true;
    }

    private int[] GetItemsRelativeSlotIndexes(Item item)
    {
        int[] realitiveItemSlotIndexOffsets = new int[item.itemData.size.x * item.itemData.size.y];

        for (int height = 0; height < item.itemData.size.y; height++)
        {
            for (int width = 0; width < item.itemData.size.x; width++)
            {
                realitiveItemSlotIndexOffsets[width + height * item.itemData.size.x] = width + (height * this.width);
            }
        }
        return realitiveItemSlotIndexOffsets;
    }
    private Vector2Int[] GetItemsRelativeSlots(Item item)
    {
        Vector2Int[] realitiveItemSlotOffsets = new Vector2Int[item.itemData.size.x * item.itemData.size.y];

        for (int height = 0; height < item.itemData.size.y; height++)
        {
            for (int width = 0; width < item.itemData.size.x; width++)
            {
                realitiveItemSlotOffsets[width + height * item.itemData.size.x] = new Vector2Int (width, height);
            }
        }
        return realitiveItemSlotOffsets;
    }

    public int GetItemOriginSlot(Item item)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].item == item) { return i; }
        }
        return -1;
    }

    public int[] GetItemsSlots(Item item, int i)
    {
        int[] itemSlotIndexes = new int[item.itemData.size.x * item.itemData.size.y];

        for (int height = 0; height < item.itemData.size.y; height++)
        {
            for (int width = 0; width < item.itemData.size.x; width++)
            {
                itemSlotIndexes[width + height * item.itemData.size.x] = i + (width + (height * this.width));
            }
        }
        return itemSlotIndexes;
    }

    public bool BoundsCheck(Item item, int slotIndex)
    {
        int[] realitiveItemSlotIndexOffsets = GetItemsRelativeSlotIndexes(item);
        int row = (int)Mathf.Floor((float)slotIndex / width);
        
        for (int i = 0; i < item.itemData.size.x; i++)
        {
            if (Mathf.Floor((float)(slotIndex + realitiveItemSlotIndexOffsets[i]) / width) != row)
            {
                return false;
            }
        }
        return true;
    }
    public bool BoundsCheck(Item item, int x, int y)
    {
        Vector2Int[] realitiveItemSlotOffsets = GetItemsRelativeSlots(item);
        
        foreach (Vector2Int realitiveItemSlotOffset in realitiveItemSlotOffsets)
        {
            Vector2Int itemSlot = realitiveItemSlotOffset + new Vector2Int(x, y);
            if (itemSlot.x < 0 || itemSlot.x >= width || itemSlot.y < 0 || itemSlot.y >= height) { return false; }
        }
        return true;
    }

    public bool OccupiedSlotsCheck(Item item, int slotIndex)
    {
        int[] realitiveItemSlotIndexOffsets = GetItemsRelativeSlotIndexes(item);

        foreach (int realitiveItemSlotIndexOffset in realitiveItemSlotIndexOffsets)
        {
            if (slotIndex + realitiveItemSlotIndexOffset >= inventory.Length)
            {
                continue;
            }
            if (inventory[slotIndex + realitiveItemSlotIndexOffset].item != null)
            {
                return false;
            }
        }
        return true;
    }
    public bool OccupiedSlotsCheck(Item item, int x, int y)
    {
        Vector2Int[] realitiveItemSlotOffsets = GetItemsRelativeSlots(item);

        foreach (Vector2Int realitiveItemSlotOffset in realitiveItemSlotOffsets)
        {
            if (realitiveItemSlotOffset.x < 0 || realitiveItemSlotOffset.x >= width || realitiveItemSlotOffset.y < 0 || realitiveItemSlotOffset.y >= height)
            {
                continue;
            }
            if (inventory[(x + width * y) + (realitiveItemSlotOffset.x + width * realitiveItemSlotOffset.y)].item != null)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveItem(Item item)
    {
        foreach (Slot slot in inventory)
        {
            if (slot.item is null) { continue; }
            if (item == slot.item)
            {
                slot.item = null;
            }
        }
    }

    public bool TryMoveItem(Item targetItem, SubInventory originSubInventory)
    {
        int returnSlotIndex = originSubInventory.GetItemOriginSlot(targetItem);
        originSubInventory.RemoveItem(targetItem);
        if (TryAddItem(targetItem))
        {
            return true;
        }
        else
        {
            originSubInventory.TryAddItem(targetItem);
            return false;
        }
    }
    public bool TryMoveItem(Item targetItem, SubInventory originSubInventory, int i)
    {
        if (CanAddItem(targetItem, i))
        {
            originSubInventory.RemoveItem(targetItem);
            TryAddItem(targetItem, i);
            return true;
        }
        return false;
    }
    public bool TryMoveItem(Item targetItem, SubInventory originSubInventory, int x, int y)
    {
        int returnSlotIndex = originSubInventory.GetItemOriginSlot(targetItem);
        originSubInventory.RemoveItem(targetItem);
        if (TryAddItem(targetItem, x, y))
        {
            return true;
        }
        else
        {
            originSubInventory.TryAddItem(targetItem, returnSlotIndex);
            return false;
        }
    }
}
