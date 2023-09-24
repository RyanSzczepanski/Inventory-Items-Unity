using FlyingWormConsole3.LiteNetLib;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IDragHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    GameObject temp;


    public ItemBasic Item { get; private set; }
    public SubInventory SubInventory { get; private set; }

    private void OnEnable()
    {
        //Item.OnItemAdded += 
    }

    public static void GenerateUIItem(System.Object sender, SubInventoryEventArgs args)
    {
        GenerateUIItem(args.item, args.subInventory, args.originIndex);
    }
    public static GameObject GenerateUIItem(ItemBasic item, SubInventory subInventory, int originSlotIndex)
    {
        GameObject uiItemObject = new GameObject(item.data.fullName);
        uiItemObject.transform.SetParent(subInventory.UISubInventory.transform.GetChild(1));
        ItemUI uiItem = uiItemObject.AddComponent<ItemUI>();
        uiItem.Item = item;
        uiItem.SubInventory = subInventory;
        item.uiItem = uiItem;
        RectTransform rect = uiItemObject.AddComponent<RectTransform>();
        Image image = uiItemObject.AddComponent<Image>();
        image.sprite = item.data.texture;
        image.preserveAspect = true;
        Vector2 originSlot = new Vector2(originSlotIndex - Mathf.Floor((float)originSlotIndex / (float)subInventory.Width) * subInventory.Width, Mathf.Floor((float)originSlotIndex / subInventory.Width));
        rect.anchoredPosition = new Vector2(originSlot.x * Slot.SlotWidth + (Slot.SlotWidth / 2) * item.data.size.x, originSlot.y * -Slot.SlotWidth - (Slot.SlotWidth / 2) * item.data.size.y);
        rect.sizeDelta = Slot.SlotWidth * item.data.size;
        return uiItemObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) { return; }
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Debug.Log(Item.data.fullName);
                //IUseable durable = item as IUseable;
                break;
            case PointerEventData.InputButton.Middle:
                Debug.Log($"{Item.data.name}\n" +
                    $"{Item.data.weight} kg\n" +
                    $"{Item.data.size}");
                break;
            case PointerEventData.InputButton.Right:
                Item.OpenDropDownMenu();
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        temp.transform.position = eventData.position - new Vector2(this.transform.GetComponent<RectTransform>().rect.width - Slot.SlotWidth * Item.data.size.x, -this.transform.GetComponent<RectTransform>().rect.height + Slot.SlotWidth * Item.data.size.y) / (2 / GetComponentInParent<Canvas>().scaleFactor);

        SubInventoryUI targetSubInventory = GetSubInventoryUnderMouse(eventData);
        if(targetSubInventory is not null)
        {
            //Magic Math To Align Item With Inventory
            Vector2 localPosition = eventData.position / GetComponentInParent<Canvas>().scaleFactor - (Vector2)targetSubInventory.transform.position / GetComponentInParent<Canvas>().scaleFactor;
            Vector2 localHoverItemPositon = localPosition - new Vector2((Item.data.size.x - 1) * Slot.SlotWidth / 2, (Item.data.size.y - 1) * -Slot.SlotWidth / 2);
            Vector2Int slot = new Vector2Int((int)Mathf.Clamp(Mathf.Floor(localHoverItemPositon.x / Slot.SlotWidth), 0, targetSubInventory.SubInventory.Width - Item.data.size.x), (int)Mathf.Clamp(Mathf.Floor((-localHoverItemPositon.y) / Slot.SlotWidth), 0, targetSubInventory.SubInventory.Height - Item.data.size.y));
            Vector2 slotPosition = new Vector2(slot.x * Slot.SlotWidth + Slot.SlotWidth / 2 * Item.data.size.x, slot.y * -Slot.SlotWidth - Slot.SlotWidth / 2 * Item.data.size.y);
            
            temp.transform.position = (Vector2)targetSubInventory.transform.position + slotPosition * GetComponentInParent<Canvas>().scaleFactor;
        }

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        temp = Utils.InstantiateToCanvas(this.gameObject);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(temp);
        SubInventoryUI targetSubInventory = GetSubInventoryUnderMouse(eventData);
        if (targetSubInventory is not null)
        {
            //Magic Math To Align Item With Inventory
            Vector2 localPosition = eventData.position / GetComponentInParent<Canvas>().scaleFactor - (Vector2)targetSubInventory.transform.position / GetComponentInParent<Canvas>().scaleFactor;
            Vector2 localHoverItemPositon = localPosition - new Vector2((Item.data.size.x - 1) * Slot.SlotWidth / 2, (Item.data.size.y - 1) * -Slot.SlotWidth / 2);
            Vector2Int slot = new Vector2Int((int)Mathf.Clamp(Mathf.Floor(localHoverItemPositon.x / Slot.SlotWidth), 0, targetSubInventory.SubInventory.Width - Item.data.size.x), (int)Mathf.Clamp(Mathf.Floor((-localHoverItemPositon.y) / Slot.SlotWidth), 0, targetSubInventory.SubInventory.Height - Item.data.size.y));

            //This is 100% in the wrong place but it works for now
            //Can put item into a container inside of its self
            if (Item.data.type == ItemType.Container)
            {
                ItemContainer thisItem = (ItemContainer)Item;
                if (targetSubInventory.SubInventory.Inventory == thisItem.inventory)
                {
                    Debug.LogWarning("Trying to move item into its self");
                    return;
                }
            }
            targetSubInventory.SubInventory.TryMoveItem(Item, SubInventory, slot.x, slot.y);
        }
    }

    public void MoveTo()
    {

    }

    public SubInventoryUI GetSubInventoryUnderMouse(PointerEventData eventData)
    {
        GraphicRaycaster graphicRaycaster = transform.GetComponentInParent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(eventData, results);
        foreach (RaycastResult result in results)
        {
            SubInventoryUI UISubInventory;

            if (result.gameObject.TryGetComponent(out UISubInventory))
            {
                return UISubInventory;
            }
        }
        return null;
    }
}
