using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubInventory
{
    public delegate void AddItemHandler(System.Object sender, SubInventoryEventArgs e);
    public AddItemHandler OnAddItem;
    public delegate void RemoveItemHandler(System.Object sender, SubInventoryEventArgs e);
    public RemoveItemHandler OnRemoveItem;

    public int Width { get; private set; }
    public int Height { get; private set; }

    [HideInInspector] public Inventory Inventory { get; private set; }
    [HideInInspector] public SubInventoryUI UISubInventory { get; set; }

    public Slot[] slots;

    public SubInventory(int x, int y, Inventory inventory)
    {
        Width = x;
        Height = y;
        slots = new Slot[x * y];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new Slot();
        }
        Inventory = inventory;
    }

    public void AddItem(ItemBasic item, int i)
    {
        int[] realitiveItemSlotIndexOffsets = GetItemsRelativeSlotIndexes(item);
        foreach (int realitiveItemSlotIndexOffset in realitiveItemSlotIndexOffsets)
        {
            slots[i + realitiveItemSlotIndexOffset].item = item;
        }
        SubInventoryEventArgs eventArgs = new SubInventoryEventArgs { item = item, subInventory = this, originIndex = i };
        //item.OnItemAdded?.Invoke(this, eventArgs);
        OnAddItem?.Invoke(this, eventArgs);
    }
    public void AddItem(ItemBasic item, int x, int y)
    {
        Vector2Int[] realitiveItemSlotOffsets = GetItemsRelativeSlots(item);
        foreach (Vector2Int realitiveItemSlotOffset in realitiveItemSlotOffsets)
        {
            slots[x + Width * y + realitiveItemSlotOffset.x + Width * realitiveItemSlotOffset.y].item = item;
        }
        int originIndex = x + y * Width;
        SubInventoryEventArgs eventArgs = new SubInventoryEventArgs { item = item, subInventory = this, originIndex = originIndex };
        OnAddItem?.Invoke(this, eventArgs);
    }

    public bool CanAddItem(ItemBasic item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!BoundsCheck(item, i)) { continue; }
            if (!OccupiedSlotsCheck(item, i)) { continue; }
            return true;
        }
        return false;
    }
    public bool CanAddItem(ItemBasic item, int i)
    {
        return BoundsCheck(item, i)
            && OccupiedSlotsCheck(item, i);
    }
    public bool CanAddItem(ItemBasic item, int x, int y)
    {
        return BoundsCheck(item, x, y)
            && OccupiedSlotsCheck(item, x, y);
    }

    public bool TryAddItem(ItemBasic item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!CanAddItem(item, i)) { continue; }
            AddItem(item, i);
            return true;
        }
        return false;
    }
    public bool TryAddItem(ItemBasic item, int i)
    {
        if (!CanAddItem(item, i)) { return false; }
        AddItem(item, i);
        return true;
    }
    public bool TryAddItem(ItemBasic item, int x, int y)
    {
        if (!CanAddItem(item, x, y)) { return false; }
        AddItem(item, x, y);
        return true;
    }

    private int[] GetItemsRelativeSlotIndexes(ItemBasic item)
    {
        int[] realitiveItemSlotIndexOffsets = new int[item.data.size.x * item.data.size.y];

        for (int height = 0; height < item.data.size.y; height++)
        {
            for (int width = 0; width < item.data.size.x; width++)
            {
                realitiveItemSlotIndexOffsets[width + height * item.data.size.x] = width + (height * this.Width);
            }
        }
        return realitiveItemSlotIndexOffsets;
    }
    private Vector2Int[] GetItemsRelativeSlots(ItemBasic item)
    {
        Vector2Int[] realitiveItemSlotOffsets = new Vector2Int[item.data.size.x * item.data.size.y];

        for (int height = 0; height < item.data.size.y; height++)
        {
            for (int width = 0; width < item.data.size.x; width++)
            {
                realitiveItemSlotOffsets[width + height * item.data.size.x] = new Vector2Int (width, height);
            }
        }
        return realitiveItemSlotOffsets;
    }

    public int GetItemOriginSlot(ItemBasic item)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item) { return i; }
        }
        return -1;
    }

    public int[] GetItemSlots(ItemBasic item, int i)
    {
        int[] itemSlotIndexes = new int[item.data.size.x * item.data.size.y];

        for (int height = 0; height < item.data.size.y; height++)
        {
            for (int width = 0; width < item.data.size.x; width++)
            {
                itemSlotIndexes[width + height * item.data.size.x] = i + (width + (height * this.Width));
            }
        }
        return itemSlotIndexes;
    }

    public bool BoundsCheck(ItemBasic item, int slotIndex)
    {
        int[] realitiveItemSlotIndexOffsets = GetItemsRelativeSlotIndexes(item);
        int row = (int)Mathf.Floor((float)slotIndex / Width);
        
        for (int i = 0; i < item.data.size.x; i++)
        {
            if (Mathf.Floor((float)(slotIndex + realitiveItemSlotIndexOffsets[i]) / Width) != row)
            {
                return false;
            }
        }
        return true;
    }
    public bool BoundsCheck(ItemBasic item, int x, int y)
    {
        Vector2Int[] realitiveItemSlotOffsets = GetItemsRelativeSlots(item);
        
        foreach (Vector2Int realitiveItemSlotOffset in realitiveItemSlotOffsets)
        {
            Vector2Int itemSlot = realitiveItemSlotOffset + new Vector2Int(x, y);
            if (itemSlot.x < 0 || itemSlot.x >= Width || itemSlot.y < 0 || itemSlot.y >= Height) { return false; }
        }
        return true;
    }

    public bool OccupiedSlotsCheck(ItemBasic item, int slotIndex)
    {
        int[] realitiveItemSlotIndexOffsets = GetItemsRelativeSlotIndexes(item);

        foreach (int realitiveItemSlotIndexOffset in realitiveItemSlotIndexOffsets)
        {
            if (slotIndex + realitiveItemSlotIndexOffset >= slots.Length)
            {
                continue;
            }
            if (slots[slotIndex + realitiveItemSlotIndexOffset].item != null)
            {
                return false;
            }
        }
        return true;
    }
    public bool OccupiedSlotsCheck(ItemBasic item, int x, int y)
    {
        Vector2Int[] realitiveItemSlotOffsets = GetItemsRelativeSlots(item);

        foreach (Vector2Int realitiveItemSlotOffset in realitiveItemSlotOffsets)
        {
            if (realitiveItemSlotOffset.x < 0 || realitiveItemSlotOffset.x >= Width || realitiveItemSlotOffset.y < 0 || realitiveItemSlotOffset.y >= Height)
            {
                continue;
            }
            if (slots[(x + Width * y) + (realitiveItemSlotOffset.x + Width * realitiveItemSlotOffset.y)].item != null)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveItem(ItemBasic item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.item is null) { continue; }
            if (item == slot.item)
            {
                slot.item = null;
            }
        }
        //This shouldnt be handled here
        GameObject.Destroy(item.uiItem.gameObject);
        SubInventoryEventArgs eventArgs = new SubInventoryEventArgs { item = item, subInventory = this, originIndex = GetItemOriginSlot(item)};
        OnRemoveItem?.Invoke(this, eventArgs);
    }

    public bool TryMoveItem(ItemBasic targetItem, SubInventory originSubInventory)
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
    public bool TryMoveItem(ItemBasic targetItem, SubInventory originSubInventory, int i)
    {
        if (CanAddItem(targetItem, i))
        {
            originSubInventory.RemoveItem(targetItem);
            TryAddItem(targetItem, i);
            return true;
        }
        return false;
    }
    public bool TryMoveItem(ItemBasic targetItem, SubInventory originSubInventory, int x, int y)
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

public class SubInventoryEventArgs: EventArgs
{
    public ItemBasic item;
    public SubInventory subInventory;
    public int originIndex;
}
