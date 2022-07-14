using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils
{
    public static GameObject InstantiateToCanvas(GameObject go)
    {
        return GameObject.Instantiate(go, GameObject.FindObjectOfType<Canvas>().transform);
    }
}
