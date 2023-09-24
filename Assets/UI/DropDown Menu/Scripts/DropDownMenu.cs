using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropDownMenu : MonoBehaviour
{
    public static GameObject DROP_DOWN_MENU_PREFAB;
    public static GameObject OPTION_PREFAB;

    public bool destroyOnNewLoad;
    //TODO: Implement DestroyOnNewLoad
    public static event NewLoad OnNewLoad;
    public delegate void NewLoad();


    public static void Init(GameObject ddmPrefab, GameObject ddmoPrefab)
    {
        DROP_DOWN_MENU_PREFAB = ddmPrefab;
        OPTION_PREFAB = ddmoPrefab;
    }

    public static GameObject CreateMenu(DropDownMenuOptions[] options, DropDownMenuSettings settings)
    {
        GameObject dropDownMenu = Utils.InstantiateToCanvas(DROP_DOWN_MENU_PREFAB);
        DropDownMenu newMenu = dropDownMenu.AddComponent<DropDownMenu>();
        newMenu.destroyOnNewLoad = settings.destroyOnNewLoad;
        if (OnNewLoad is not null) { OnNewLoad(); }
        OnNewLoad += newMenu.DestroyDropDown;
        foreach (DropDownMenuOptions ddmp in options)
        {
            GameObject option = Instantiate(OPTION_PREFAB, dropDownMenu.transform);
            option.GetComponentInChildren<Button>().onClick.AddListener(ddmp.action);
            option.GetComponentInChildren<Button>().onClick.AddListener(delegate () { newMenu.DestroyDropDown(); });
            option.GetComponentInChildren<TMP_Text>().text = ddmp.optionText;
        }
        switch (settings.creationLocation)
        {
            case CreateLocation.Cursor:
                RectTransform rect = newMenu.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0, 1);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 0);
                Vector2 pos = Input.mousePosition * UIManager.ActiveCanvas.scaleFactor;
                rect.position = pos;
                break;
            default:
                break;
        }
        return dropDownMenu;
    }
    public static void Test()
    {
        Debug.Log("OnClick");
    }
    public void DestroyDropDown()
    {
        OnNewLoad -= DestroyDropDown;
        Destroy(this.gameObject);
    }
}
public struct DropDownMenuOptions
{
    public string optionText;
    public UnityAction action;
}
public struct DropDownMenuSettings
{
    public bool destroyOnNewLoad;
    public CreateLocation creationLocation;
}

public enum CreateLocation
{
    Cursor,
    Center,
}


public class EV : EventArgs
{
    public ItemBasic item;
    public SubInventory subInventory;
}

