using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemUseable : ItemBasic, IUseable
{
    public new readonly UseableItemSO data;
    public bool CanUse => throw new System.NotImplementedException();

    public ItemUseable(UseableItemSO itemSO) : base(itemSO)
    {
        data = itemSO;
    }

    public bool TryUse()
    {
        throw new System.NotImplementedException();
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
            new DropDownMenuOptions { optionText = "Use", action = delegate() { TryUse(); } },
        };
        DropDownMenu.CreateMenu(options, settings);
    }
}
