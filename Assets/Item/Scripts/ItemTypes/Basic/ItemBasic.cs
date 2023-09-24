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

    public virtual void OpenContextMenu()
    {

    }

    public virtual void OpenDropDownMenu()
    {
        DropDownMenuSettings settings = new DropDownMenuSettings
        {
            destroyOnNewLoad = true,
            creationLocation = CreateLocation.Cursor
        };
        DropDownMenuOptions[] options = new DropDownMenuOptions[]
        {
            new DropDownMenuOptions { optionText = "Discard", action = delegate () { uiItem.SubInventory.RemoveItem(this); } },
        };
        DropDownMenu.CreateMenu(options, settings);
    }
}
