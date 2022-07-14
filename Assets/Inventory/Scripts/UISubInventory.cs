using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public Inventory inventory;
}

public class UISubInventory : MonoBehaviour
{
    public Inventory inventory;
    public SubInventory subInventory;
}

public class UISlot : MonoBehaviour
{
    public Inventory inventory;
    public SubInventory subInventory;
    public Slot slot;

    public void Init(Inventory inventory, SubInventory subInventory, Slot slot)
    {
        this.inventory = inventory;
        this.subInventory = subInventory;
        this.slot = slot;
    }
}

