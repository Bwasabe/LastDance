using System;
using System.Collections.Generic;
using ColorHierarchyNameSpace;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteAlways]
public class ComponentController : MonoBehaviour
{
    public static readonly Dictionary<Transform, ComponentController> Controllers = new();

    [SerializeField] private List<Component> _componentsList = new();

    private readonly Dictionary<Type, Component> _componentsDict = new();

    [ContextMenu("DebugDict")]
    private void DebugDict()
    {
        foreach ((Type type, Component component) in _componentsDict)
        {
            Debug.Log($"{type}, {component}");
        }
    }
    
    [ContextMenu("DebugList")]
    private void DebugList()
    {
        for (int i = 0; i < _componentsList.Count; i++)
        {
            Component component = _componentsList[i];
            Debug.Log($"{component.GetType()} / {i}");
        }
    }
    
    [ContextMenu("AddDict")]
    private void AddDict()
    {
        for (int i = 0; i < _componentsList.Count; i++)
        {
            Component component = _componentsList[i];
            Debug.Log($"{component.GetType()} / {i}" );

            if(_componentsDict.TryAdd(component.GetType(), component))
                throw new SystemException($"{component.GetType()} is overlaped from {component.name} oldComponent is {_componentsDict[component.GetType()].name}");
        }
    }
    
    [ContextMenu("ClearDict")]
    private void ClearDict()
    {
        _componentsDict.Clear();
    }

    private void Awake()
    {
        Controllers.TryAdd(transform.root, this);
        _componentsDict.Clear();
        for (int i = 0; i < _componentsList.Count; i++)
        {
            Component component = _componentsList[i];

            if(!_componentsDict.TryAdd(component.GetType(), component))
                throw new SystemException($"{component.GetType()} is overlaped from {component.name} oldComponent is {_componentsDict[component.GetType()].name}");
        }
    }

    public T Get<T>() where T : Component
    {
        if(_componentsDict.TryGetValue(typeof(T), out Component component)) return component as T;

        component = GetComponent<T>();

        return (T)component;
    }

    public void Refresh()
    {
        _componentsList.Clear();

        Component[] components = GetComponentsInChildren<Component>();

        Dictionary<Type, Component> componentDict = new();

        for (int i = 0; i < components.Length; i++)
        {
            Component component = components[i];

            if(component is Transform or ComponentController or ColorHierarchy) continue;

            if(component == null)
            {
                throw new NullReferenceException($"{components[i - 1].name} 's Component is missingScripts");
            }
            if(componentDict.TryAdd(component.GetType(), component))
            {
                _componentsList.Add(component);
            }
        }
    }
}
