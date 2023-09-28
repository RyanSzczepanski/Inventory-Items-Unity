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

    public override ContextMenuOption[] GetContextMenuOptions()
    {
        List<ContextMenuOption> contextMenuOptions  = new List<ContextMenuOption>();
        ContextMenuOption[] specificContextMenuOptions = new ContextMenuOption[]
        {
            new ContextMenuOption()
            {
                optionText = "Use",
                action = delegate { TryUse(); }

            },
        };
        contextMenuOptions.AddRange(base.GetContextMenuOptions());
        contextMenuOptions.AddRange(specificContextMenuOptions);
        return contextMenuOptions.ToArray();
    }
}
