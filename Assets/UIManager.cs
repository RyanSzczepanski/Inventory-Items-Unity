using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject inventoryItemPrefab;
    public GameObject rowPrefab;
    public GameObject inventoryPrefab;
    public GameObject miniInventoryPrefab;
    public GameObject contextMenuPrefab;

    private void Awake()
    {
        InventoryGrid.Init(slotPrefab, inventoryItemPrefab, rowPrefab, inventoryPrefab, miniInventoryPrefab);
        FloatingMenu.Init(contextMenuPrefab);
        Destroy(gameObject);
    }
}
