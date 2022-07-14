using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attachment Item", menuName = "Assets/AttachmentItem")]
public class AttachmentItemSO : ItemSO
{
    public Vector2Int recoil;

    public override Item CreateItem()
    {
        return new AttachmentItem(this);
    }
    public override void AddItemScript(GameObject gameObject)
    {
        //AttachmentItem attachmentItem = gameObject.AddComponent<AttachmentItem>();
        //attachmentItem.itemData = this;
    }
}
