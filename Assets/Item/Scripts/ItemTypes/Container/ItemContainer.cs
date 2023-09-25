using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemContainer : ItemBasic, IContainer
{
    public new readonly ItemContainerSO data;
    public Inventory inventory;
    public Arrangement[] arrangements;

    public Arrangement Arrangement { get; private set; }

    public ItemContainer(ItemContainerSO itemSO) : base(itemSO)
    {
        Arrangement = itemSO.Arrangement;
        inventory = new Inventory(this);
        arrangements = Arrangement.TreeToArray;
        data = itemSO;
        //List<SubInventory> subInventories = new List<SubInventory>();
        //foreach (var item in arrangements)
        //{
        //    if (item.HasSubInventory)
        //    {
        //        subInventories.Add(new SubInventory(item.subInventory.x, item.subInventory.y, inventory));
        //    }
        //}
    }

    public override void OpenContextMenu()
    {
        GameObject newMenu = FloatingMenu.CreateMenu(GenerateContextView());
        newMenu.GetComponent<FloatingMenu>().title.text = data.name;
        ContainerInventoryMenu containerMenu = newMenu.AddComponent<ContainerInventoryMenu>();
        containerMenu.item = this;
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
            new DropDownMenuOptions { optionText = "Inspect", action = delegate() { OpenInspectMenu(); } },
            new DropDownMenuOptions { optionText = "Discard", action = delegate() { uiItem.SubInventory.RemoveItem(this); } },
            new DropDownMenuOptions { optionText = "Open Container", action = delegate() { OpenContextMenu(); } },
        };
        DropDownMenu.CreateMenu(options, settings);
    }

    public /*override*/ GameObject GenerateContextView()
    {
        //ItemContainerSO _itemData = data;
        GameObject content = InventoryGrid.GenerateInventory(data, inventory, null);

        return content;
    }
}
