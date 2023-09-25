using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.Progress;

public interface IContainer : IContainerData
{

}

public interface IContainerData
{
    public Arrangement Arrangement { get; }
}

[System.Serializable]
public struct Arrangement
{
    public bool HasSubInventory { get => subInventory != Vector2Int.zero; }
    public bool IsLeaf { get => childArrangements is null || childArrangements.Length == 0; }
    [HideIf(nameof(HasSubInventory))] public ArrangementDirection arrangementDirection;
    [HideIf(nameof(HasSubInventory))] public ArrangementAlignment arrangementAlignment;
    [HideIf(nameof(HasSubInventory))] public Arrangement[] childArrangements;
    [ShowIf(nameof(IsLeaf))] public Vector2Int subInventory;

    public Arrangement[] TreeToArray
    {
        get
        {
            List<Arrangement> arrangements = new List<Arrangement> { this };
            if (IsLeaf) { return arrangements.ToArray(); }
            foreach (var item in childArrangements)
            {
                arrangements.AddRange(item.TreeToArray);
            }
            return arrangements.ToArray();
        }
    }
    public Vector2Int[] SubInventories
    {
        get
        {
            List<Vector2Int> arrangements = new List<Vector2Int>();
            if (HasSubInventory) { arrangements.Add(this.subInventory); }
            if (IsLeaf) { return arrangements.ToArray(); }
            foreach (var item in childArrangements)
            {
                arrangements.AddRange(item.SubInventories);
            }
            return arrangements.ToArray();
        }
    }
}

public enum ArrangementDirection
{
    Horizontal,
    Vertical
}
public enum ArrangementAlignment
{
    TopLeft,
    TopCenter,
    TopRight,
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
    BottomLeft,
    BottomCenter,
    BottomRight,
}