using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingMenu : MonoBehaviour
{
    public static GameObject prefab;

    public TMP_Text title;

    public static void Init(GameObject floatingMenuUIPrefab)
    {
        prefab = floatingMenuUIPrefab;
    }

    private static GameObject CreateMenu()
    {
        GameObject floatingMenu = Utils.InstantiateToCanvas(prefab);
        //Instantiate(content, floatingMenu.transform);
        return floatingMenu;
    }

    public static GameObject CreateMenu(GameObject content)
    {
        GameObject floatingMenu = CreateMenu();
        content.transform.SetParent(floatingMenu.transform, false);
        return floatingMenu;
    }

   public void CloseMenu()
    {
        Destroy(gameObject);
    }
}
