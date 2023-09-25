using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid
{
    private static GameObject slot;
    private static GameObject row;
    private static GameObject inv;
    private static GameObject miniInv;

    public static void Init(GameObject _slot, GameObject _row, GameObject _inv, GameObject _miniInv)
    {
        slot = _slot;
        inv = _inv;
        row = _row;
        miniInv = _miniInv;
    }

    public static GameObject GenerateInventory(IContainerData containerData, Inventory inventory, Transform parent)
    {
        GameObject inventoryObject = GameObject.Instantiate(inv, parent);
        inventoryObject.name = "Inventory";
        //TODO: make multi sub inventory work
        foreach (var item in containerData.Arrangement.TreeToArray)
        {
            int subInventoryIndex = 0;
            GameObject currentObject = inventoryObject;
            Transform currentParent = inventoryObject.transform;
            if (!item.IsLeaf)
            {
                if (item.arrangementDirection == ArrangementDirection.Horizontal)
                {
                    currentObject = GameObject.Instantiate(row, currentParent);
                    currentObject.name = "Row";
                    currentParent = currentObject.transform;
                }
                else
                {
                    //currentObject = GameObject.Instantiate(column, currentParent);
                    //currentObject.name = "Column";
                    //currentParent = currentObject.transform;
                }
                continue;
            }
            if (item.HasSubInventory)
            {
                SubInventory currentSubInventory = inventory.SubInventories[subInventoryIndex];
                GameObject subInventoryObj = SubInventoryUI.Generate(miniInv, currentParent, currentSubInventory);
                subInventoryIndex++;

                Dictionary<int, ItemBasic> items = new Dictionary<int, ItemBasic>();
                for (int i = 0; i < currentSubInventory.slots.Length; i++)
                {
                    //Loop
                    Slot currentSlot = currentSubInventory.slots[i];

                    //Refactor Removes this from being relevant so it will stay ugly
                    //UI
                    GameObject slotObject = GameObject.Instantiate(slot, subInventoryObj.transform.GetChild(0).transform);
                    slotObject.name = "Slot";
                    slotObject.transform.parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Slot.SlotWidth, Slot.SlotWidth);

                    //Component
                    UISlot UISlot = slotObject.AddComponent<UISlot>();
                    UISlot.Init(inventory, currentSubInventory, currentSlot);

                    //Add Items
                    if (currentSlot.item is null) { continue; }
                    if (items.ContainsValue(currentSlot.item))
                    {
                        continue;
                    }
                    items.Add(i, currentSlot.item);
                }

                //Draw Items
                foreach (KeyValuePair<int, ItemBasic> itemkvp in items)
                {
                    GameObject newUIItemObject = ItemUI.GenerateUIItem(itemkvp.Value, currentSubInventory, itemkvp.Key);
                }
            }
        }
        return inventoryObject;
    }
}

[System.Serializable]
public class ContainerInventory
{
    public InventoryRow[] rows;
}

[System.Serializable]
public class InventoryRow
{
    public InventoryColumn[] columns;
}

[System.Serializable]
public class InventoryColumn
{
    public int width;
    public int height;
}

public enum InventoryGridType
{
    maxCellWidth,
    explicitRows,
}

