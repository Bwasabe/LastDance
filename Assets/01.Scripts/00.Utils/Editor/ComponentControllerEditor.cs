using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ComponentController))]
public class ComponentControllerEditor : Editor
{
    private static ComponentController[] _controllers;

    private static bool _refreshComponentController = true;
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal("box");
        {
            _refreshComponentController = GUILayout.Toggle(_refreshComponentController, "AutoRefresh");
            
            if (GUILayout.Button("Refresh"))
            {
                ComponentController controller = (ComponentController)target;
                controller.Refresh();
            }

        }
        GUILayout.EndHorizontal();

        if(_refreshComponentController)
        {
            _controllers = targets
                .Select(target => target as ComponentController)
                .Where(target => target != null).ToArray();
        }
        
        base.OnInspectorGUI();
    }

    [InitializeOnLoadMethod]
    public static void Init()
    {
        if(!EditorApplication.isPlaying)
        {
            ObjectFactory.componentWasAdded -= OnComponentAdded;
            ObjectFactory.componentWasAdded += OnComponentAdded;

            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }
        else
        {
            ObjectFactory.componentWasAdded -= OnComponentAdded;
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
        }
    }

    private static void OnComponentAdded(Component obj)
    {
        if (obj is not ComponentController controller) return;
        controller.Refresh();
    }

    private static void OnHierarchyChanged()
    {
        Debug.Log("HierarchyChanged");
        foreach (ComponentController controller in ComponentController.Controllers.Values)
            controller.Refresh();
    }
}