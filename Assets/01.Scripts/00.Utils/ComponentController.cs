using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ComponentController : MonoBehaviour
{
    public static readonly Dictionary<Transform, ComponentController> Controllers = new();

    [SerializeField] private List<Component> componentsList = new();

    private readonly Dictionary<Type, Component> _componentsDict;
    private void Awake()
    {
        Controllers.TryAdd(transform, this);
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
        _componentsDict.Clear();

        Component[] components = GetComponentsInChildren<Component>();

        foreach (Component component in components)
            _componentsDict.TryAdd(component.GetType(),component);
    }
}

public static class ComponentControllerExtensions
{
    public static T GetComponentCache<T>(this Transform transform) where T : Component
    {
        Transform root = transform.root;

        if(ComponentController.Controllers.TryGetValue(root, out ComponentController controller)) return controller.Get<T>();

        controller = root.gameObject.AddComponent<ComponentController>();
        ComponentController.Controllers.Add(transform, controller);

        return controller.Get<T>();
    }

    public static T GetComponentCache<T>(this Component component) where T : Component
    {
        return component.transform.GetComponentCache<T>();
    }
}
