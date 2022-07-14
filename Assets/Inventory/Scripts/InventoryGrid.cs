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
    public static GameObject item;

    public static void Init(GameObject _slot, GameObject _item, GameObject _row, GameObject _inv, GameObject _miniInv)
    {
        slot = _slot;
        item = _item;
        inv = _inv;
        row = _row;
        miniInv = _miniInv;
    }

    public static GameObject GenerateInventory(ContainerInventory containerInventory, Inventory inventory, Transform parent)
    {
        GameObject inventoryObject = GameObject.Instantiate(inv, parent);
        inventoryObject.name = "Inventory";
        for (int _row = 0; _row < containerInventory.rows.Length; _row++)
        {
            InventoryRows currentRow = containerInventory.rows[_row];
            GameObject rowObj = GameObject.Instantiate(row, inventoryObject.transform);
            rowObj.name = "Row";

            for (int _col = 0; _col < currentRow.columns.Length; _col++)
            {
                //Loop
                InventoryColumns currentColumn = currentRow.columns[_col];
                SubInventory currentSubInventory = inventory.subInventories[_row * _col];

                //UI
                GameObject subInventoryObj = GameObject.Instantiate(miniInv, rowObj.transform);
                subInventoryObj.name = "Sub Inventory";

                GameObject slotsContainer = subInventoryObj.transform.GetChild(0).gameObject;
                GameObject itemContainer = subInventoryObj.transform.GetChild(1).gameObject;

                slotsContainer.GetComponent<GridLayoutGroup>().constraintCount = currentColumn.width;

                //Component
                UISubInventory UISubInventory = subInventoryObj.AddComponent<UISubInventory>();

                UISubInventory.inventory = inventory;
                UISubInventory.subInventory = currentSubInventory;

                Dictionary<int,Item> items = new Dictionary<int, Item>();
                for (int i = 0; i < currentColumn.width * currentColumn.height; i++)
                {
                    //Loop
                    Slot currentSlot = currentSubInventory.inventory[i];

                    //UI
                    GameObject slotObject = GameObject.Instantiate(slot, slotsContainer.transform);
                    slotObject.name = "Slot";
                    slotObject.transform.parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Slot.SlotWidth, Slot.SlotWidth);

                    //Component
                    UISlot UISlot = slotObject.AddComponent<UISlot>();
                    UISlot.Init(inventory, currentSubInventory, currentSlot);

                    //Add Items
                    if (currentSlot.item is null) { continue; }
                    if (items.ContainsValue(currentSlot.item))
                    {
                       //Duplicate not neccesarily bad
                       //Code gets here for multi-slot items
                        continue;
                    }
                    items.Add(i, currentSlot.item);
                }

                //Draw Items
                foreach (KeyValuePair<int, Item> item in items)
                {
                    GameObject uiInventoryItemObject = GameObject.Instantiate(InventoryGrid.item, itemContainer.transform);
                    
                    UIInventoryItem newUIInventoryItem;
                    
                    if (!uiInventoryItemObject.TryGetComponent(out newUIInventoryItem))
                    {
                        newUIInventoryItem = uiInventoryItemObject.AddComponent<UIInventoryItem>();
                    }
                    newUIInventoryItem.BuildUIInventoryItem(item.Value, inventory, inventory.subInventories[_row * _col], item.Key);
                }
            }
        }
        return inventoryObject;
    }
}

[System.Serializable]
public class ContainerInventory
{
    public InventoryRows[] rows;
}

[System.Serializable]
public class InventoryRows
{
    public InventoryColumns[] columns;
}

[System.Serializable]
public class InventoryColumns
{
    public int width;
    public int height;
}

public enum InventoryGridType
{
    maxCellWidth,
    explicitRows,
}

