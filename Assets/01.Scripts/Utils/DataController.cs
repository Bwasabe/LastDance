using System;
using System.Collections.Generic;

public class DataController<TBase> where TBase : class
{
    private Dictionary<Type, TBase> _dataDict = new();
    
    public T GetData<T>() where T : class, TBase
    {
        if(_dataDict.TryGetValue(typeof(T), out TBase value))
            return value as T;
        else
            throw new SystemException($"{typeof(T)} is null in dictionary");
    }
    
    public void AddData(TBase data)
    {
        if(!_dataDict.TryAdd(data.GetType(), data))
            throw new SystemException($"{data.GetType()} is null or already exist in dictionary");
    }

    public void ClearData()
    {
        _dataDict.Clear();
    }
}
