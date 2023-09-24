using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public ItemBasic containerAsBasic;
    public ItemContainer container;
    public GameObject prefab;
    //public Item item;

    void Start()
    {
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
        container.OpenContextMenu();
    }
}
