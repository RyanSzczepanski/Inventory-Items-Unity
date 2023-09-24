using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Durable", menuName = "Assets/Durable")]
public class ItemDurableSO : ItemBasicSO, IDurableData
{
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

    [SerializeField] private float _currentDurability;
    [SerializeField] private float _maxDurability;

    public override ItemBasic CreateItem()
    {
        return new ItemDurable(this);
    }
}
