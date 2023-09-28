using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static Canvas ActiveCanvas;
    [SerializeField] private Canvas activeCanvas;

    public GameObject itemUIPrefab;
    public GameObject slotPrefab;
    public GameObject rowPrefab;
    public GameObject inventoryPrefab;
    public GameObject miniInventoryPrefab;
    public GameObject floatingMenuPrefab;
    public GameObject dropDownMenuPrefab;
    public GameObject dropOptionPrefab;

    private void Awake()
    {
        ActiveCanvas = activeCanvas;
        InventoryGrid.Init(slotPrefab, rowPrefab, inventoryPrefab, miniInventoryPrefab);
        FloatingMenu.Init(floatingMenuPrefab);
        ContextMenu.Init(dropDownMenuPrefab, dropOptionPrefab);
        ItemUI.Init(itemUIPrefab);
        Destroy(gameObject);
    }
}
