using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContainerItem : Item
{
    public Inventory inventory;
    public ContainerItem(ContainerItemSO itemSO) : base(itemSO)
    {
        List<SubInventory> subInventories = new List<SubInventory>();
        for (int _row = 0; _row < itemSO.containerInventory.rows.Length; _row++)
        {
            InventoryRows currentRow = itemSO.containerInventory.rows[_row];

            for (int _col = 0; _col < currentRow.columns.Length; _col++)
            {
                InventoryColumns currentColumn = currentRow.columns[_col];

                subInventories.Add(new SubInventory(currentColumn.width, currentColumn.height));
            } 
        }
        inventory = new Inventory(subInventories.ToArray());
    }

    public override void OpenContextMenu()
    {
        GameObject newMenu = FloatingMenu.CreateMenu(GenerateContextView());
        newMenu.GetComponent<FloatingMenu>().title.text = itemData.name;
        ContainerInventoryMenu containerMenu = newMenu.AddComponent<ContainerInventoryMenu>();
        containerMenu.item = this;
    }

    public override GameObject GenerateContextView()
    {
        ContainerItemSO _itemData = itemData as ContainerItemSO;
        GameObject content = InventoryGrid.GenerateInventory(_itemData.containerInventory, inventory, null);

        return content;
    }
}
