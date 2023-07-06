using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGUIManager : MonoSingleton<OnGUIManager>
{
    private readonly Dictionary<string, string> _guiDict = new();

    [SerializeField]
    private int _fontSize = 30;

    public void Init(string value)
    {
        if(!_guiDict.TryAdd(value, string.Empty))
        {
            throw new System.Exception($"{value} is already exist in Dict");
        }
    }

    public void Set(string key, string value)
    {
        _guiDict[key] = value;
    }


    private void OnGUI()
    {
#if UNITY_EDITOR
        if(_guiDict.Count <= 0) return;
        
        GUIStyle label = new();
        label.normal.textColor = Color.red;
        label.fontSize = _fontSize;

        foreach (string dict in _guiDict.Values)
        {
            GUILayout.Label(dict, label);
        }
#endif
    }
}
