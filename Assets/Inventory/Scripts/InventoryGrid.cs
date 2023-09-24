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
                GameObject subInventoryObj = SubInventoryUI.Generate(miniInv, rowObj.transform, currentSubInventory);



                Dictionary<int, ItemBasic> items = new Dictionary<int, ItemBasic>();
                for (int i = 0; i < currentColumn.width * currentColumn.height; i++)
                {
                    //Loop
                    Slot currentSlot = currentSubInventory.slots[i];

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
                foreach (KeyValuePair<int, ItemBasic> item in items)
                {
                    GameObject newUIItemObject = ItemUI.GenerateUIItem(item.Value, inventory.subInventories[_row * _col], item.Key);
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

