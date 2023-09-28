using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour, IPointerClickHandler
{
    public static GameObject DROP_DOWN_MENU_PREFAB;
    public static GameObject OPTION_PREFAB;

    public bool destroyOnNewLoad;
    //TODO: Implement DestroyOnNewLoad
    public static event NewLoad DestroyMenu;
    public delegate void NewLoad();


    public static void Init(GameObject ddmPrefab, GameObject ddmoPrefab)
    {
        DROP_DOWN_MENU_PREFAB = ddmPrefab;
        OPTION_PREFAB = ddmoPrefab;
    }

    public static GameObject CreateMenu(ContextMenuOption[] options, ContextMenuSettings settings)
    {
        GameObject dropDownMenu = Utils.InstantiateToCanvas(DROP_DOWN_MENU_PREFAB);

        switch (settings.creationLocation)
        {
            case CreateLocation.Cursor:
                RectTransform rect = dropDownMenu.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0, 1);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 0);
                Vector2 pos = Input.mousePosition * UIManager.ActiveCanvas.scaleFactor;
                rect.position = pos;
                break;
            default:
                break;
        }

        ContextMenu newMenu = dropDownMenu.AddComponent<ContextMenu>();
        newMenu.destroyOnNewLoad = settings.destroyOnNewLoad;
        if (DestroyMenu is not null) { DestroyMenu(); }
        DestroyMenu += newMenu.DestroyDropDown;
        foreach (ContextMenuOption ddmp in options)
        {
            GameObject option = Instantiate(OPTION_PREFAB, dropDownMenu.transform);
            option.GetComponentInChildren<Button>().onClick.AddListener(ddmp.action);
            option.GetComponentInChildren<Button>().onClick.AddListener(delegate () { newMenu.DestroyDropDown(); });
            option.GetComponentInChildren<TMP_Text>().text = ddmp.optionText;
        }
        
        return dropDownMenu;
    }

    public void DestroyDropDown()
    {
        DestroyMenu -= DestroyDropDown;
        Destroy(this.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DestroyMenu();
    }
}
public struct ContextMenuOption
{
    public string optionText;
    public UnityAction action;
}
public struct ContextMenuSettings
{
    public bool destroyOnNewLoad;
    public CreateLocation creationLocation;
}

public enum CreateLocation
{
    Cursor,
    Center,
}