using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CacheEverything 
{
    private static readonly Dictionary<Collider2D, SelectableObject> CacheInteractObjects = new Dictionary<Collider2D, SelectableObject>();
    
    // ReSharper disable Unity.PerformanceAnalysis
    public static SelectableObject GetInteractObject(this Collider2D key)
    {
        if (!CacheInteractObjects.ContainsKey(key))
        {
            CacheInteractObjects.Add(key, key.GetComponent<SelectableObject>());
        }
        return CacheInteractObjects[key];
    }
}
