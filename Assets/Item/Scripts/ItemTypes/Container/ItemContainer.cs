using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemContainer : ItemBasic, IContainer
{
    public new readonly ItemContainerSO data;
    public Inventory inventory;

    public ItemContainer(ItemContainerSO itemSO) : base(itemSO)
    {
        data = itemSO;
        inventory = new Inventory();
        List<SubInventory> subInventories = new List<SubInventory>();
        for (int _row = 0; _row < itemSO.containerInventory.rows.Length; _row++)
        {
            InventoryRows currentRow = itemSO.containerInventory.rows[_row];

            for (int _col = 0; _col < currentRow.columns.Length; _col++)
            {
                InventoryColumns currentColumn = currentRow.columns[_col];

                subInventories.Add(new SubInventory(currentColumn.width, currentColumn.height, inventory));
            } 
        }
        inventory.subInventories = subInventories.ToArray();
    }

    public override void OpenContextMenu()
    {
        GameObject newMenu = FloatingMenu.CreateMenu(GenerateContextView());
        newMenu.GetComponent<FloatingMenu>().title.text = data.name;
        ContainerInventoryMenu containerMenu = newMenu.AddComponent<ContainerInventoryMenu>();
        containerMenu.item = this;
    }

    public void Test()
    {
        Debug.Log("TESTING");
    }

    public override void OpenDropDownMenu()
    {
        DropDownMenuSettings settings = new DropDownMenuSettings
        {
            destroyOnNewLoad = true,
            creationLocation = CreateLocation.Cursor
        };
        DropDownMenuOptions[] options = new DropDownMenuOptions[]
        {
            new DropDownMenuOptions { optionText = "Discard", action = delegate() { uiItem.SubInventory.RemoveItem(this); } },
            new DropDownMenuOptions { optionText = "Open Container", action = delegate() { OpenContextMenu(); } },
        };
        DropDownMenu.CreateMenu(options, settings);
    }

    public /*override*/ GameObject GenerateContextView()
    {
        //ItemContainerSO _itemData = data;
        GameObject content = InventoryGrid.GenerateInventory(data.containerInventory, inventory, null);

        return content;
    }
}
