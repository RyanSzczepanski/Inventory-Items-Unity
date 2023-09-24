using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable: IUseableData
{
    public bool CanUse { get; }
    public bool TryUse();
}

public interface IUseableData
{

}

