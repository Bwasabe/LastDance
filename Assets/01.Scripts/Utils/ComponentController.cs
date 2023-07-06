using System;
using System.Collections.Generic;
using ColorHierarchyNameSpace;
using UnityEngine;

[ExecuteAlways]
public class ComponentController : MonoBehaviour
{
    public static readonly Dictionary<Transform, ComponentController> Controllers = new();

    [SerializeField] private List<Component> _componentsList = new();

    private readonly Dictionary<Type, Component> _componentsDict = new();


    private void Awake()
    {
        Controllers.TryAdd(transform.root, this);

        foreach (Component component in _componentsList) _componentsDict.Add(component.GetType(), component);
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
        _componentsList.Clear();

        Component[] components = GetComponentsInChildren<Component>();

        Dictionary<Type, Component> componentDict = new();

        foreach (Component component in components)
        {
            if(component is Transform or ComponentController or ColorHierarchy) continue;

            if(componentDict.TryAdd(component.GetType(), component))
            {
                _componentsList.Add(component);
            }
        }
    }
}
