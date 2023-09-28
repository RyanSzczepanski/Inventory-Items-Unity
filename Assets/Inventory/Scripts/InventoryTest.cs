using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    
    public ItemContainer container;
    public GameObject prefab;
    //public Item item;

    void Start()
    {
        ItemDB.Init();
        container = ItemDB.GetObjectByName("Debug Container Item").CreateItem() as ItemContainer;
        foreach (ItemBasicSO item in ItemDB.GetValues())
        {
            ItemBasic newItem = item.CreateItem();
            container.inventory.AddItem(newItem);
        }
        OpenDebugContainer();
    }

    public void OpenDebugContainer()
    {
        container.OpenFloatingWindowInventory();
    }
}
