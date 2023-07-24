using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGUIManager : MonoSingleton<OnGUIManager>
{
    private readonly Dictionary<string, string> _guiDict = new();

    [SerializeField]
    private int _fontSize = 30;
    
    public void SetGUI(string key, object value)
    {
        _guiDict[key] = value.ToString();
    }


    private void OnGUI()
    {
#if UNITY_EDITOR
        if(_guiDict.Count <= 0) return;
        
        GUIStyle label = new();
        label.normal.textColor = Color.red;
        label.fontSize = _fontSize;

        foreach (KeyValuePair<string, string> dict in _guiDict)
        {
            GUILayout.Label($"{dict.Key} : {dict.Value}", label);
        }
#endif
    }
}
