using UnityEngine;

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
