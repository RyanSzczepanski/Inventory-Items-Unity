using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IDragHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    GameObject dragObj;

    GameObject itemImage;
    GameObject backgroundImage;

    public Item item;
    private Inventory inventory;
    private SubInventory subInventory;

    public void BuildUIInventoryItem(Item item, Inventory inventory, SubInventory subInventory, int originSlotIndex)
    {
        this.item = item;
        this.subInventory = subInventory;
        this.inventory = inventory;
        this.backgroundImage = transform.GetChild(0).gameObject;
        this.itemImage = transform.GetChild(1).gameObject;
        int[] occupiedSlotIndexes = subInventory.GetItemsSlots(item, originSlotIndex);
        Slot[] occupiedSlots = new Slot[occupiedSlotIndexes.Length];
        for (int i = 0; i < occupiedSlotIndexes.Length; i++)
        {
            occupiedSlots[i] = subInventory.inventory[occupiedSlotIndexes[0]];
        }
        Vector2 originSlot = new Vector2(occupiedSlotIndexes[0] - Mathf.Floor((float)occupiedSlotIndexes[0] / (float)subInventory.width) * subInventory.width, Mathf.Floor((float)occupiedSlotIndexes[0] / subInventory.width));
        GetComponent<RectTransform>().anchoredPosition = new Vector2(originSlot.x * Slot.SlotWidth + (Slot.SlotWidth / 2) * item.itemData.size.x, originSlot.y * -Slot.SlotWidth - (Slot.SlotWidth / 2) * item.itemData.size.y);
        GetComponent<RectTransform>().sizeDelta = Slot.SlotWidth * item.itemData.size;

        itemImage.GetComponent<Image>().sprite = this.item.itemData.texture;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) { return; }
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                item.OpenContextMenu();
                break;
            case PointerEventData.InputButton.Middle:
                Debug.Log($"{item.itemData.name}\n" +
                    $"{item.itemData.weight} kg\n" +
                    $"{item.itemData.size}");
                break;
            case PointerEventData.InputButton.Right:
                subInventory.RemoveItem(item);
                Destroy(this.gameObject);
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragObj.transform.position = eventData.position - new Vector2(this.transform.GetComponent<RectTransform>().rect.width - Slot.SlotWidth * item.itemData.size.x, -this.transform.GetComponent<RectTransform>().rect.height + Slot.SlotWidth * item.itemData.size.y) / (2 / GetComponentInParent<Canvas>().scaleFactor);

        UISubInventory targetSubInventory = GetSubInventoryUnderMouse(eventData);
        if(targetSubInventory is not null)
        {
            //Magic Math To Align Item With Inventory
            Vector2 localPosition = eventData.position / GetComponentInParent<Canvas>().scaleFactor - (Vector2)targetSubInventory.transform.position / GetComponentInParent<Canvas>().scaleFactor;
            Vector2 localHoverItemPositon = localPosition - new Vector2((item.itemData.size.x - 1) * Slot.SlotWidth / 2, (item.itemData.size.y - 1) * -Slot.SlotWidth / 2);
            Vector2Int slot = new Vector2Int((int)Mathf.Clamp(Mathf.Floor(localHoverItemPositon.x / Slot.SlotWidth), 0, targetSubInventory.subInventory.width - item.itemData.size.x), (int)Mathf.Clamp(Mathf.Floor((-localHoverItemPositon.y) / Slot.SlotWidth), 0, targetSubInventory.subInventory.height - item.itemData.size.y));
            Vector2 slotPosition = new Vector2(slot.x * Slot.SlotWidth + Slot.SlotWidth / 2 * item.itemData.size.x, slot.y * -Slot.SlotWidth - Slot.SlotWidth / 2 * item.itemData.size.y);
            
            dragObj.transform.position = (Vector2)targetSubInventory.transform.position + slotPosition * GetComponentInParent<Canvas>().scaleFactor;
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragObj = Utils.InstantiateToCanvas(this.gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(dragObj);
        UISubInventory targetSubInventory = GetSubInventoryUnderMouse(eventData);
        if (targetSubInventory is not null)
        {
            //Magic Math To Align Item With Inventory
            Vector2 localPosition = eventData.position / GetComponentInParent<Canvas>().scaleFactor - (Vector2)targetSubInventory.transform.position / GetComponentInParent<Canvas>().scaleFactor;
            Vector2 localHoverItemPositon = localPosition - new Vector2((item.itemData.size.x - 1) * Slot.SlotWidth / 2, (item.itemData.size.y - 1) * -Slot.SlotWidth / 2);
            Vector2Int slot = new Vector2Int((int)Mathf.Clamp(Mathf.Floor(localHoverItemPositon.x / Slot.SlotWidth), 0, targetSubInventory.subInventory.width - item.itemData.size.x), (int)Mathf.Clamp(Mathf.Floor((-localHoverItemPositon.y) / Slot.SlotWidth), 0, targetSubInventory.subInventory.height - item.itemData.size.y));
            Vector2 slotPosition = new Vector2(slot.x * Slot.SlotWidth + Slot.SlotWidth / 2 * item.itemData.size.x, slot.y * -Slot.SlotWidth - Slot.SlotWidth / 2 * item.itemData.size.y);

            //This is 100% in the wrong place but it works for now
            //Can put item into a container inside of its self
            if(item.itemData.type == ItemType.Container)
            {
                ContainerItem thisItem = (ContainerItem)item;
                if (targetSubInventory.inventory == thisItem.inventory)
                {
                    Debug.LogWarning("Trying to move item into its self");
                    return;
                }
            }
            

            if (targetSubInventory.subInventory.TryMoveItem(item, subInventory, slot.x, slot.y))
            {
                //TODO: Make Work

                //targetSubInventory.subInventory.parentInventory.parentItem.OpenContextMenu();
                this.subInventory = targetSubInventory.subInventory;
                this.inventory = targetSubInventory.inventory;
                this.transform.SetParent(targetSubInventory.transform.GetChild(1));
                this.transform.position = (Vector2)targetSubInventory.transform.position + slotPosition * GetComponentInParent<Canvas>().scaleFactor;
            }
        }
    }

    public UISubInventory GetSubInventoryUnderMouse(PointerEventData eventData)
    {
        GraphicRaycaster graphicRaycaster = transform.GetComponentInParent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(eventData, results);
        foreach (RaycastResult result in results)
        {
            UISubInventory UISubInventory;

            if (result.gameObject.TryGetComponent(out UISubInventory))
            {
                return UISubInventory;
            }
        }
        return null;
    }
}
