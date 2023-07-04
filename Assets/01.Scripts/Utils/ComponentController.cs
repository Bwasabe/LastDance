using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ComponentController : MonoBehaviour
{
    public static readonly Dictionary<Transform, ComponentController> Controllers = new();

    [SerializeField] private List<Component> componentsList = new();

    private readonly Dictionary<Type, Component> _componentsDict = new();
    
    
    private void Awake()
    {
        foreach (Component component in componentsList) _componentsDict.Add(component.GetType(), component);
    }

    public T Get<T>() where T : Component
    {
        if(_componentsDict.TryGetValue(typeof(T), out Component component)) return component as T;

        component = GetComponent<T>();
        _componentsDict.TryAdd(typeof(T), component);

        return (T)component;
    }

    public void Refresh()
    {
        componentsList.Clear();

        Component[] components = GetComponentsInChildren<Component>();

        foreach (Component component in components)
        {
            if(component is Transform or ComponentController) continue;
            
            componentsList.Add(component);
        }
    }
}