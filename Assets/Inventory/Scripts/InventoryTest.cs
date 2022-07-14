using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public ContainerItem container;
    public ContainerItem targetContainer;
    public Item targetItem;
    public GameObject prefab;
    //public Item item;

    void Start()
    {
        container = (ContainerItem)ItemDB.GetObjectByName("Debug Container Item").CreateItem();
        foreach (ItemSO item in ItemDB.GetValues())
        {
            Item newItem = item.CreateItem();
            container.inventory.AddItem(newItem);
        }
        OpenDebugContainer();
    }

    public void OpenDebugContainer()
    {
        container.OpenContextMenu();
    }
}
