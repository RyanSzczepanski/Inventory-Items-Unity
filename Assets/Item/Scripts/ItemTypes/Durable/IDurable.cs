using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDurable : IDurableData
{
    
}

public interface IDurableData 
{
    public float CurrentDurability { get; }
    public float MaxDurability { get; }
}
