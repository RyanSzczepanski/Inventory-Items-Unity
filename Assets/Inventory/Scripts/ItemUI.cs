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
    public ItemBasic Item { get; private set; }
    private GameObject draggingObject;
    private static GameObject ITEM_UI_PREFAB;

    public static void Init(GameObject itemUI)
    {
        ITEM_UI_PREFAB = itemUI;
    }

    private void OnEnable()
    {
        Item.OnItemRemoved += DestroyItemUI;
    }
    private void OnDisable()
    {
        Item.OnItemRemoved -= DestroyItemUI;
    }

    public static GameObject GenerateUIItem(ItemBasic item, Transform parent, int originSlotIndex)
    {
        GameObject itemUIObject = Instantiate(ITEM_UI_PREFAB, parent);
        itemUIObject.name = item.data.fullName;
        //ITEM UI
        ItemUI itemUI = itemUIObject.GetComponent<ItemUI>();
        itemUI.Item = item;
        itemUI.enabled = true;
        //Icon
        Image icon = itemUIObject.transform.GetChild(1).GetComponent<Image>();
        icon.sprite = item.data.texture;
        //RectTransform
        RectTransform rect = itemUIObject.GetComponent<RectTransform>();
        SubInventory subInventory = item.SubInventory;
        Vector2 originSlot = new Vector2(originSlotIndex - Mathf.Floor((float)originSlotIndex / (float)subInventory.Width) * subInventory.Width, Mathf.Floor((float)originSlotIndex / subInventory.Width));
        rect.anchoredPosition = new Vector2(originSlot.x * Slot.SlotWidth + (Slot.SlotWidth / 2) * item.data.size.x, originSlot.y * -Slot.SlotWidth - (Slot.SlotWidth / 2) * item.data.size.y);
        rect.sizeDelta = Slot.SlotWidth * item.data.size;
        return itemUIObject;
    }

    public void DestroyItemUI()
    {
        Destroy(this.gameObject);
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
                Item.OpenContextMenu();
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggingObject.transform.position = eventData.position - new Vector2(this.transform.GetComponent<RectTransform>().rect.width - Slot.SlotWidth * Item.data.size.x, -this.transform.GetComponent<RectTransform>().rect.height + Slot.SlotWidth * Item.data.size.y) / (2 / GetComponentInParent<Canvas>().scaleFactor);

        SubInventoryUI targetSubInventory = GetSubInventoryUnderMouse(eventData);
        if(targetSubInventory is not null)
        {
            //Magic Math To Align Item With Inventory
            Vector2 localPosition = eventData.position / GetComponentInParent<Canvas>().scaleFactor - (Vector2)targetSubInventory.transform.position / GetComponentInParent<Canvas>().scaleFactor;
            Vector2 localHoverItemPositon = localPosition - new Vector2((Item.data.size.x - 1) * Slot.SlotWidth / 2, (Item.data.size.y - 1) * -Slot.SlotWidth / 2);
            Vector2Int slot = new Vector2Int((int)Mathf.Clamp(Mathf.Floor(localHoverItemPositon.x / Slot.SlotWidth), 0, targetSubInventory.SubInventory.Width - Item.data.size.x), (int)Mathf.Clamp(Mathf.Floor((-localHoverItemPositon.y) / Slot.SlotWidth), 0, targetSubInventory.SubInventory.Height - Item.data.size.y));
            Vector2 slotPosition = new Vector2(slot.x * Slot.SlotWidth + Slot.SlotWidth / 2 * Item.data.size.x, slot.y * -Slot.SlotWidth - Slot.SlotWidth / 2 * Item.data.size.y);
            
            draggingObject.transform.position = (Vector2)targetSubInventory.transform.position + slotPosition * GetComponentInParent<Canvas>().scaleFactor;
        }

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingObject = ItemUI.GenerateUIItem(Item, null, 0);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(draggingObject);
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
            targetSubInventory.SubInventory.TryMoveItem(Item, Item.SubInventory, slot.x, slot.y);
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
