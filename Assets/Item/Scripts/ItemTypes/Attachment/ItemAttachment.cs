using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemAttachment : ItemBasic, IAttachment
{
    //public new ItemAttachmentSO data;
    public ItemAttachment(AttachmentItemSO itemSO) : base(itemSO)
    {
        //data = itemSO;
    }
}
