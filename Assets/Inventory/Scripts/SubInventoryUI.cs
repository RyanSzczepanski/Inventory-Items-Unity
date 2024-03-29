using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public Inventory inventory;
}

public class SubInventoryUI : MonoBehaviour
{
    public static GameObject Generate(GameObject subInventoryPrefab, Transform parent, SubInventory subInventory)
    {
        GameObject subInventoryObj = Instantiate(subInventoryPrefab, parent);
        subInventoryObj.SetActive(false);
        subInventoryObj.name = "Sub Inventory";

        GameObject slotsContainer = subInventoryObj.transform.GetChild(0).gameObject;
        GameObject itemContainer = subInventoryObj.transform.GetChild(1).gameObject;

        slotsContainer.GetComponent<GridLayoutGroup>().constraintCount = subInventory.Width;

        //Component
        SubInventoryUI UISubInventory = subInventoryObj.AddComponent<SubInventoryUI>();

        UISubInventory.SubInventory = subInventory;
        UISubInventory.SubInventory.UISubInventory = UISubInventory;

        subInventoryObj.SetActive(true);
        return subInventoryObj;
    }

    public SubInventory SubInventory { get; private set; }

    private void OnEnable()
    {
        SubInventory.UISubInventory = this;
        SubInventory.OnAddItem += ItemUI.GenerateUIItem;
        //subInventory.OnRemoveItem += DestroyFunc;
    }
    private void OnDisable()
    {
        SubInventory.OnAddItem -= ItemUI.GenerateUIItem;
        //subInventory.OnRemoveItem -= DestroyFunc;
    }
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

