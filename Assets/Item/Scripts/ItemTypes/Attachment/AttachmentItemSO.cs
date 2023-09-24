using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attachment Item", menuName = "Assets/AttachmentItem")]
public class AttachmentItemSO : ItemBasicSO, IAttachment
{
    public Vector2Int recoil;

    public override ItemBasic CreateItem()
    {
        return new ItemAttachment(this);
    }
}
