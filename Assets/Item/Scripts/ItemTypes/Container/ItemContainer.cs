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
    }

    public void OpenFloatingWindowInventory()
    {
        GameObject newMenu = FloatingMenu.CreateMenu(GenerateInventoryContent());
        newMenu.GetComponent<FloatingMenu>().title.text = data.name;
    }

    public /*override*/ GameObject GenerateInventoryContent()
    {
        GameObject content = InventoryGrid.GenerateInventory(data, inventory, null);
        return content;
    }

    public override ContextMenuOption[] GetContextMenuOptions()
    {
        List<ContextMenuOption> contextMenuOptions = new List<ContextMenuOption>();
        ContextMenuOption[] specificContextMenuOptions = new ContextMenuOption[]
        {
            new ContextMenuOption()
            {
                optionText = "Open Container",
                action = delegate { OpenFloatingWindowInventory(); }

            },
        };
        contextMenuOptions.AddRange(base.GetContextMenuOptions());
        contextMenuOptions.AddRange(specificContextMenuOptions);
        return contextMenuOptions.ToArray();
    }
}
