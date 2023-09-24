using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Assets/UsableItem")]
public class UseableItemSO : ItemBasicSO, IUseableData
{
    public bool CanUse => throw new System.NotImplementedException();

    public override ItemBasic CreateItem()
    {
        return new ItemUseable(this);
    }
}
