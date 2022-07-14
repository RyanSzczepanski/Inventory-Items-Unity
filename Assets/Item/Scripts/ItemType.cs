using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ItemType
{
    Basic = 0 << 0,
    Useable = 1 << 1,
    Attachment = 1 << 2,
    Equipable = 1 << 3,
    Container = 1 << 4,
    All = Useable | Attachment | Equipable | Container
}
