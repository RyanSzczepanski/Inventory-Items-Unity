using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Assets/Item")]
public class ItemBasicSO : ScriptableObject
{
    public Sprite texture;
    public GameObject itemPrefab;
    public GameObject worldPrefab;

    public ItemType type;

    public string fullName;
    public string shortName;
    public float weight = 1;

    public Vector2Int size = Vector2Int.one;

    public virtual ItemBasic CreateItem()
    {
        return new ItemBasic(this);
    }
}