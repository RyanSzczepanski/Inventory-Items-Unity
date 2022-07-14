using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Assets/Item")]
public class ItemSO : ScriptableObject
{
    public Sprite texture;
    public GameObject itemPrefab;
    public GameObject worldPrefab;

    public ItemType type;

    public string fullName;
    public string shortName;
    public float weight;

    public Vector2Int size;

    public virtual Item CreateItem()
    {
        return new Item(this);
    }

    public virtual GameObject CreateItemWorldObject(Transform parent)
    {
        GameObject itemWorldObject = new GameObject(fullName);
        AddItemScript(itemWorldObject);
        return itemWorldObject;
    }

    public virtual GameObject CreateItemInventoryObject(Transform parent)
    {
        Debug.LogException(new System.NotImplementedException());
        return null;
    }

    public virtual void AddItemScript(GameObject gameObject)
    {
        //Item item = gameObject.AddComponent<Item>();
        //item.itemData = this;
    }
}