using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    private void Awake()
    {
        ItemDB.Init();

        //CreateAllItems();
    }

    public void CreateAllItems()
    {
        foreach (ItemSO item in ItemDB.GetValues())
        {
            Debug.Log($"Created {item.name}");
        }
    }
}
