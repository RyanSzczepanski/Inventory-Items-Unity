using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectDatabase<T> : MonoBehaviour where T : ScriptableObject
{
    protected static Dictionary<string, T> objects;
    public static void Init()
    {
        objects = new Dictionary<string, T>();
        var objectArray = Resources.LoadAll<T>("");
        foreach (var obj in objectArray)
        {
            if (objects.ContainsKey(obj.name))
            {
                Debug.LogError($"Duplicate Entry \'{obj.name}\'");
                return;
            }
            objects[obj.name] = obj;
        }
        #if UNITY_EDITOR
        Debug.Log($"Created A Database Of With {objects.Count} Entries");
        #endif
    }

    public static T GetObjectByName(string name)
    {
        if (!objects.ContainsKey(name))
        {
            Debug.LogError($" No object with name \"{name}\" in database");
            return null;
        }
        
        return objects[name];
    }

    public static Dictionary<string, T>.ValueCollection GetValues()
    {
        return objects.Values;
    }

    public static T[] GetObjectArray()
    {
        T[] output;
        output = new T[objects.Count];
        objects.Values.CopyTo(output, 0);
        return output;
    }

    public static int GetLength()
    {
        return objects.Count;
    }
}
