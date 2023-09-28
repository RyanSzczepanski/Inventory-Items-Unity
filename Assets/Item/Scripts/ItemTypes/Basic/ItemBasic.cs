using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ItemBasic
{
    public readonly ItemBasicSO data;

    public delegate void ItemAddedHandler();
    public ItemAddedHandler OnItemAdded;
    public delegate void ItemRemovedHandler();
    public ItemAddedHandler OnItemRemoved;

    //TODO: Value Never Set
    public SubInventory SubInventory { get; private set; }

    public ItemBasic(ItemBasicSO itemSO)
    {
        data = itemSO;

    }

    public void AddToSubInventory(SubInventory subInventory)
    {
        SubInventory = subInventory;
    }

    public virtual ContextMenuOption[] GetContextMenuOptions()
    {
        return new ContextMenuOption[]
        {
            new ContextMenuOption()
            {
                optionText = "Inspect",
                action = OpenInspectMenu

            },
            new ContextMenuOption()
            {
                optionText = "Discard",
                action = delegate { SubInventory.RemoveItem(this); }
            }
        };
    }

    public virtual void OpenInspectMenu()
    {
        throw new NotImplementedException();
    }

    public virtual void OpenContextMenu()
    {
        ContextMenuSettings settings = new ContextMenuSettings
        {
            destroyOnNewLoad = true,
            creationLocation = CreateLocation.Cursor
        };
        ContextMenu.CreateMenu(GetContextMenuOptions(), settings);
    }
}
