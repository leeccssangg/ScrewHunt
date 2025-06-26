using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDelegateCacheable
{
    Dictionary<string, Delegate> CacheDelegateDic { get; set; }

    public T CacheDelegate<T>(string method) where T: class
    {
        CacheDelegateDic ??= new Dictionary<string, Delegate>();
        if (!CacheDelegateDic.TryGetValue(method, out Delegate del))
        {
            CacheDelegateDic[method] = del = Delegate.CreateDelegate(typeof(T), this, method);
        }
        if (del is not T)
        {
            Debug.LogError("Delegate type mismatch");
        }
        return del as T;
    }
}