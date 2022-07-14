using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Item
{
    public ItemSO itemData;

    public event Action OnOpenContextMenu;

    public Item(ItemSO itemSO)
    {
        itemData = itemSO;
    }

    public virtual void OpenContextMenu()
    {
        GameObject newMenu = FloatingMenu.CreateMenu(GenerateContextView());
        newMenu.GetComponent<FloatingMenu>().title.text = itemData.name;
    }

    public virtual GameObject GenerateContextView()
    {
        throw new System.NotImplementedException();
    }
}

//public class UIInventoryItem : MonoBehaviour, IDragHandler, IPointerClickHandler
//{
//    public Item item;

//    public void OnDrag(PointerEventData eventData)
//    {
//        throw new NotImplementedException();
//    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        throw new NotImplementedException();
//    }
//}
