using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ComponentController)), CanEditMultipleObjects]
public class ComponentControllerEditor : Editor
{
    private static List<ComponentController> _controllers = new();

    private static bool _isAutoRefresh = true;

    private static bool _prevAutoRefresh;
    public override void OnInspectorGUI()
    {
        
        GUILayout.BeginHorizontal("box");
        {
            _isAutoRefresh = GUILayout.Toggle(_isAutoRefresh, "AutoRefresh");

            if(_isAutoRefresh)
            {
                
                // 선택한 오브젝트들 중에서 ComponentController를 가진 객체들만 남겨둔다 (하이어라키가 바뀔경우 다시 Refresh하기 위해)
                _controllers = targets
                    .Select(t => t as ComponentController)
                    .Where(t => t != null).ToList();

                if(!_prevAutoRefresh)
                {
                    foreach (ComponentController componentController in _controllers)
                        componentController.Refresh();
                }
            }

            _prevAutoRefresh = _isAutoRefresh;
            
            if(GUILayout.Button("Refresh"))
            {
                // 선택한 오브젝트들 중에서 ComponentController를 가진 객체들만 남겨둔 후
                _controllers = targets
                    .Select(t => t as ComponentController)
                    .Where(t => t != null).ToList();

                // 그 Controller들을 다시 새로고침 한다
                foreach (ComponentController componentController in _controllers)
                    componentController.Refresh();
            }

        }
        GUILayout.EndHorizontal();

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
        if(!_isAutoRefresh) return;
        
        
        if(obj is not ComponentController controller) return;

        controller.Refresh();
    }

    private static void OnHierarchyChanged()
    {
        if(!_isAutoRefresh) return;
        
        foreach (ComponentController componentController in _controllers)
        {
            try
            {
                componentController.Refresh();
            }
            catch
            {
                // ignored
            }
        }
    }
}
