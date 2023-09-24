using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDurable : ItemBasic, IDurable
{
    public new readonly ItemDurableSO data;
    public float CurrentDurability
    {
        get => _currentDurability;
        private set => _currentDurability = value;
    }
    public float MaxDurability
    {
        get => _maxDurability;
        private set => _maxDurability = value;
    }

    private float _currentDurability;
    private float _maxDurability;

    public ItemDurable(ItemDurableSO itemSO): base(itemSO)
    {
        data = itemSO;
    }
}
