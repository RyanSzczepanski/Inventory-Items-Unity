using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot
{
    public static int SlotWidth = 50;
    public Item item;

    public Slot()
    {
        item = null;
    }
}