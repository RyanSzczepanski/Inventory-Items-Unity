using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class UseableItem : ItemBasic, IUseable
{
    public bool CanUse => throw new System.NotImplementedException();

    public UseableItem(UseableItemSO itemSO) : base(itemSO)
    {

    }


    public bool TryUse()
    {
        throw new System.NotImplementedException();
    }
}
