using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ItemBasic
{
    public readonly ItemBasicSO data;

    public delegate void ItemAddedHandler(System.Object sender, SubInventoryEventArgs args);
    public ItemAddedHandler OnItemAdded;
    public delegate void ItemRemovedHandler(System.Object sender, SubInventoryEventArgs args);
    public ItemAddedHandler OnItemRemoved;

    [HideInInspector] public ItemUI uiItem;

    public SubInventory SubInventory { get; private set; }

    public ItemBasic(ItemBasicSO itemSO)
    {
        data = itemSO;
    }

    public virtual void OpenInspectMenu()
    {
        throw new NotImplementedException();
    }

    public virtual void OpenContextMenu()
    {
        throw new NotImplementedException();

    }

    //public virtual void OpenDropDownMenu()
    //{
    //    DropDownMenuSettings settings = new DropDownMenuSettings
    //    {
    //        destroyOnNewLoad = true,
    //        creationLocation = CreateLocation.Cursor
    //    };
    //    DropDownMenuOptions[] options = new DropDownMenuOptions[]
    //    {
    //        new DropDownMenuOptions { optionText = "Inspect", action = delegate() { OpenInspectMenu(); } },
    //        new DropDownMenuOptions { optionText = "Discard", action = delegate() { uiItem.SubInventory.RemoveItem(this); } },
    //        new DropDownMenuOptions { optionText = "Open Container", action = delegate() { OpenContextMenu(); } },
    //    };
    //    DropDownMenu.CreateMenu(options, settings);
    //}
    public virtual void OpenDropDownMenu()
    {
        DropDownMenuSettings settings = new DropDownMenuSettings
        {
            destroyOnNewLoad = true,
            creationLocation = CreateLocation.Cursor
        };
        DropDownMenuOptions[] options = new DropDownMenuOptions[]
        {
            new DropDownMenuOptions { optionText = "Inspect", action = delegate () { OpenContextMenu(); } },
            new DropDownMenuOptions { optionText = "Discard", action = delegate () { uiItem.SubInventory.RemoveItem(this); } },
        };
        DropDownMenu.CreateMenu(options, settings);
    }
}
